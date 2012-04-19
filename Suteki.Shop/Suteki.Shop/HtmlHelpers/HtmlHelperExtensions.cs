using System;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using MvcContrib;
using MvcContrib.FluentHtml.Elements;
using Suteki.Common.Extensions;
using Suteki.Common.HtmlHelpers;
using Suteki.Shop.Controllers;
using System.Web.Mvc.Html;
using Suteki.Shop.Exports.HtmlHelpers;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static string LoginStatus(this HtmlHelper htmlHelper)
        {
            return htmlHelper.CurrentUser().PublicIdentity;
        }

        public static MvcHtmlString LoginLink(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.CurrentUser().CanLogin)
            {
                return htmlHelper.ActionLink<LoginController>(c => c.Logout(), "Logout");
            }
            return htmlHelper.ActionLink<LoginController>(c => c.Index(), "Login").ToSslLink();
        }

        public static User CurrentUser(this HtmlHelper htmlHelper)
        {
            var user = htmlHelper.ViewContext.HttpContext.User as User;
            if (user == null) throw new ApplicationException("Current context user cannot be cast to Suteki.Shop.User");
            return user;
        }

        public static string WriteCategories(this HtmlHelper htmlHelper, CategoryViewData rootCategory, CategoryDisplay display)
        {
            var categoryWriter = new CategoryWriter(rootCategory, htmlHelper, display);
            return categoryWriter.Write();
        }

        public static string WriteStock(this HtmlHelper htmlHelper, Category rootCategory)
        {
            var stockWriter = new StockWriter(htmlHelper, rootCategory);
            return stockWriter.Write();
        }

        public static string WriteMenu(this HtmlHelper htmlHelper, Menu menu)
        {
            var menuWriter = new MenuWriter(htmlHelper, menu);
            return menuWriter.Write();
        }

        public static string WriteMenu(this HtmlHelper htmlHelper, Menu menu, object attributes)
        {
            var menuWriter = new MenuWriter(htmlHelper, menu, attributes);
            return menuWriter.Write();
        }

        public static string WriteMenu(this HtmlHelper htmlHelper, Menu menu, bool nest)
        {
            var menuWriter = new MenuWriter(htmlHelper, menu, nest, false);
            return menuWriter.Write();
        }

        public static string WriteMenu(this HtmlHelper htmlHelper, Menu menu, bool nest, object attributes)
        {
            var menuWriter = new MenuWriter(htmlHelper, menu, nest, attributes);
            return menuWriter.Write();
        }

		public static THelper Name<THelper>(this THelper helper, string name) where THelper : IElement
		{
			helper.Builder.MergeAttribute("name", name, true);
			return helper;
		}

		public static IDisposable MultipartForm(this HtmlHelper helper)
		{
			string action = helper.ViewContext.RouteData.GetRequiredString("action");
			string controller = helper.ViewContext.RouteData.GetRequiredString("controller");
			return helper.BeginForm(action, controller, FormMethod.Post, new Hash(enctype => "multipart/form-data"));
		}

		public static bool IsAdministrator(this IPrincipal principal)
		{
			return principal.IsInRole("Administrator");
		}

		public static void InitialiseRichTextEditor(this HtmlHelper helper)
		{
			helper.RenderPartial("TinyMce");
		}

		public static string Stars(this HtmlHelper helper, int numberOfStars)
		{
			var sb = new StringBuilder();
			string url = new UrlHelper(helper.ViewContext.RequestContext).Content("~/content/images/star.gif");
			for (int i = 0; i < numberOfStars; i++) {
				sb.AppendFormat("<img src='{0}' alt='Star' />", url);
			}

			return sb.ToString();
		}

        public static void StockControlUi<TModel>(this HtmlHelper<TModel> htmlHelper, string productUrlName)
        {
            htmlHelper.With<IStockControlUi, TModel>(service => service.RenderForProductId(htmlHelper, productUrlName));
        }
    }
}
