using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using Turner.Infrastructure.Mediator.Decorators;
using Turner.Infrastructure.Mediator.Tests.Fakes;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class TransactionHandlerTests : BaseUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task OneHandler_CreatesNewTransaction()
        {
            // Act
            await Mediator.HandleAsync(new FirstRequest());

            // Assert
            FakeDatabaseFacade.TransactionCount.ShouldBe(1);
        }

        [Test]
        public async Task NestedHandler_CreatesOneTransaction()
        {
            // Act
            await Mediator.HandleAsync(new ParentRequest());

            // Assert
            FakeDatabaseFacade.TransactionCount.ShouldBe(1);
        }

        [Test]
        public async Task ErrorOnRequest_RollsBackTransaction()
        {
            // Act
            await Mediator.HandleAsync(new ErrorRequest());

            // Assert
            FakeDatabaseFacade.TransactionCount.ShouldBe(1);
            FakeDatabaseFacade.RollbackCount.ShouldBe(1);
            FakeDatabaseFacade.CommitCount.ShouldBe(0);
        }

        [Test]
        public async Task NoErrors_CommitsTransaction()
        {
            // Act
            await Mediator.HandleAsync(new FirstRequest());

            // Assert
            FakeDatabaseFacade.TransactionCount.ShouldBe(1);
            FakeDatabaseFacade.RollbackCount.ShouldBe(0);
            FakeDatabaseFacade.CommitCount.ShouldBe(1);
        }
    }

    [DoNotValidate]
    public class FirstRequest : IRequest
    {
    }

    public class FirstRequestHandler : IRequestHandler<FirstRequest>
    {
        public Task<Response> HandleAsync(FirstRequest request)
        {
            return Response.SuccessAsync();
        }
    }

    [DoNotValidate]
    public class ParentRequest : IRequest
    {
    }

    public class ParentRequestHandler : IRequestHandler<ParentRequest>
    {
        private readonly IRequestHandler<FirstRequest> _nestedRequestHandler;

        public ParentRequestHandler(IRequestHandler<FirstRequest> nestedRequestHandler)
        {
            _nestedRequestHandler = nestedRequestHandler;
        }

        public Task<Response> HandleAsync(ParentRequest request)
        {
            return _nestedRequestHandler.HandleAsync(new FirstRequest());
        }
    }

    [DoNotValidate]
    public class ErrorRequest : IRequest
    {
    }

    public class ErrorRequestHandler : IRequestHandler<ErrorRequest>
    {
        public Task<Response> HandleAsync(ErrorRequest request)
        {
            return Error.AsResponseAsync("Error.");
        }
    }
}
