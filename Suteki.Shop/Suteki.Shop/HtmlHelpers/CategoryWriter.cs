using System.Web.UI;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using Suteki.Common.Extensions;
using Suteki.Common.HtmlHelpers;
using Suteki.Common.Repositories;
using Suteki.Shop.Controllers;
using Suteki.Shop.Repositories;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.HtmlHelpers
{
    public enum CategoryDisplay
    {
        Edit,
        View
    }

    public class CategoryWriter
    {
        readonly CategoryViewData rootCategory;
        readonly HtmlHelper htmlHelper;
        readonly CategoryDisplay display;

        public CategoryWriter(CategoryViewData rootCategory, HtmlHelper htmlHelper, CategoryDisplay display)
        {
            this.rootCategory = rootCategory;
            this.htmlHelper = htmlHelper;
            this.display = display;
        }

        public string Write()
        {
            var writer = new HtmlTextWriter(new StringWriter());

            if (display == CategoryDisplay.View)
            {
                writer.AddAttribute("class", "category");
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            WriteCategories(writer, rootCategory.ChildCategories.InOrder());

            writer.RenderEndTag();

            return writer.InnerWriter.ToString();
        }

        private void WriteCategories(HtmlTextWriter writer, IEnumerable<CategoryViewData> categories)
        {
            
            
            bool first = true;
            foreach (var category in categories.ActiveFor(htmlHelper.CurrentUser()))
            {
                if (first)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                    first = false;
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.Write(WriteCategory(category));
                WriteCategories(writer, category.ChildCategories.InOrder());
                writer.RenderEndTag();
            }

            if (!first) writer.RenderEndTag();
        }

        private MvcHtmlString WriteCategory(CategoryViewData category)
        {
            if (display == CategoryDisplay.Edit)
            {
                return MvcHtmlString.Create("{0} {1} {2} {3} {4}".With(
                    WriteCategoryLink(category),
                    htmlHelper.ActionLink<CategoryController>(c => c.Edit(category.CategoryId), "Edit"),
                    htmlHelper.Tick(category.IsActive),
                    htmlHelper.UpArrowLink<CategoryController>(c => c.MoveUp(category.CategoryId)),
                    htmlHelper.DownArrowLink<CategoryController>(c => c.MoveDown(category.CategoryId))
                    ));
            }

            return WriteCategoryLink(category);
        }

        private MvcHtmlString WriteCategoryLink(CategoryViewData category)
        {
            return htmlHelper.ActionLink<ProductController>(c => c.Index(category.CategoryId), category.Name);
        }
    }
}
