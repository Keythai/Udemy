using Models;
using System.Collections.Generic;

namespace ServiceContract
{
    public interface IWeatherService
    {
        //  Returns a list of CityWeather objects that contains weather details of cities
        List<CityWeather> GetWeatherDetails();
        //  Returns an object of CityWeather based on the given city code
        CityWeather? GetWeatherByCityCode(string CityCode);
    }
}
