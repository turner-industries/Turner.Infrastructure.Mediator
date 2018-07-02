namespace Turner.Infrastructure.Mediator.Mediators
{
    public interface IStaticDispatcher
    {
        object Dispatch(object request);
    }
}
