using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimpleInjector;
using Turner.Infrastructure.Mediator.Configuration;
using Turner.Infrastructure.Mediator.Tests.Fakes;

namespace Turner.Infrastructure.Mediator.Tests
{
    public class BaseUnitTest
    {
        protected Container Container { get; set; }

        protected IMediator Mediator => Container.GetInstance<IMediator>();
        
        [SetUp]
        public void SetUp()
        {
            Container = new Container();
            var assemblies = new[]
            {
                GetType().GetTypeInfo().Assembly
            };
            Container.ConfigureMediator(assemblies);
            Container.RegisterTransactionHandlers(assemblies);
            Container.RegisterSingleton<DbContext>(() => new FakeDbContext());
        }
    }
}