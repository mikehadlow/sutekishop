using System;
using Suteki.Common.ViewData;

namespace Suteki.Shop.ViewData
{
    public class CmsViewData : ViewDataBase
    {
        public Content Content { get; set; }

        public ITextContent TextContent
        {
            get
            {
                var textContent = Content as ITextContent;
                if (textContent == null)
                    throw new ApplicationException("ViewData Content is not of type ITextContent");
                return textContent;
            }
        }

        public Menu Menu
        {
            get
            {
                var menu = Content as Menu;
                if (menu == null)
                    throw new ApplicationException("ViewData Content is not of type Menu");
                return menu;
            }
        }

        // attempt at a fluent interface

        public CmsViewData WithContent(Content content)
        {
            Content = content;
            return this;
        }
    }

    public static class CmsView
    {
        public static CmsViewData Data
        {
            get
            {
                return new CmsViewData();
            }
        }
    }
}
