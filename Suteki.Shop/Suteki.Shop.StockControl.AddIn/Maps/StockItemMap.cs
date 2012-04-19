using FluentNHibernate.Mapping;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Maps
{
    public class StockItemMap : ClassMap<StockItem>
    {
        public StockItemMap()
        {
            Id(x => x.Id);

            Map(x => x.Level);
            Map(x => x.IsActive);
            Map(x => x.ProductName);
            Map(x => x.SizeName);
            Map(x => x.IsInStock);

            HasMany(x => x.History).Cascade.AllDeleteOrphan().Inverse();
        }
    }
}