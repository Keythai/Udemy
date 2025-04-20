using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private readonly ApplicationDbContext _context;

        //public CountriesService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly ICountriesRepository _countriesRepository;
        public CountriesService(ICountriesRepository countriesRepository)
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

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = workSheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;

                        //if (_context.Countries.Where(c => c.CountryName == countryName).Count() == 0)
                        //{
                        //    Country country = new Country()
                        //    {
                        //        CountryName = countryName
                        //    };
                        //    _context.Countries.Add(country);
                        //    await _context.SaveChangesAsync();
                        //    countriesInserted++;
                        //}
                        if (_countriesRepository.GetCountryByName(countryName) == null)
                        {
                            Country country = new Country()
                            {
                                CountryName = countryName
                            };
                            await _countriesRepository.AddCountry(country);
                            countriesInserted++;
                        }
                    }
                }
            }
            return countriesInserted;
        }
    }
}
