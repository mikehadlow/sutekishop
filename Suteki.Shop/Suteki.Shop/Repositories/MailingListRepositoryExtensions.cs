using System.Collections.Generic;
using System.Linq;

namespace Suteki.Shop.Repositories
{
	public static class MailingListRepositoryExtensions
	{
        public static IEnumerable<MailingListSubscription> EnsureNoDuplicates(this IEnumerable<MailingListSubscription> subscriptions)
		{
			var grouped = from s in subscriptions
			              group s by s.Email into groupedByEmail
			              select groupedByEmail;

			var query = from g in grouped
						select g.OrderByDescending(x => x.DateSubscribed).First();


			return query;
		}
	}
}