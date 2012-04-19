using Suteki.Common.Extensions;

namespace Suteki.Shop
{
    public class StockItem
    {
        public StockItem() { }

        public StockItem(Size size)
        {
            SizeId = size.Id;
            Name = "{0} - {1}".With(size.Product.Name, size.Name);
            IsInStock = size.IsInStock;
        }

        public int SizeId { get; set; }
        public string Name { get; set; }
        public bool IsInStock { get; set; }
    }
}
