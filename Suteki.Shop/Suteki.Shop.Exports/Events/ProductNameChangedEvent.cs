using Suteki.Common.Events;

namespace Suteki.Shop.Exports.Events
{
    public class ProductNameChangedEvent : IDomainEvent
    {
        public string OldProductName { get; private set; }
        public string NewProductName { get; private set; }

        public ProductNameChangedEvent(string oldProductName, string newProductName)
        {
            OldProductName = oldProductName;
            NewProductName = newProductName;
        }
    }
}