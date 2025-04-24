using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Example of Open/Closed principle - Interface approach
namespace Services
{
    public class PersonsGetterServiceWithFewExcelFields : IPersonsGetterService
    {
        private readonly PersonsGetterService _personsGetterService;
        public PersonsGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService)
        {
            _personsGetterService = personsGetterService;
        }
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            return await _personsGetterService.GetAllPersons();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString)
        {
            return await _personsGetterService.GetFilteredPersons(searchBy, searchString);
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            return await _personsGetterService.GetPersonByPersonId((Guid)personId);
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            return await _personsGetterService.GetPersonsCSV();
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            ExcelPackage.License.SetNonCommercialPersonal("User");
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonSheet");
                workSheet.Cells["A1"].Value = "Person Name";
                workSheet.Cells["B1"].Value = "Age";
                workSheet.Cells["C1"].Value = "Gender";

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
                    workSheet.Cells[row, 2].Value = person.Age;
                    workSheet.Cells[row, 3].Value = person.Gender;

                    row++;
                }

                // A1:H10 = A1 to H10 as range for example
                workSheet.Cells[$"A1:C{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0; // reset the position of the stream to the beginning
            return memoryStream;
        }
    }
}
