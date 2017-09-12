using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using Turner.Infrastructure.Mediator.Decorators;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class MediatorPipelineTests : BaseUnitTest
    {
        [Test]
        public async Task Handle_NoResponseData_ReturnsResponseObject()
        {
            // Act
            var response = await Mediator.HandleAsync(new RequestWithoutResponse());

            // Assert
            response.HasErrors.ShouldBe(false);
        }

        [Test]
        public async Task Handle_HasResponseData_ReturnsResponseObjectWithData()
        {
            // Act
            var response = await Mediator.HandleAsync(new RequestWithResponse());

            // Assert
            response.HasErrors.ShouldBe(false);
            response.Data.ShouldBe("Bar");
        }
    }

    [DoNotValidate]
    public class RequestWithoutResponse : IRequest
    {
    }

    public class RequestWithoutResponseHandler : IRequestHandler<RequestWithoutResponse>
    {
        public Task<Response> HandleAsync(RequestWithoutResponse request)
        {
            return request.SuccessAsync();
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