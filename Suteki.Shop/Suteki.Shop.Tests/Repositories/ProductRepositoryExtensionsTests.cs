using System.Linq;
using System.Collections.Generic;

namespace Suteki.Shop.Tests.Repositories
{
    public class ProductRepositoryExtensionsTests
    {
       public static void AssertProductsReturnedBy_WhereCategoryIdIs4_AreCorrect(IEnumerable<Product> products)
        {
            const int categoryId = 4;
            products.Count().ShouldEqual(2, "Unexpected number of products returned");

            products.SelectMany(p => p.ProductCategories).Any(x => x.Category.Id == categoryId)
                .ShouldBeTrue("Incorrect categoryId returned");
        }
    }
}
