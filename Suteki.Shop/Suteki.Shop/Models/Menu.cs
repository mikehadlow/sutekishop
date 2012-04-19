using Microsoft.Web.Mvc;
using Suteki.Shop.Controllers;
using System.Web.Mvc;

namespace Suteki.Shop
{
    public class Menu : Content
    {
        public const int MainMenuId = 1;

        public override MvcHtmlString EditLink(HtmlHelper htmlHelper)
        {
            return htmlHelper.ActionLink<MenuController>(c => c.Edit(Id), "Edit");
        }

        public virtual bool IsMainMenu
        {
            get
            {
                return Id == MainMenuId;
            }
        }

		public static Menu CreateDefaultMenu(int position, Menu parent)
		{
			return new Menu 
			{
				ParentContent = parent,
				IsActive = true,
				Position = position
			};
		}
    }
}
