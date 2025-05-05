using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OrderAPI.Controllers
{
    [Route("error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            return Problem(
                detail: exception.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An error occurred while processing your request."
            );
        }
    }
}
