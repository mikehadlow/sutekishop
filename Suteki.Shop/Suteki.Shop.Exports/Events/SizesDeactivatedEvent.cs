using Suteki.Common.Events;

namespace Suteki.Shop.Exports.Events
{
    public class SizesDeactivatedEvent : IDomainEvent
    {
        public string ProductName { get; private set; }

        public SizesDeactivatedEvent(string productName)
        {
            ProductName = productName;
        }
    }
}