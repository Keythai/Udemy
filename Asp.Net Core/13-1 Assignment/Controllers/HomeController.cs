using _13_1_Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace _13_1_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly SocialMediaLinksOptions _options;
        public HomeController(IOptions<SocialMediaLinksOptions> option)
        {
            _options = option.Value;
        }
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.Options = _options;
            return View();
        }
    }
}
