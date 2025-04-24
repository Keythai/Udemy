using ServiceContracts.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating Country entity
    /// </summary>
    public interface ICountriesUploaderService
    {
        /// <summary>
        /// Uploads a list of countries from an Excel file to the database
        /// </summary>
        /// <param name="formFile">Excel file with a list of countries</param>
        /// <returns>Returns the number of countries added</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
