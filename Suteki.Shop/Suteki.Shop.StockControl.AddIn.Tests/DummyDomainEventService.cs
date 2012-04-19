using System;
using Suteki.Common.Events;

namespace Suteki.Shop.StockControl.AddIn.Tests
{
    public class DummyDomainEventService : IDomainEventService
    {
        public Action<IDomainEvent> RaiseDelegate { get; set; } 

        public void Raise<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            RaiseDelegate(@event);
        }
    }
}