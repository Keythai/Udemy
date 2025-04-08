using Microsoft.AspNetCore.Mvc;
using ModelValidationsExample.CustomModelBinders;
using ModelValidationsExample.Models;

namespace ModelValidationsExample.Controllers
{
    public class HomeController : Controller
    {
        // Bind tells system only receive the specified data from object
        /*  [Bind(nameof(Person.PersonName), 
              nameof(Person.Email),
              nameof(Person.Password),
              nameof(Person.ConfirmPassword))]
        */
        // [FromBody] [ModelBinder(BinderType=typeof(PersonModelBinder))]
        [Route("register")]
        public IActionResult Index(Person person, // must be frombody to receive data otherwise it will receive from urlencode or formdata by default
            [FromHeader(Name = "User-Agent")]string UserAgent)  // example of FromHeader
        {
            if(!ModelState.IsValid)
            {
                string errors = string.Join("\n", 
                    ModelState.Values.
                    SelectMany(value => value.Errors).
                    Select(error => error.ErrorMessage));
                //foreach (var value in ModelState.Values)
                //{
                //    foreach (var error in value.Errors)
                //    {
                //        if (error.ErrorMessage != "")
                //        {
                //            errorList.Add(error.ErrorMessage);
                //        }
                //    }
                //}
                
                return BadRequest(errors);
            }
            return Content($"{ person}");
        }
    }
}
