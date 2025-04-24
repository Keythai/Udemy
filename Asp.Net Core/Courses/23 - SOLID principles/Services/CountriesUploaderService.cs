using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class CountriesUploaderService : ICountriesUploaderService
    {
        //private readonly ApplicationDbContext _context;

        //public CountriesService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly ICountriesRepository _countriesRepository;
        public CountriesUploaderService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
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
