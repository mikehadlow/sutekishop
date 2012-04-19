using Suteki.Common.Models;

namespace Suteki.Shop.StockControl.AddIn.Tests
{
    public static class EntityTestExtensions
    {
        public static T SetId<T>(this T entity, int id) where T : IEntity
        {
            entity.Id = id;
            return entity;
        }
    }
}