using SimpleInjector;
using System.Reflection;
using Turner.Infrastructure.Mediator.Decorators;

namespace Turner.Infrastructure.Mediator.Configuration
{
    public static class SimpleInjectorMediatorConfiguration
    {
        public static void RegisterTransactionHandlers(this Container container, Assembly[] assemblies)
        {
            container.RegisterDecorator(typeof(IRequestHandler<>), typeof(TransactionHandler<>));
            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionHandler<,>));
        }
    }
}
