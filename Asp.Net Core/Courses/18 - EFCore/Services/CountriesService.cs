using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonsDbContext _context;

        public CountriesService(PersonsDbContext context)
        {
            _context = context;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if(string.IsNullOrEmpty(countryAddRequest.CountryName))
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            if (await _context.Countries.CountAsync(temp =>
            temp.CountryName == countryAddRequest.CountryName) > 0)
                throw new ArgumentException("Given country name already exists");

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate new Guid
            country.CountryId = Guid.NewGuid();

            //Add country object into countries
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _context.Countries
                .Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;

            Country? countryResponse = await _context.Countries.FirstOrDefaultAsync(country => country.CountryId == countryId);
            if(countryResponse == null)
                return null;
            return countryResponse.ToCountryResponse();
        }
    }
}
