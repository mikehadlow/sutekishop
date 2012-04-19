using Suteki.Common.Events;

namespace Suteki.Shop.Exports.Events
{
    public class SizeCreatedEvent : IDomainEvent
    {
        public string ProductName { get; private set; }
        public string SizeName { get; private set; }
        public bool IsActive { get; private set; }

        public SizeCreatedEvent(string productName, string sizeName, bool isActive)
        {
            ProductName = productName;
            SizeName = sizeName;
            IsActive = isActive;
        }
    }
}