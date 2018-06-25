using System;

namespace Turner.Infrastructure.Mediator.BackgroundJobs
{
    public interface IBackgroundJobMediator
    {
        void Enqueue<T>(T command) where T : IRequest;
        void Schedule<T>(T command, TimeSpan delay) where T : IRequest;
        void Schedule<T>(string name, T command, string cron) where T : IRequest;
    }
}