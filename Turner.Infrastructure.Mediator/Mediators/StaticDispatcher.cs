using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Expr = System.Linq.Expressions.Expression;

namespace Turner.Infrastructure.Mediator.Mediators
{
    public class StaticDispatcher : IStaticDispatcher
    {
        private Dictionary<Type, Func<object, object>> _dispatchers;

        public StaticDispatcher(Func<Type, object> handlerFactory, params Assembly[] assemblies)
        {
            Register(handlerFactory, assemblies);
        }

        public object Dispatch(object request) =>
            _dispatchers[request.GetType()](request);

        private void Register(Func<Type, object> handlerFactory, Assembly[] assemblies)
        {
            _dispatchers = new Dictionary<Type, Func<object, object>>();

            foreach (var assembly in assemblies)
            {
                var classTypes = assembly.GetTypes()
                    .Where(type => type.IsClass && !type.IsAbstract && !type.IsInterface)
                    .ToArray();

                foreach (var type in classTypes
                    .SelectMany(type => type.GetInterfaces()
                        .Where(t => t.IsGenericType && typeof(IRequestHandler<>) == t.GetGenericTypeDefinition()))
                    .Select(type => type.GetGenericArguments().First()))
                {
                    RegisterType(type, handlerFactory);
                }

                foreach (var types in classTypes
                    .SelectMany(type => type.GetInterfaces()
                        .Where(t => t.IsGenericType && typeof(IRequestHandler<,>) == t.GetGenericTypeDefinition()))
                    .Select(type => type.GetGenericArguments()))
                {
                    RegisterType(types[0], types[1], handlerFactory);
                }
            }
        }

        private void RegisterType(Type requestType, Func<Type, object> handlerFactory)
        {
            var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
            _dispatchers[requestType] = CreateDispatcher(handlerType, requestType, handlerFactory);
        }

        private void RegisterType(Type requestType, Type resultType, Func<Type, object> handlerFactory)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, resultType);
            _dispatchers[requestType] = CreateDispatcher(handlerType, requestType, handlerFactory);
        }
        
        private static Func<object, object> CreateDispatcher(Type handlerType, Type requestType, Func<Type, object> handlerFactory)
        {
            var handleAsyncMethod = handlerType.GetMethod("HandleAsync", new[] { requestType })
                ?? throw new NotImplementedException();

            var factoryTargetExpr = Expr.Constant(handlerFactory.Target);
            var handlerTypeExpr = Expr.Constant(handlerType);
            var boxedHandlerExpr = Expr.Call(factoryTargetExpr, handlerFactory.Method, handlerTypeExpr);
            var boxedRequestExpr = Expr.Parameter(typeof(object));
            var handlerExpr = Expr.Convert(boxedHandlerExpr, handlerType);
            var requestExpr = Expr.Convert(boxedRequestExpr, requestType);
            var callHandleExpr = Expr.Call(handlerExpr, handleAsyncMethod, requestExpr);

            return Expr.Lambda<Func<object, object>>(callHandleExpr, boxedRequestExpr).Compile();
        }
    }
}
