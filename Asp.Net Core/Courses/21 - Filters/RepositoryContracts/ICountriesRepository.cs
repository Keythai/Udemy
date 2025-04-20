using Entities;

namespace RepositoryContracts
{
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a new country to the database.
        /// </summary>
        /// <param name="country">Country object to add</param>
        /// <returns>Returns the country object after adding it to the database</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Retrieves all countries from the database.
        /// </summary>
        /// <returns>All countries from the database</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Retrieves a country by its ID from the database.
        /// </summary>
        /// <param name="countryId">CountryId to search</param>
        /// <returns>Matching country object or null</returns>
        Task<Country?> GetCountryById(Guid countryId);

        /// <summary>
        /// Retrieves a country by its name from the database.
        /// </summary>
        /// <param name="countryName">Country name to search</param>
        /// <returns>Matching country object or null</returns>
        Task<Country?> GetCountryByName(string countryName);
    }
}
