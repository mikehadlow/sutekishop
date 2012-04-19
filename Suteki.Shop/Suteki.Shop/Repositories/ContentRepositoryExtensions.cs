using System.Linq;
using Suteki.Common.Repositories;

namespace Suteki.Shop.Repositories
{
    public static class ContentRepositoryExtensions
    {
        public static IQueryable<Content> WithParent(this IQueryable<Content> contents, Content parent)
        {
            return contents.WithParent(parent.Id);
        }

        public static IQueryable<Content> WithParent(this IQueryable<Content> contents, int parentContentId)
        {
            return contents.Where(c => c.ParentContent.Id == parentContentId);
        }

        public static IQueryable<Content> WithAnyParent(this IQueryable<Content> contents)
        {
            return contents.Where(c => c.ParentContent != null);
        }

        public static Content DefaultText(this IQueryable<Content> contents, Menu menu)
        {
            return contents.TextContent().InOrder().FirstOrDefault() ?? new TextContent
            {
                Name = "Default",
                Text = "No content has been created yet",
                IsActive = true,
                ParentContent = menu
            };
        }

        public static IQueryable<Content> TextContent(this IQueryable<Content> contents)
        {
            return contents.Where(c => c is TextContent || c is TopContent);
        }

        public static IQueryable<Menu> Menus(this IQueryable<Content> contents)
        {
            return contents.OfType<Menu>();
        }
    }
}
