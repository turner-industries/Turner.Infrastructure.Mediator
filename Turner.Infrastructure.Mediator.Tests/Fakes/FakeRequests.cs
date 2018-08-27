using System.Threading.Tasks;
using Turner.Infrastructure.Mediator.Decorators;

namespace Turner.Infrastructure.Mediator.Tests.Fakes
{
    [DoNotValidate]
    public class RequestWithoutResponse : IRequest
    {
    }

    public class RequestWithoutResponseHandler : IRequestHandler<RequestWithoutResponse>
    {
        public Task<Response> HandleAsync(RequestWithoutResponse request)
        {
            return Response.SuccessAsync();
        }
    }

    [DoNotValidate]
    public class RequestWithResponse : IRequest<string>
    {
        public string Foo { get; set; } = "Bar";
    }

    public class RequestWithResponseHandler : IRequestHandler<RequestWithResponse, string>
    {
        public Task<Response<string>> HandleAsync(RequestWithResponse request)
        {
            return request.Foo.AsResponseAsync();
        }
    }
}
