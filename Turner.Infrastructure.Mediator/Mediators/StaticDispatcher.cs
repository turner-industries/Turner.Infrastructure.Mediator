using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Expr = System.Linq.Expressions.Expression;

namespace Turner.Infrastructure.Mediator
{
    public class StaticDispatcher : IStaticDispatcher
    {
        private readonly Func<Type, object> _handlerFactory;
        private readonly Dictionary<Type, Func<object, object>> _dispatchers;
          
        public StaticDispatcher(Func<Type, object> handlerFactory, params Assembly[] assemblies)
        {
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _dispatchers = new Dictionary<Type, Func<object, object>>();

            Register(handlerFactory, assemblies);
        }

        public object Dispatch(object request) =>
            _dispatchers.ContainsKey(request.GetType()) 
                ? _dispatchers[request.GetType()](request)
                : Register(request.GetType())(request);

        private void Register(Func<Type, object> handlerFactory, IEnumerable<Assembly> assemblies)
        {
            _dispatchers.Clear();

            var classTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && !type.IsInterface)
                .ToArray();

            var requestTypesWithoutResult = classTypes
                .SelectMany(type => type.GetInterfaces()
                    .Where(t => t.IsGenericType && typeof(IRequestHandler<>) == t.GetGenericTypeDefinition()))
                .Select(type => type.GenericTypeArguments)
                .Where(types => types.All(t => !t.IsGenericParameter && !t.IsGenericType));

            foreach (var types in requestTypesWithoutResult)
            {
                var requestType = types[0];
                var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);

                _dispatchers[requestType] = CreateDispatcher(handlerType, requestType);
            }

            var requestTypesWithResult = classTypes
                .SelectMany(type => type.GetInterfaces()
                    .Where(t => t.IsGenericType && typeof(IRequestHandler<,>) == t.GetGenericTypeDefinition()))
                .Select(type => type.GenericTypeArguments)
                .Where(types => types.All(t => !t.IsGenericParameter && !t.IsGenericType));

            foreach (var types in requestTypesWithResult)
            {
                var requestType = types[0];
                var resultType = types[1];
                var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, resultType);

                _dispatchers[requestType] = CreateDispatcher(handlerType, requestType);
            }
        }

        private Func<object, object> Register(Type requestType)
        {
            Func<object, object> dispatcher = null;

            if (requestType.GetInterfaces().Any(t => t == typeof(IRequest)))
            {
                var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
                dispatcher = CreateDispatcher(handlerType, requestType);
            }
            else
            {
                var requestInterface = requestType.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IRequest<>))
                    ?? throw new ArgumentException($"{requestType} does not implement IRequest or IRequest<>");

                var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, requestInterface.GetGenericArguments()[0]);
                dispatcher = CreateDispatcher(handlerType, requestType);
            }

            _dispatchers[requestType] = dispatcher;
            return dispatcher;
        }

        private Func<object, object> CreateDispatcher(Type handlerType, Type requestType)
        {
            var handleAsyncMethod = handlerType.GetMethod("HandleAsync", new[] { requestType })
                ?? throw new NotImplementedException();

            var factoryTargetExpr = Expr.Constant(_handlerFactory.Target);
            var handlerTypeExpr = Expr.Constant(handlerType);
            var boxedHandlerExpr = Expr.Call(factoryTargetExpr, _handlerFactory.Method, handlerTypeExpr);
            var boxedRequestExpr = Expr.Parameter(typeof(object));
            var handlerExpr = Expr.Convert(boxedHandlerExpr, handlerType);
            var requestExpr = Expr.Convert(boxedRequestExpr, requestType);
            var callHandleExpr = Expr.Call(handlerExpr, handleAsyncMethod, requestExpr);

            return Expr.Lambda<Func<object, object>>(callHandleExpr, boxedRequestExpr).Compile();
        }
    }
}
