namespace Turner.Infrastructure.Mediator
{
    public interface IStaticDispatcher
    {
        object Dispatch(object request);
    }
}
