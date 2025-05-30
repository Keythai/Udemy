﻿using Microsoft.AspNetCore.Mvc;

namespace EnvironmentsExample.Controllers
{
    public class HomeController : Controller
    {
        // to get environment in controller
        private readonly IWebHostEnvironment _webHostEnvironment;
        // constructor
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.CurrentEnvironment = _webHostEnvironment.EnvironmentName;
            return View();
        }
    }
}
