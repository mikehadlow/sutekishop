using Suteki.Common.Repositories;

namespace Suteki.Shop.Repositories
{
    public static class PostZoneRepositoryExtensions
    {
        public static PostZone GetDefaultPostZone(this IRepository<PostZone> postZoneRepository)
        {
            // HACK: user should be able to define defulat post zone
            return postZoneRepository.GetById(1); // 1 == UK post zone, set by static data inserts
        }
    }
}
