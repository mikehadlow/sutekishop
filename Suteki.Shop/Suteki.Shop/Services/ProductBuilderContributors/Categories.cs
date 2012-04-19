using System;
using System.Linq;
using Suteki.Common.Repositories;

namespace Suteki.Shop.Services.ProductBuilderContributors
{
    public class Categories : IProductBuilderContributor
    {
        readonly IRepository<Category> categoryRepository;

        public Categories(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public void ContributeTo(ProductBuildingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.ProductViewData.CategoryIds.Count == 0)
            {
                context.ModelStateDictionary.AddModelError("CategoryIds", "You must select at least one category");
                return;
            }

            var newIds = context.ProductViewData.CategoryIds;
            var existingIds = context.Product.ProductCategories.Select(x => x.Category.Id);

            var idsToAdd = newIds.Where(newId => !existingIds.Any(existingId => existingId == newId));
            var idsToDelete = existingIds.Where(existingId => !newIds.Any(newId => newId == existingId));

            var categoriesToDelete = (from productCategory in context.Product.ProductCategories
                                     let category = productCategory.Category
                                     from id in idsToDelete
                                     where id == category.Id
                                     select category).ToList();

            foreach (var category in categoriesToDelete)
            {
                context.Product.RemoveCategory(category);
            }

            foreach (var newId in idsToAdd)
            {
                var category = categoryRepository.GetById(newId);
                context.Product.AddCategory(category);
            }
        }

        public int Order
        {
            get { return 2; }
        }
    }
}