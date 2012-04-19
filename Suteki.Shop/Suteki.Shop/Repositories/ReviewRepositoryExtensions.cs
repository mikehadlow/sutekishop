using System.Linq;

namespace Suteki.Shop.Repositories
{
	public static class ReviewRepositoryExtensions
	{
		public static IQueryable<Review> ForProduct(this IQueryable<Review> reviews, int productId)
		{
			return reviews.Where(x => x.Product.Id == productId);
		}

        public static IQueryable<T> Approved<T>(this IQueryable<T> reviews) where T : IComment
		{
			return reviews.Where(x => x.Approved);
		}

        public static IQueryable<T> Unapproved<T>(this IQueryable<T> reviews) where T : IComment
		{
			return reviews.Where(x => x.Approved == false);
		}
	}
}