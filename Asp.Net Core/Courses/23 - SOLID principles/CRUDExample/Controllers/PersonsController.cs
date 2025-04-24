using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilters;
using CRUDExample.Filters.ExceptionFilters;
using CRUDExample.Filters.ResourceFilters;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System.Collections.Generic;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-_key-From-Controller", "My-_value-From-Controller", 3 }, Order = 3)] //Has to assign both IOrderFilter and TypeFilter orders, a bug in ASP.NET Core
    [ResponseHeaderFilterFactory("My-_key-From-Controller", "My-_value-From-Controller", 3)]
    //[TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonsAlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        // private fields
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly ICountriesAdderService _countriesAdderService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ICountriesUploaderService _countriesUploaderService;
        private readonly ILogger<PersonsController> _logger;
        public PersonsController(IPersonsGetterService personsGetterService, 
            ICountriesAdderService countriesAdderService, 
            ICountriesGetterService countriesGetterService,
            ICountriesUploaderService countriesUploaderService,
            ILogger<PersonsController> logger,
            IPersonsUpdaterService personsUpdaterService,
            IPersonsSorterService personsSorterService,
            IPersonsDeleterService personsDeleterService,
            IPersonsAdderService personsAdderService)
        {
            _personsGetterService = personsGetterService;
            _personsAdderService = personsAdderService;
            _personsDeleterService = personsDeleterService;
            _personsSorterService = personsSorterService;
            _personsUpdaterService = personsUpdaterService;

            _countriesAdderService = countriesAdderService;
            _countriesGetterService = countriesGetterService;
            _countriesUploaderService = countriesUploaderService;
            _logger = logger;
        }
        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter), Order = 4)] //attach action filter
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "My-_key-From-Action", "My-_value-From-Action", 1 }, Order = 1)] //argument example //Order value defines when to execute (overwrites default order)
        [ResponseHeaderFilterFactory("My-key-From-Action", "My-value-From-Action", 1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [SkipFilter]
        // parameter names must be same with elements' names used in views
        public async Task<IActionResult> Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");
            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}");
            //Search
            
            List<PersonResponse> persons = await _personsGetterService.GetFilteredPersons(searchBy, searchString);

            //Sort
            List<PersonResponse> sortedPerson = await _personsSorterService.GetSortedPersons(persons, sortBy, sortOrder);

            return View(sortedPerson); // Views/Persons/Index.cshtml
        }

        // Executes when user clicks on Create button
        [Route("[action]")]
        [HttpGet]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "my-key", "my-value", 4 })]
        [ResponseHeaderFilterFactory("my-key", "my-value", 4)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Countries = (await _countriesGetterService.GetAllCountries()).Select(c => new SelectListItem()
            {
                Text = c.CountryName,
                Value = c.CountryId.ToString()
            });
            return View(); // Views/Persons/Create.cshtml
        }

        // Executes when user clicks on Save button
        [HttpPost]
        [Route("[action]")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] { false })]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            await _personsAdderService.AddPerson(personRequest);
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid personId)
        {
            PersonResponse? person = await _personsGetterService.GetPersonByPersonId(personId);

            if (person == null)
            {
                return RedirectToAction("Index", "Persons");
            }
            ViewBag.Countries = (await _countriesGetterService.GetAllCountries()).Select(c => new SelectListItem()
            {
                Text = c.CountryName,
                Value = c.CountryId.ToString()
            });
            return View(person.ToPersonUpdateRequest()); // Views/Persons/Edit.cshtml
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            if (_personsGetterService.GetPersonByPersonId(personRequest.PersonId) == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            await _personsUpdaterService.UpdatePerson(personRequest);
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(Guid personId)
        {
            return View(await _personsGetterService.GetPersonByPersonId(personId)); // Views/Persons/Delete.cshtml
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(PersonResponse personResponse)
        {
            if (await _personsGetterService.GetPersonByPersonId(personResponse.PersonId) == null)
            {
                return RedirectToAction("Index", "Persons");
            }
            await _personsDeleterService.DeletePerson(personResponse.PersonId);
            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> persons = await _personsGetterService.GetAllPersons();

            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins(20, 20, 20, 20),
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.ms-excel", "persons.xlsx");
        }
    }
}
