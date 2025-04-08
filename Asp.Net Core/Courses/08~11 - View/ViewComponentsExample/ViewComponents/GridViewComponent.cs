using Microsoft.AspNetCore.Mvc;
using ViewComponentsExample.Models;

namespace ViewComponentsExample.ViewComponents
{
    // ViewComponent is for complex, reusable UI components that require logic and data retrieval, used only when passing model data is needed
    // Partial View  is for simple, reusable markup, used only when no passing model is needed
    public class GridViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PersonGridModel grid)
        {
            //PersonGridModel personGridModel = new PersonGridModel()
            //{
            //    GridTitle = "Person List",
            //    Persons = new List<Person>
            //    {
            //        new Person(){PersonName="John", JobTitle="Manager"},
            //        new Person(){PersonName="Jones", JobTitle="Asst. Manager"},
            //        new Person(){PersonName="Wiliam", JobTitle="Clerk"}
            //    }
            //};
            return View("Sample", grid); // in default, it invokes a partial view at Views/Shared/Components/Grid/Default.cshtml
        }
    }
}
