using System;
using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator.Mediators
{
    public class DynamicDispatchMediator : IMediator
    {
        private readonly Func<Type, object> _resolver;

        public DynamicDispatchMediator(Func<Type, object> resolver)
        {
            _resolver = resolver;
        }

        public Task<Response> HandleAsync(IRequest request)
        {
            var handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());
            return (Task<Response>)HandleBase(handlerType, request);
        }

        public Task<Response<TResult>> HandleAsync<TResult>(IRequest<TResult> request)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
            return (Task<Response<TResult>>)HandleBase(handlerType, request);
        }

        private object HandleBase(Type handlerType, dynamic request)
        {
            dynamic handler = _resolver(handlerType);
            return handler.HandleAsync(request);
        }
    }
}