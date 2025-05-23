﻿using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Country object to be added</param>
        /// <returns>Returns the country object after adding it (including newly generated country id)</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
        /// <summary>
        /// Returns all countries from the list
        /// </summary>
        /// <returns>All countries from the list as List of CountryResponse</returns>
        Task<List<CountryResponse>> GetAllCountries();
        /// <summary>
        /// Returns a country object based on the given country id
        /// </summary>
        /// <param name="countryId">CountryId (guid) to search</param>
        /// <returns>Matching country as CountryResponse object</returns>
        Task<CountryResponse?> GetCountryByCountryId(Guid? countryId);
        /// <summary>
        /// Uploads a list of countries from an Excel file to the database
        /// </summary>
        /// <param name="formFile">Excel file with a list of countries</param>
        /// <returns>Returns the number of countries added</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
