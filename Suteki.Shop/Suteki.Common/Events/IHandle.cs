namespace Suteki.Common.Events
{
    public interface IHandle<TEvent> where TEvent : class, IDomainEvent
    {
        void Handle(TEvent @event);
    }
}