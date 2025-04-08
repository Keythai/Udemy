using Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ServiceContract;

namespace Assignment.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        [Route("/")]
        public IActionResult Index()
        {
            return View(_weatherService.GetWeatherDetails());
        }

        [Route("[controller]/{cityCode}")]
        public IActionResult GetCityWeather(string? cityCode)
        {
            return View(_weatherService.GetWeatherByCityCode(cityCode));
        }
    }
}
