using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.IO;
using Suteki.Common.Extensions;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.HtmlHelpers
{
    public class StockWriter
    {
        readonly HtmlHelper htmlHelper;
        readonly Category root;

        public StockWriter(HtmlHelper htmlHelper, Category root)
        {
            this.htmlHelper = htmlHelper;
            this.root = root;
        }

        public string Write()
        {
            var writer = new HtmlTextWriter(new StringWriter());

            WriteChildCategories(writer, root);

            return writer.InnerWriter.ToString();
        }

        private void WriteChildCategories(HtmlTextWriter writer, Category category)
        {
            foreach (var childCategory in category.Categories)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.H3);
                writer.Write(childCategory.Name);
                writer.RenderEndTag();

                foreach (var product in childCategory.Products)
                {
                    WriteProduct(writer, product);
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "clear");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();

                WriteChildCategories(writer, childCategory);
            }
        }

        private void WriteProduct(HtmlTextWriter writer, Product product)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "stockItem");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "stockProduct");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(product.Name);
            writer.RenderEndTag();

            if (product.HasSize)
            {
                foreach (Size size in product.Sizes.Active())
                {
                    WriteSizeCheckbox(writer, size);
                }
            }
            else
            {
                WriteSizeCheckbox(writer, product.DefaultSize);
            }

            writer.RenderEndTag();
        }

        private void WriteSizeCheckbox(HtmlTextWriter writer, Size size)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "stockCheckbox");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(size.Name);
            writer.Write(htmlHelper.CheckBox("stockitem_{0}".With(size.Id), size.IsInStock));
            writer.RenderEndTag();
        }
    }
}
