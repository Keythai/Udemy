using Microsoft.AspNetCore.Mvc;
using ControllersExample.Models;

namespace ControllersExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("sayhello")]
        public string SayHello()
        {
            return "Hello from the Home Controller!";
        }

        // Route with a parameter and constraint
        [Route("contact-us/{mobile:regex(^\\d{{10}}$)}")]
        public string Contact(int mobile)
        {
            return $"Contact me at: {mobile}";
        }

        // ContentResult with text/plain and text/html
        [Route("/")]
        public ContentResult Index()
        {
            //return new ContentResult
            //{
            //    Content = "Hello from the Home Controller!",
            //    ContentType = "text/plain"
            //};

            // Same with above (required inheritance from Controller)
            //return Content("Hello from the Home Controller!", "text/plain");

            return Content("<h1>Welcome</h1> <h2>Hello from the Home Controller!</h2>", "text/html"); 
        }

        [Route("person")]
        public JsonResult Person()
        {
            Person person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Smith",
                Age = 25
            };
            //return new JsonResult(person);
            return Json(person);
        }

        // VirtualFileResult = files within wwwroot folder
        [Route("file-download")]
        public VirtualFileResult FileDownload()
        {
            //return new VirtualFileResult("/Resume 2025 .pdf", "application/pdf");
            return File("/Resume 2025 .pdf", "application/pdf");
        }

        // PhysicalFileResult = file with full path
        [Route("file-download2")]
        public PhysicalFileResult FileDownload2()
        {
            //return new PhysicalFileResult(@"C:\Users\tcyya\OneDrive\デスクトップ\Resume 2025 .pdf", "application/pdf");
            return PhysicalFile(@"C:\Users\tcyya\OneDrive\デスクトップ\Resume 2025 .pdf", "application/pdf");
        }

        // FileContentResult = file as byte array
        [Route("file-download3")]
        public FileContentResult FileDownload3()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\tcyya\OneDrive\デスクトップ\Resume 2025 .pdf");
            //return new FileContentResult(bytes, "application/pdf");
            return File(bytes, "application/pdf");
        }
    }
}
