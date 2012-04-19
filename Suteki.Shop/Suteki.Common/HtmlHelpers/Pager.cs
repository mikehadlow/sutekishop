using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Suteki.Common.Extensions;
using System.Web.Routing;

namespace Suteki.Common.HtmlHelpers
{
    public class Pager
    {
        readonly StringBuilder htmlText = new StringBuilder();

        readonly HtmlHelper htmlHelper;
        readonly string controller;
        readonly string action;
        readonly IPagedList pagedList;

        public Pager(
            HtmlHelper htmlHelper,
            string controller,
            string action,
            IPagedList pagedList)
        {
            if (htmlHelper == null) throw new ArgumentNullException("htmlHelper");
            if (controller == null) throw new ArgumentNullException("controller");
            if (action == null) throw new ArgumentNullException("action");
            if (pagedList == null) throw new ArgumentNullException("pagedList");

            this.htmlHelper = htmlHelper;
            this.controller = controller;
            this.action = action;
            this.pagedList = pagedList;
        }

        public string WriteHtml()
        {
            const int pageRange = 10;
            htmlText.Append("<div class=\"pager\">");

            WriteLink(0, "<<");
            WriteLink(pagedList.PageIndex - 1, "<");

            for (int i = 0; i < pagedList.NumberOfPages; i++)
            {
                if ((i + pageRange) > pagedList.PageIndex && ((i - pageRange) < pagedList.PageIndex))
                    WriteLink(i);
            }

            WriteLink(pagedList.PageIndex + 1, ">");
            WriteLink(pagedList.NumberOfPages - 1, ">>");

            htmlText.Append("</div>");

            return htmlText.ToString();
        }

        private void WriteLink(int pageNumber)
        {
            WriteLink(pageNumber, (pageNumber + 1).ToString());
        }

        private void WriteLink(int pageNumber, string text)
        {
            if (pageNumber == pagedList.PageIndex || pageNumber < 0 || pageNumber > pagedList.NumberOfPages-1)
            {
                htmlText.AppendFormat("{0} ", text);
            }
            else
            {
                htmlText.AppendFormat("{0} ", htmlHelper.ActionLink(text, action, controller, 
                    GetCriteria(pageNumber), new RouteValueDictionary()));
            }
        }

        private RouteValueDictionary GetCriteria(int pageNumber)
        {
            var values = htmlHelper.ViewContext.HttpContext.Request.GetRequestValues("action", "controller", "CurrentPage");
            values.Add("CurrentPage", pageNumber);
            return values;
        }
    }
}
