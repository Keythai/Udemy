using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly WeatherApiOptions _options;
        public HomeController(IConfiguration configuration,
            IOptions<WeatherApiOptions> weatherApiOptions)
        {
            _configuration = configuration;
            _options = weatherApiOptions.Value;
        }
        [Route("/")]
        public IActionResult Index()
        {
            // for calling json value from appsettings.json
            ViewBag.MyKey = _configuration["MyKey"];
            ViewBag.MyAPIKey = _configuration.GetValue("MyAPIKey", "the default key");

            // for calling json object from appsettings.json - first way
            ViewBag.ClientID = _configuration.GetSection("weatherapi")["ClientID"];

            // second way
            //IConfigurationSection weatherapiSection = _configuration.GetSection("weatherapi");
            //ViewBag.ClientSecret = weatherapiSection("weatherapi")["ClientSecret"];

            // third way, loads configuration values into new Option object
            //WeatherApiOptions? options = _configuration.GetSection("weatherapi").Get<WeatherApiOptions>();
            //ViewBag.ClientSecret = options?.ClientSecret;

            // Bind: loads configuration values into existing Option object
            //WeatherApiOptions options = new WeatherApiOptions();
            //_configuration.GetSection("weatherapi").Bind(options);
            //ViewBag.ClientSecret = options?.ClientSecret;

            ViewBag.ClientSecret = _options.ClientSecret;
            return View();
        }
    }
}
