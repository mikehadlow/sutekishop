using System;
using System.Collections.Generic;

namespace Suteki.Shop.Tests.Maps
{
    public class ModelTestData
    {
        public static IEnumerable<Type> AllEntityTypes()
        {
            return new[]
            {
                typeof (PostZone),
                typeof (Postage),
                typeof (Country),
                typeof (CardType),
                typeof (Card),
                typeof (Contact),
                typeof (MailingListSubscription),
                typeof (Order),
                typeof (OrderStatus),
                typeof (User),
                typeof (Role),
                typeof (Basket),
                typeof (BasketItem),
                typeof (Size),
                typeof (Product),
                typeof (Review),
                typeof (ProductImage),
                typeof (Image),
                typeof (ProductCategory),
                typeof (Category),
                typeof (Menu),
                typeof (TextContent),
                typeof (ActionContent),
                typeof (TopContent)
            };
        }
    }
}