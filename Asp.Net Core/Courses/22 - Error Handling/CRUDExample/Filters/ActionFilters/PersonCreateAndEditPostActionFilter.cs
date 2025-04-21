using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger;
        public PersonCreateAndEditPostActionFilter(ICountriesService countriesService, ILogger<PersonCreateAndEditPostActionFilter> logger)
        {
            _countriesService = countriesService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //before
            if (context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    personsController.ViewBag.Countries = (await _countriesService.GetAllCountries())
                        .Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() });
                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    var personAddRequest = context.ActionArguments["personAddRequest"];
                    context.Result = personsController.View(personAddRequest); // Views/Persons/Create.cshtml
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }

            //after
            _logger.LogInformation("In after logic of PersonsCreateAndEdit Action filter");
        }
    }
}
