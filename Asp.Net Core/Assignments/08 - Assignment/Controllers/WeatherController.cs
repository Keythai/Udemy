using Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Assignment.Controllers
{
    public class WeatherController : Controller
    {
        public List<CityWeather> cityWeathers = new List<CityWeather>()
        {
            new CityWeather(){
            CityUniqueCode = "LDN",
            CityName = "London",
            DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
            TemperatureFahrenheit = 33 },

            new CityWeather(){
            CityUniqueCode = "NYC",
            CityName = "London",
            DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),
            TemperatureFahrenheit = 60 },

            new CityWeather(){
            CityUniqueCode = "PAR",
            CityName = "Paris",
            DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),
            TemperatureFahrenheit = 82 }

        };
        [Route("/")]
        public IActionResult Index()
        {
            return View(cityWeathers);
        }

        [Route("[controller]/{cityCode}")]
        public IActionResult GetCityWeather(string? cityCode)
        {
            foreach(CityWeather cityWeather in cityWeathers)
            {
                if(cityWeather.CityUniqueCode == cityCode)
                    return View(cityWeather);
            }
            return View(null);
        }
    }
}
