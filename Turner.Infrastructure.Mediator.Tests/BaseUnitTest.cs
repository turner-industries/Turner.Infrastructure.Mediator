using System.Reflection;
using NUnit.Framework;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Turner.Infrastructure.Mediator.Configuration;

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
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            Container.ConfigureMediator(new[]
            {
                GetType().GetTypeInfo().Assembly
            });
        }
    }
}