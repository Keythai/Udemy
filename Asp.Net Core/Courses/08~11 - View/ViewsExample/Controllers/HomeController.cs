using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("[controller]")]
        [Route("/")]
        public IActionResult Index()
        {
            //ViewData["appTitle"] = "Asp.Net Core App";
            // if ViewData is storing a list object, then type casting is needed (List<objectType>), since index is not allowed for ViewData

            
            List<Person> people = new List<Person>(){
                new Person() { Name = "John", DateOfBirth = Convert.ToDateTime("2000-05-06"), PersonGender = Gender.Male },
                new Person() { Name = "Linda", DateOfBirth = Convert.ToDateTime("2005-01-09"), PersonGender = Gender.Female },
                new Person() { Name = "Susan", DateOfBirth = Convert.ToDateTime("2008-07-12"), PersonGender = Gender.Other }
            };
            // ViewBag no casting is required
            //ViewBag.people = people;
            return View("Index", people); // Views/Home/Index.cshtml as default 
            //return View("ViewName"); // ViewName.cshtml
        }
        [Route("person-details/{name}")]
        public IActionResult Details(string? name)
        {
            if(name == null){
                return Content("Person name cannot be null");
            }
            List<Person> people = new List<Person>(){
                new Person() { Name = "John", DateOfBirth = Convert.ToDateTime("2000-05-06"), PersonGender = Gender.Male },
                new Person() { Name = "Linda", DateOfBirth = Convert.ToDateTime("2005-01-09"), PersonGender = Gender.Female },
                new Person() { Name = "Susan", DateOfBirth = Convert.ToDateTime("2008-07-12"), PersonGender = Gender.Other }
            };
            Person? matchingPerson = people.Where(temp => temp.Name == name).FirstOrDefault();
            return View(matchingPerson); // Views/Home/Details.cshtml
        }


        [Route("person-with-product")]
        public IActionResult PersonWithProduct()
        {
            Person person = new Person() { Name = "Sara", PersonGender = Gender.Female, DateOfBirth = Convert.ToDateTime("2004-07-01") };
            Product product = new Product() { ProductId = 1, ProductName = "Air conditioner" };
            PersonAndProductWrapperModel personAndProductWrapperModel = new PersonAndProductWrapperModel()
            {
                PersonData = person,
                ProductData = product
            };
            return View("PersonAndProduct", personAndProductWrapperModel);
        }

        [Route("home/all-products")]
        [Route("products/all")]
        public IActionResult All()
        {
            return View();
            // Views/Home/All.cshtml
            // Views/Shared/All.cshtml
        }
    }
}
