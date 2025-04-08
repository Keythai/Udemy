using Models;
using ServiceContract;

namespace Service
{
    public class WeatherService : IWeatherService
    {
        private List<CityWeather> cityWeathers;
        public WeatherService()
        {
             cityWeathers = new List<CityWeather>()
            {
                new CityWeather(){
                CityUniqueCode = "LDN",
                CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
                TemperatureFahrenheit = 33 },

                new CityWeather(){
                CityUniqueCode = "NYC",
                CityName = "New York",
                DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),
                TemperatureFahrenheit = 60 },

                new CityWeather(){
                CityUniqueCode = "PAR",
                CityName = "Paris",
                DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),
                TemperatureFahrenheit = 82 }
            };
        }
        public CityWeather? GetWeatherByCityCode(string CityCode)
        {
            foreach(CityWeather cityWeather in cityWeathers)
            {
                if(cityWeather.CityUniqueCode == CityCode)
                    return cityWeather;
            }
            return null;
        }

        public List<CityWeather> GetWeatherDetails()
        {
            return cityWeathers;
        }
    }
}
