using System;
using System.Linq;
using Suteki.Common.Models;
using Suteki.Common.Repositories;

namespace Suteki.Shop.Services
{
    public class PostageService : IPostageService
    {
        private readonly IRepository<Postage> postageRepository;

        public PostageService(IRepository<Postage> postageRepository)
        {
            this.postageRepository = postageRepository;
        }

        public PostageResult CalculatePostageFor(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException("basket");
            }

            var postages = postageRepository.GetAll();

            var postZone = basket.Country.PostZone;

            var totalWeight = (int)basket.BasketItems
                .Sum(bi => bi.TotalWeight);

            var postageToApply = postages
                .Where(p => totalWeight <= p.MaxWeight && p.IsActive)
                .OrderBy(p => p.MaxWeight)
                .FirstOrDefault();

            var postageDescription = string.Format("for {0}", basket.Country.Name);

            if (postageToApply == null) return PostageResult.WithDefault(postZone, postageDescription);

            var multiplier = postZone.Multiplier;
            var total = new Money(Math.Round(postageToApply.Price.Amount * multiplier, 2, MidpointRounding.AwayFromZero));

            return PostageResult.WithPrice(total, postageDescription);
        }
    }
}
