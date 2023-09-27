namespace Linker.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The global exception handler.
    /// </summary>
    [Route("/error")]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// The handler for exceptions.
        /// </summary>
        /// <returns>Wrapped Http response.</returns>
        [HttpGet("", Name = "Error")]
        public IActionResult Index()
        {
            return this.Problem();
        }
    }
}
