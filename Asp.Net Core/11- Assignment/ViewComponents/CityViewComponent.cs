using Assignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.ViewComponents
{
    public class CityViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CityWeather grid)
        {
            return View("Default", grid);
        }
    }
}
