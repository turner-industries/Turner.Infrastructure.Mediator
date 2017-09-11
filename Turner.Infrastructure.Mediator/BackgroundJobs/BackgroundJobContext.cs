namespace Turner.Infrastructure.Mediator.BackgroundJobs
{
    public class BackgroundJobContext
    {
        public bool IsBackgroundJob { get; }

        public BackgroundJobContext(bool isBackgroundJob)
        {
            IsBackgroundJob = isBackgroundJob;
        }
    }
}
