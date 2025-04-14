using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

                        if (_context.Countries.Where(c => c.CountryName == countryName).Count() == 0)
                        {
                            Country country = new Country()
                            {
                                CountryName = countryName
                            };
                            _context.Countries.Add(country);
                            await _context.SaveChangesAsync();
                            countriesInserted++;
                        }
                    }
                }
            }
            return countriesInserted;
        }
    }
}
