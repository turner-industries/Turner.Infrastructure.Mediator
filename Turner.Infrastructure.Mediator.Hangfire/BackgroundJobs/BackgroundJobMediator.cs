using System;
using Hangfire;

namespace Turner.Infrastructure.Mediator.BackgroundJobs
{
    public class BackgroundJobMediator : IBackgroundJobMediator
    {
        public void Enqueue<T>(T command) where T : IRequest
        {
            BackgroundJob.Enqueue<BackgroundJobExecutor<T>>(x => x.Execute(command));
        }

        public void Schedule<T>(T command, TimeSpan delay) where T : IRequest
        {
            BackgroundJob.Schedule<BackgroundJobExecutor<T>>(x => x.Execute(command), delay);
        }

        public void Schedule<T>(string name, T command, string cron) where T : IRequest
        {
            RecurringJob.AddOrUpdate<BackgroundJobExecutor<T>>(name, x => x.Execute(command), cron);
        }
    }
}