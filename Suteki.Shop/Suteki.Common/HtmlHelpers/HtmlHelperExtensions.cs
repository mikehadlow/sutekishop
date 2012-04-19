using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.UI;
using Castle.Windsor;
using Suteki.Common.Extensions;
using Suteki.Common.Models;
using Suteki.Common.ViewData;
using Microsoft.Web.Mvc;
using Suteki.Common.Windsor;

namespace Suteki.Common.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Render an error box 
        /// </summary>
        /// <returns></returns>
        public static string ErrorBox(this HtmlHelper htmlHelper, IErrorViewData errorViewData)
        {
            if (errorViewData.ErrorMessage == null) return string.Empty;

            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            writer.AddAttribute("class", "error");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(errorViewData.ErrorMessage);
            writer.RenderEndTag();
            return writer.InnerWriter.ToString();
        }

        public static string MessageBox(this HtmlHelper htmlHelper, IMessageViewData messageViewData)
        {
            if (messageViewData.Message == null) return string.Empty;

            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            writer.AddAttribute("class", "message");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(messageViewData.Message);
            writer.RenderEndTag();
            return writer.InnerWriter.ToString();
        }

        public static string Tick(this HtmlHelper htmlHelper, bool ticked)
        {
            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            if (ticked)
            {
                writer.AddAttribute("class", "tick");
            }
            else
            {
                writer.AddAttribute("class", "cross");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
            writer.RenderEndTag();
            return writer.InnerWriter.ToString();
        }

        public static string UpArrowLink<T>(this HtmlHelper htmlHelper, Expression<Action<T>> action) where T : Controller
        {
            return StringExtensions.With("<a href=\"{0}\" class=\"arrowlink\">{1}</a>", htmlHelper.BuildUrlFromExpression<T>(action),
                htmlHelper.Image("~/Content/Images/Up.png", "Move Up"));
        }

        public static string DownArrowLink<T>(this HtmlHelper htmlHelper, Expression<Action<T>> action) where T : Controller
        {
            return "<a href=\"{0}\" class=\"arrowlink\">{1}</a>".With(
                htmlHelper.BuildUrlFromExpression<T>(action),
                htmlHelper.Image("~/Content/Images/Down.png", "Move Down"));
        }

        public static string CrossLink<T>(this HtmlHelper htmlHelper, Expression<Action<T>> action, string alt) where T : Controller
        {
            return "<a href=\"{0}\">{1}</a>".With(
                htmlHelper.BuildUrlFromExpression<T>(action),
                htmlHelper.Image("~/Content/Images/Cross.png", alt));
        }

		public static string CrossLink<T>(this HtmlHelper htmlHelper, Expression<Action<T>> action) where T : Controller 
		{
			return htmlHelper.CrossLink(action, "Disabled");
		}


        public static string Pager(
        this HtmlHelper htmlHelper,
        IPagedList pagedList)
        {
            string controller = htmlHelper.ViewContext.RouteData.Values["controller"].ToString();
            string action = htmlHelper.ViewContext.RouteData.Values["action"].ToString();

            return htmlHelper.Pager(controller, action, pagedList);
        }

        public static string Pager(
            this HtmlHelper htmlHelper,
            string controller,
            string action,
            IPagedList pagedList)
        {
            var pageListBuilder = new Pager(htmlHelper, controller, action, pagedList);
            return pageListBuilder.WriteHtml();
        }

        public static string With<TService, TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return htmlHelper.With<TService, TModel>(service => service.ToString());
        }

        public static string With<TService, TModel>(this HtmlHelper<TModel> htmlHelper, Func<TService, string> useServiceAction)
        {
            var service = IocContainer.Resolve<TService>();
            try
            {
                var requireHtmlHelper = service as IRequireHtmlHelper<TModel>;
                if (requireHtmlHelper != null)
                {
                    requireHtmlHelper.HtmlHelper = htmlHelper;
                }

                return useServiceAction(service);
            }
            finally
            {
                IocContainer.Release(service);
            }
        }

        public static void With<TService, TModel>(this HtmlHelper<TModel> htmlHelper, Action<TService> useServiceAction)
        {
            var service = IocContainer.Resolve<TService>();
            try
            {
                var requireHtmlHelper = service as IRequireHtmlHelper<TModel>;
                if (requireHtmlHelper != null)
                {
                    requireHtmlHelper.HtmlHelper = htmlHelper;
                }

                useServiceAction(service);
            }
            finally
            {
                IocContainer.Release(service);
            }
        }

        public static string ComboFor<TModel, TLookup>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TLookup>> propertyExpression)
            where TLookup : class, INamedEntity
        {
            return htmlHelper.With<IComboFor<TLookup, TModel>, TModel>(combo => combo.BoundTo(propertyExpression));
        }

        public static string ComboFor<TModel, TLookup>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TLookup>> propertyExpression, 
            Expression<Func<TLookup, bool>> whereClause)
            where TLookup : class, INamedEntity
        {
            return htmlHelper.With<IComboFor<TLookup, TModel>, TModel>(combo => combo.BoundTo(propertyExpression, whereClause));
        }

        public static string ComboFor<TModel, TLookup>(this HtmlHelper<TModel> htmlHelper, string propertyToBind, IEnumerable<int> selectedIds)
            where TLookup : class, INamedEntity
        {
            return htmlHelper.With<IComboFor<TLookup, TModel>, TModel>(combo => combo.BoundTo(propertyToBind, selectedIds));
        }

        public static string MutipleSelectComboFor<TModel, TLookup>(this HtmlHelper<TModel> htmlHelper, string propertyToBind, IEnumerable<int> selectedIds)
            where TLookup : class, INamedEntity
        {
            return htmlHelper.With<IComboFor<TLookup, TModel>, TModel>(combo => combo.Multiple().BoundTo(propertyToBind, selectedIds));
        }

        public static string MutipleSelectComboFor<TModel, TLookup>(
            this HtmlHelper<TModel> htmlHelper, 
            string propertyToBind, 
            IEnumerable<int> selectedIds,
            Expression<Func<TLookup, bool>> whereClause)
            where TLookup : class, INamedEntity
        {
            return htmlHelper.With<IComboFor<TLookup, TModel>, TModel>(combo => combo.Multiple().BoundTo(propertyToBind, selectedIds, whereClause));
        }

        public static void PostAction<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> action, string buttonText)
        {
            var postAction = new PostAction<TController>(htmlHelper, action, buttonText);
            postAction.Render();
        }
    }

    public interface IRequireHtmlHelper<TModel>
    {
        HtmlHelper<TModel> HtmlHelper { get; set; }
    }
}
