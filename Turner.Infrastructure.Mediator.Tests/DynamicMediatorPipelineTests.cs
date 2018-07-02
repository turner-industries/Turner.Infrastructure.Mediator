using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;
using SimpleInjector;
using System.Reflection;
using System.Threading.Tasks;
using Turner.Infrastructure.Mediator.Configuration;
using Turner.Infrastructure.Mediator.Tests.Fakes;

namespace Turner.Infrastructure.Mediator.Tests
{
    [TestFixture]
    public class DynamicMediatorPipelineTests
    {
        private Container Container { get; set; }

        private IMediator Mediator => Container.GetInstance<IMediator>();

        [SetUp]
        public void SetUp()
        {
            Container = new Container();
            Container.ConfigureDynamicMediator(new[]
            {
                GetType().GetTypeInfo().Assembly
            });

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
    }
}