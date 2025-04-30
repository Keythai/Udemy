using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class CountriesGetterService : ICountriesGetterService
    {
        //private readonly ApplicationDbContext _context;

        //public CountriesService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly ICountriesRepository _countriesRepository;
        public CountriesGetterService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return (await _countriesRepository.GetAllCountries())
                .Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;

            Country? countryResponse = await _countriesRepository.GetCountryById(countryId.Value);
            if(countryResponse == null)
                return null;
            return countryResponse.ToCountryResponse();
        }

    }
}
