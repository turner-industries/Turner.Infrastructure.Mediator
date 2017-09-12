using System.Reflection;
using SimpleInjector;
using Turner.Infrastructure.Mediator.Decorators;

namespace Turner.Infrastructure.Mediator.Configuration
{
    public static class SimpleInjectorMediatorConfiguration
    {
        public static void ConfigureMediator(this Container container, Assembly[] assemblies)
        {
            RegisterMediator(container, assemblies);
        }

        private static void RegisterMediator(Container container, Assembly[] assemblies)
        {
            container.Register<IMediator>(() => new Mediator(container.GetInstance));
            container.Register(typeof(IRequestHandler<>), assemblies);
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.RegisterDecorator(typeof(IRequestHandler<>), typeof(ValidationHandler<>));
            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ValidationHandler<,>));
            container.RegisterDecorator(typeof(IRequestHandler<>), typeof(TransactionHandler<>));
            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionHandler<,>));
        }
    }
}
