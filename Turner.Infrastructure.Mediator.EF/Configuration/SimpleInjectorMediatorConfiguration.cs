using SimpleInjector;
using System.Reflection;
using Turner.Infrastructure.Mediator.Decorators;

namespace Turner.Infrastructure.Mediator.Configuration.EF
{
    public static class SimpleInjectorMediatorConfiguration
    {
        private static void RegisterHandlers(Container container, Assembly[] assemblies)
        {
            container.RegisterDecorator(typeof(IRequestHandler<>), typeof(TransactionHandler<>));
            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionHandler<,>));
        }
    }
}
