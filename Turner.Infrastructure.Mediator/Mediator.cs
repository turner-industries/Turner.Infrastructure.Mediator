using System;
using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator
{
    public class Mediator : IMediator
    {
        private readonly Func<Type, object> _resolver;

        public Mediator(Func<Type, object> resolver)
        {
            _resolver = resolver;
        }

        public Task<Response> HandleAsync(IRequest request)
        {
            dynamic type = typeof(IRequestHandler<>).MakeGenericType(request.GetType());
            return (Task<Response>)HandleBase(type, request);
        }

        public Task<Response<TResult>> HandleAsync<TResult>(IRequest<TResult> request)
        {
            var type = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
            return (Task<Response<TResult>>)HandleBase(type, request);
        }

        private object HandleBase(dynamic type, dynamic command)
        {
            var handler = _resolver(type);
            return handler.HandleAsync(command);
        }
    }
}