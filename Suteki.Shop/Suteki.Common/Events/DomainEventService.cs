namespace Suteki.Common.Events
{
    public interface IDomainEventService
    {
        void Raise<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
    }

    public class DomainEventService : IDomainEventService
    {
        public void Raise<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            DomainEvent.Raise(@event);
        }
    }
}