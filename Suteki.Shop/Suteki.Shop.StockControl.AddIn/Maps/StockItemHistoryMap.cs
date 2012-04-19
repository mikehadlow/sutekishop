using FluentNHibernate.Mapping;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Maps
{
    public class StockItemHistoryMap : ClassMap<StockItemHistoryBase>
    {
        public StockItemHistoryMap()
        {
            Id(x => x.Id);

            Map(x => x.DateTime);
            Map(x => x.User).Column("`User`");
            Map(x => x.Level);
            Map(x => x.Comment);

            References(x => x.StockItem);

            DiscriminateSubClassesOnColumn("StockItemHistoryType");
        }
    }

    public class StockItemCreatedMap : SubclassMap<StockItemCreated>
    {
    }

    public class ReceivedStockMap : SubclassMap<ReceivedStock>
    {
        public ReceivedStockMap()
        {
            Map(x => x.NumberOfItemsRecieved);
        }
    }

    public class DispatchedStockMap : SubclassMap<DispatchedStock>
    {
        public DispatchedStockMap()
        {
            Map(x => x.NumberOfItemsDispatched);
            Map(x => x.OrderNumber);
        }
    }

    public class StockAdjustmentMap : SubclassMap<StockAdjustment>
    {
        public StockAdjustmentMap()
        {
            Map(x => x.NewLevel);
        }
    }

    public class StockItemDeactivatedMap : SubclassMap<StockItemDeactivated>
    {
    }

    public class StockItemProductNameChangedMap : SubclassMap<StockItemProductNameChanged>
    {
        public StockItemProductNameChangedMap()
        {
            Map(x => x.OldProductName);
            Map(x => x.NewProductName);
        }
    }

    public class StockItemSetOutOfStockMap : SubclassMap<StockItemSetOutOfStock>{}
    public class StockItemSetInStockMap : SubclassMap<StockItemSetInStock>{}
}