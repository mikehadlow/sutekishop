using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class ActionContentMap : SubclassMap<ActionContent>
    {
        public ActionContentMap()
        {
            Map(x => x.Controller);
            Map(x => x.Action);
        }
    }
}