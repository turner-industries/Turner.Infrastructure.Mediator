using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using SimpleInjector;
using Turner.Infrastructure.Mediator.Configuration;
using Turner.Infrastructure.Mediator.Decorators;
using Turner.Infrastructure.Mediator.Tests.Fakes;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class StaticMediatorPipelineTests
    {
        private Container Container { get; set; }

        private IMediator Mediator => Container.GetInstance<IMediator>();

        [SetUp]
        public void SetUp()
        {
            Container = new Container();
            Container.ConfigureStaticMediator(new[]
            {
                GetType().GetTypeInfo().Assembly
            });

            Container.Register(typeof(IRequestHandler<>), typeof(GenericHandler<>));
            Container.Register(typeof(IRequestHandler<,>), typeof(GenericHandlerWithResult<>));

            Container.RegisterSingleton<DbContext>(() => new FakeDbContext());
        }

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

        [Test]
        public async Task Handle_WithGenericBaseRequestHandlers_HandleRequests()
        {
            // Act
            var responseA = await Mediator.HandleAsync(new RequestA());
            var responseB = await Mediator.HandleAsync(new RequestB());

            // Assert
            responseA.HasErrors.ShouldBe(true);
            responseA.Errors[0].ErrorMessage.ShouldBe("RequestA");
            responseB.HasErrors.ShouldBe(true);
            responseB.Errors[0].ErrorMessage.ShouldBe("RequestB");
        }

        [Test]
        public async Task Handle_WithGenericBaseWithResultRequestHandlers_HandleRequests()
        {
            // Act
            var responseA = await Mediator.HandleAsync(new RequestWithResultA());
            var responseB = await Mediator.HandleAsync(new RequestWithResultB());

            // Assert
            responseA.HasErrors.ShouldBe(true);
            responseA.Errors[0].ErrorMessage.ShouldBe("RequestWithResultA, System.String");
            responseB.HasErrors.ShouldBe(true);
            responseB.Errors[0].ErrorMessage.ShouldBe("RequestWithResultB, System.String");
        }

        [Test]
        public async Task Handle_WithGenericRequestHandlers_HandleRequests()
        {
            // Act
            var responseA = await Mediator.HandleAsync(new GenericRequest<FakeEntityA>());
            var responseB = await Mediator.HandleAsync(new GenericRequest<FakeEntityB>());

            // Assert
            responseA.HasErrors.ShouldBe(true);
            responseA.Errors[0].ErrorMessage.ShouldBe("FakeEntityA");
            responseB.HasErrors.ShouldBe(true);
            responseB.Errors[0].ErrorMessage.ShouldBe("FakeEntityB");
        }

        [Test]
        public async Task Handle_WithGenericRequestWithResultHandlers_HandleRequests()
        {
            // Act
            var responseA = await Mediator.HandleAsync(new GenericRequestWithResult<FakeEntityA>());
            var responseB = await Mediator.HandleAsync(new GenericRequestWithResult<FakeEntityB>());

            // Assert
            responseA.HasErrors.ShouldBe(true);
            responseA.Errors[0].ErrorMessage.ShouldBe("FakeEntityA, FakeEntityA");
            responseB.HasErrors.ShouldBe(true);
            responseB.Errors[0].ErrorMessage.ShouldBe("FakeEntityB, FakeEntityB");
        }


        public class GenericBaseHandler<TRequest> : IRequestHandler<TRequest>
            where TRequest : IRequest
        {
            public Task<Response> HandleAsync(TRequest request) 
                => Error.AsResponseAsync(typeof(TRequest).Name);
        }

        public class GenericBaseHandlerWithResult<TRequest, TResult> : IRequestHandler<TRequest, TResult>
            where TRequest : IRequest<TResult>
        {
            public Task<Response<TResult>> HandleAsync(TRequest request)
                => Error.AsResponseAsync<TResult>($"{typeof(TRequest).Name}, {typeof(TResult)}");
        }

        [DoNotValidate]
        public class RequestA : IRequest { }

        [DoNotValidate]
        public class RequestWithResultA : IRequest<string> { }

        [DoNotValidate]
        public class RequestB : IRequest { }

        [DoNotValidate]
        public class RequestWithResultB : IRequest<string> { }

        public class ChildHandlerA : GenericBaseHandler<RequestA> { }

        public class ChildHandlerWithResultA : GenericBaseHandlerWithResult<RequestWithResultA, string> { }

        public class ChildHandlerB : GenericBaseHandler<RequestB> { }

        public class ChildHandlerWithResultB : GenericBaseHandlerWithResult<RequestWithResultB, string> { }

        public class FakeEntityA { }

        public class FakeEntityB { }

        [DoNotValidate]
        public class GenericRequest<T> : IRequest { }

        public class GenericHandler<T> : IRequestHandler<GenericRequest<T>>
        {
            public Task<Response> HandleAsync(GenericRequest<T> request) 
                => Error.AsResponseAsync(typeof(T).Name);
        }

        [DoNotValidate]
        public class GenericRequestWithResult<T> : IRequest<T> { }

        public class GenericHandlerWithResult<T> : IRequestHandler<GenericRequestWithResult<T>, T>
        {
            public Task<Response<T>> HandleAsync(GenericRequestWithResult<T> request) 
                => Error.AsResponseAsync<T>($"{typeof(T).Name}, {typeof(T).Name}");
        }
    }
}