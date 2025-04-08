using Microsoft.AspNetCore.Mvc;
using IActionResultExample.Models;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        // Redirection example 
        // Use cases:
        // form submission
        // authentication and authorization
        // url rewriting (replacing old url)
        // error page redirect
        // [Route("bookstore/bookId?/loggedIn?")] << this one had higher priority
        [Route("bookstore/{bookId?}")]
        public IActionResult Redirection(
            [FromRoute]int bookId, 
            [FromQuery]bool loggedIn, // with [FromRoute] or [FromQuery], system will pick only from route or query string
            Book book)  // class object also read the property name from query string, but only work on Index()
        {
            // 301 Permanent Redirection
            // return RedirectToActionPermanent("Index", "Home", new { bookId = bookId, loggedIn = loggedIn});

            // 302 Temporary Redirection
            return RedirectToAction("Index", "Home", new { bookId = bookId, loggedIn = loggedIn, book = book});

            // specifying url in same application
            // return LocalRedirect("url");

            // specifying url in different application
            // return Redirect("url");
        }

        [Route("store/book")]
        public IActionResult Index(int? bookId, bool? loggedIn, Book book) // this is model binding
            // can accept inputs from urlencoded (default) or form data
            // where form data is better choice when handling file uploads and massive data
        {
            // Check if book id exist in the query string
            if (bookId.HasValue == false)
            { 
                return BadRequest("Book ID is not supplied");
            }

            // Book id string cannot be empty
            //string? bookId = Convert.ToString(Request.Query["bookid"]);
            if (bookId == 0)
            {
                return BadRequest("Book ID cannot be null or empty.");
            }

            // Book id must be between 1 and 1000
            int bookIdInt = Convert.ToInt16(bookId);
            if (bookIdInt < 1 || bookIdInt > 1000)
            {
                return NotFound("Book ID must be between 1 and 1000.");
            }

            // Check if loggedIn is true
            if(loggedIn.HasValue==false || loggedIn == false)
            {
                Response.StatusCode = 401;
                return Unauthorized("User must be authenticated.");
            }
            return File("/sample.jpg", "image/jpeg");
        }
    }
}
