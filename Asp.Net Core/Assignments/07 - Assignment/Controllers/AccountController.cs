using Microsoft.AspNetCore.Mvc;
using Assignment;
using System.Text.Json;

namespace Assignment.Controllers
{
    public class AccountController : Controller
    {
        Account account = new Account
        {
            accountNumber = 1001,
            accountHolderName = "John Doe",
            currentBalance = 5000
        };

        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Best Bank");
        }

        [Route("/account-details")]
        public IActionResult AccountDetails()
        {
            return Ok(JsonSerializer.Serialize(account));
        }

        [Route("/account-statement")]
        public IActionResult AccountStatement()
        {
            return File("Resume 2025 .pdf", "application/pdf");
        }

        [Route("/get-current-balance/{accountNumber:int?}")]
        public IActionResult GetCurrentBalance(string accountNumber)
        {
            int accNum;
            if (string.IsNullOrEmpty(accountNumber))
            {
                return Content("Account number should be supplied");
            }
            accNum = int.Parse(accountNumber);
            if (accNum != 1001)
            {
                return BadRequest("Account Number should be 1001");
            }
            return Ok(account.currentBalance);
        }
    }
}
