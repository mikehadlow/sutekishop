using Suteki.Common.Repositories;

namespace Suteki.Shop.Repositories
{
    public static class CountryRepositoryExtensions
    {
        public static Country GetDefaultCountry(this IRepository<Country> countryRepository)
        {
            return countryRepository.GetById(1); // HACK, just get the UK which is defined in static data
        }
    }
}
