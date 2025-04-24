using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SerilogTimings;
using Exceptions;

namespace Services
{
    public class PersonsGetterService : IPersonsGetterService
    {
        //private readonly ApplicationDbContext _context;

        //public PersonService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public PersonsGetterService(IPersonsRepository personRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public virtual async Task<List<PersonResponse>> GetAllPersons()
        {
            _logger.LogInformation("GetAllPersons method of PersonService");

            return (await _personsRepository.GetAllPersons())
                .Select(p => p.ToPersonResponse()).ToList();
        }

        public virtual async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if (personId == null) return null;
            //Person? person = await _context.Persons.Include("Country")
            //    .FirstOrDefaultAsync(p => p.PersonId == personId);
            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);
            if (person == null) return null;
            return person.ToPersonResponse();
        }

        public virtual async Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString)
        {
            _logger.LogInformation("GetFilteredPersons method of PersonService");

            List<Person> persons;
            using (Operation.Time("Time for GetFilteredPersons from Database"))
            {
                persons = searchBy switch
                {
                    nameof(PersonResponse.PersonName) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.PersonName.Contains(searchString)),

                    nameof(PersonResponse.Email) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Email.Contains(searchString)),

                    nameof(PersonResponse.DateOfBirth) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),

                    nameof(PersonResponse.Gender) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Gender.Contains(searchString)),

                    nameof(PersonResponse.CountryId) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Country.CountryName.Contains(searchString)),

                    nameof(PersonResponse.Address) =>
                    await _personsRepository.GetFilteredPersons(temp =>
                    temp.Address.Contains(searchString)),

                    _ => await _personsRepository.GetAllPersons()
                };
            } // end of serilog using for timing

            _diagnosticContext.Set("Persons", persons);

            return persons.Select(p => p.ToPersonResponse()).ToList();
        }

        public virtual async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

            // leaveOpen : true means that the new text will be appended to the top of the file
            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);
            // write header row into csv
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
            csvWriter.NextRecord();
            // get all persons
            List<PersonResponse> persons = await GetAllPersons();

            // write data row into csv
            foreach (PersonResponse person in persons)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                if (person.DateOfBirth != null)
                {
                    csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                }
                else
                {
                    csvWriter.WriteField(string.Empty);
                }
                csvWriter.WriteField(person.Age);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.ReceiveNewsLetters);
                csvWriter.NextRecord();
                csvWriter.Flush();
            }


            memoryStream.Position = 0; // reset the position of the stream to the beginning
            return memoryStream;
        }

        public virtual async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            ExcelPackage.License.SetNonCommercialPersonal("User");
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonSheet");
                workSheet.Cells["A1"].Value = "Person Name";
                workSheet.Cells["B1"].Value = "Email";
                workSheet.Cells["C1"].Value = "DateOfBirth";
                workSheet.Cells["D1"].Value = "Age";
                workSheet.Cells["F1"].Value = "Gender";
                workSheet.Cells["F1"].Value = "Country";
                workSheet.Cells["G1"].Value = "Address";
                workSheet.Cells["H1"].Value = "ReceiveNewsLetters";

                using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                List<PersonResponse> persons = await GetAllPersons();
                foreach (PersonResponse person in persons)
                {
                    workSheet.Cells[row, 1].Value = person.PersonName;
                    workSheet.Cells[row, 2].Value = person.Email;
                    workSheet.Cells[row, 3].Value = person.DateOfBirth;
                    workSheet.Cells[row, 4].Value = person.Age;
                    workSheet.Cells[row, 5].Value = person.Gender;
                    workSheet.Cells[row, 6].Value = person.Country;
                    workSheet.Cells[row, 7].Value = person.Address;
                    workSheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

                    row++;
                }

                // A1:H10 = A1 to H10 as range for example
                workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0; // reset the position of the stream to the beginning
            return memoryStream;
        }
    }
}
