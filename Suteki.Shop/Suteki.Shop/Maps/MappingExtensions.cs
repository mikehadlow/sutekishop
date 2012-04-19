using FluentNHibernate.Mapping;
using Suteki.Common.NHibernate;

namespace Suteki.Shop.Maps
{
    public static class MappingExtensions
    {
        public static PropertyPart Text(this PropertyPart propertyPart)
        {
            propertyPart.Length(10000);
            return propertyPart;
        }

        public static PropertyPart Money(this PropertyPart propertyPart)
        {
            propertyPart.CustomType<MoneyUserType>();
            return propertyPart;
        }
    }
}