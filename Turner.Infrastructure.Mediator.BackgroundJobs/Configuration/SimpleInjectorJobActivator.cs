using System;
using Hangfire;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Turner.Infrastructure.Mediator.BackgroundJobs.Configuration
{
    public class SimpleInjectorJobActivator : JobActivator
    {
        private readonly Container _container;

        public SimpleInjectorJobActivator(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            var scope = AsyncScopedLifestyle.BeginScope(_container);
            return new SimpleInjectorScope(_container, scope);
        }

        public override object ActivateJob(Type jobType)
        {
            return _container.GetInstance(jobType);
        }
    }

    class SimpleInjectorScope : JobActivatorScope
    {
        private readonly Container _container;
        private readonly Scope _scope;

        public SimpleInjectorScope(Container container, Scope scope)
        {
            _container = container;
            _scope = scope;
        }

        public override object Resolve(Type type)
        {
            return _container.GetInstance(type);
        }

        public override void DisposeScope()
        {
            _scope?.Dispose();
        }
    }
}
