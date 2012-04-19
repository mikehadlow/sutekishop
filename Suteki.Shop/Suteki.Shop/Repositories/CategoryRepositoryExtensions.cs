using System.Linq;
using Suteki.Common.Repositories;

namespace Suteki.Shop.Repositories
{
    public static class CategoryRepositoryExtensions
    {
        public static Category GetRootCategory(this IRepository<Category> categoryRepository)
        {
            return categoryRepository.GetById(1);
        }

        public static IQueryable<Category> Alphabetical(this IQueryable<Category> categories)
        {
            return categories.OrderBy(c => c.Name);
        }
    }
}
