using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _context;
        public CountriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Repository is occurred after Business Logics (including validation), so we don't need to validate the input data here
        public async Task<Country> AddCountry(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryById(Guid countryId)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == countryId);
        }

        public async Task<Country?> GetCountryByName(string countryName)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.CountryName == countryName);
        }
    }
}
