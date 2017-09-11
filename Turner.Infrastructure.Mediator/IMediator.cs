using System.Threading.Tasks;

namespace Turner.Infrastructure.Mediator
{
    public interface IMediator
    {
        Task<Response> HandleAsync(IRequest request);
        Task<Response<TResult>> HandleAsync<TResult>(IRequest<TResult> request);
    }
}
