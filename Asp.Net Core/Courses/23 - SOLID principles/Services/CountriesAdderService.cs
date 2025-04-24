using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class CountriesAdderService : ICountriesAdderService
    {
        //private readonly ApplicationDbContext _context;

        //public CountriesService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly ICountriesRepository _countriesRepository;
        public CountriesAdderService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if(string.IsNullOrEmpty(countryAddRequest.CountryName))
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            //if (await _context.Countries.CountAsync(temp =>
            //temp.CountryName == countryAddRequest.CountryName) > 0)
            //    throw new ArgumentException("Given country name already exists");
            if (await _countriesRepository.GetCountryByName(countryAddRequest.CountryName) != null)
                throw new ArgumentException("Given country name already exists");

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate new Guid
            country.CountryId = Guid.NewGuid();

            //Add country object into countries
            //_context.Countries.Add(country);
            //await _context.SaveChangesAsync();
            await _countriesRepository.AddCountry(country);

            return country.ToCountryResponse();
        }
        
    }
}
