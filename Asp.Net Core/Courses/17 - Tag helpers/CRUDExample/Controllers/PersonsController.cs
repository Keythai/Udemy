using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Collections.Generic;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        // private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }
        [Route("[action]")]
        [Route("/")]
        // parameter names must be same with elements' names used in views
        public IActionResult Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponse.PersonName), 
            SortOrderOptions sortOrder = SortOrderOptions.ASC) 
        {
            //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Address), "Address" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryId), "Country" }
            };
            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List < PersonResponse > sortedPerson = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPerson); // Views/Persons/Index.cshtml
        }

        // Executes when user clicks on Create button
        [Route("[action]")]
        public IActionResult Create()
        {
            ViewBag.Countries = _countriesService.GetAllCountries().Select(c => new SelectListItem()
            {
                Text = c.CountryName,
                Value = c.CountryId.ToString()
            });
            return View(); // Views/Persons/Create.cshtml
        }

        // Executes when user clicks on Save button
        [HttpPost]
        [Route("[action]")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            ViewBag.Countries = _countriesService.GetAllCountries();
            if (ModelState.IsValid)
            {
                _personsService.AddPerson(personAddRequest);
                return RedirectToAction("Index", "Persons");
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(); // Views/Persons/Create.cshtml
            }
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Edit(Guid personId)
        {
            PersonResponse? person = _personsService.GetPersonByPersonId(personId);
            
            if (person == null)
            {
                return RedirectToAction("Index", "Persons");
            }
            ViewBag.Countries = _countriesService.GetAllCountries().Select(c => new SelectListItem()
            {
                Text = c.CountryName,
                Value = c.CountryId.ToString()
            });
            return View(person.ToPersonUpdateRequest()); // Views/Persons/Edit.cshtml
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            if (_personsService.GetPersonByPersonId(personUpdateRequest.PersonId) == null)
            {
                return RedirectToAction("Index", "Persons");
            }

            if (ModelState.IsValid)
            {
                _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index", "Persons");
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.Countries = _countriesService.GetAllCountries().Select(c => new SelectListItem()
                {
                    Text = c.CountryName,
                    Value = c.CountryId.ToString()
                });
                return View(personUpdateRequest); // Views/Persons/Edit.cshtml
            }
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Delete(Guid personId)
        {
            return View(_personsService.GetPersonByPersonId(personId)); // Views/Persons/Delete.cshtml
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Delete(PersonResponse personResponse)
        {
            if(_personsService.GetPersonByPersonId(personResponse.PersonId) == null)
            {
                return RedirectToAction("Index", "Persons");
            }
            _personsService.DeletePerson(personResponse.PersonId);
            return RedirectToAction("Index", "Persons");
        }
    }
}
