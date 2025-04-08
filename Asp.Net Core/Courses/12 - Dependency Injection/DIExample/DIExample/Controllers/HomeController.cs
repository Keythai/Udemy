using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICitiesService _citiesService1;
        private readonly ICitiesService _citiesService2;
        private readonly ICitiesService _citiesService3;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILifetimeScope _lifetimeScope;

        public HomeController(ICitiesService citiesService1,
            ICitiesService citiesService2,
            ICitiesService citiesService3,
            ILifetimeScope serviceScopeFactory)
        {
            _citiesService1 = citiesService1;
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
            _lifetimeScope = serviceScopeFactory;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities = _citiesService1.GetCities();
            ViewBag.InstanceId_CitiesService_1 = _citiesService1.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService_2 = _citiesService2.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService_3 = _citiesService3.ServiceInstanceId;

            // this creates a different lifetime even from the same request
            //using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            //{
            //    ICitiesService citiesService = scope.ServiceProvider.GetRequiredService<ICitiesService>();

            //    ViewBag.InstanceId_CitiesService_InScope = citiesService.ServiceInstanceId;
            //}

            // Autofac
            using (ILifetimeScope scope = _lifetimeScope.BeginLifetimeScope())
            {
                ICitiesService citiesService = scope.Resolve<ICitiesService>();

                ViewBag.InstanceId_CitiesService_InScope = citiesService.ServiceInstanceId;
            }

            return View(cities);
        }

        /* this is method injection, very rare to use this
         [Route("/")]
        public IActionResult Index([FromServices] ICitiesService _citiesService)
        {
            List<string> cities = _citiesService.GetCities();
            return View(cities);
        }
         */
    }
}
