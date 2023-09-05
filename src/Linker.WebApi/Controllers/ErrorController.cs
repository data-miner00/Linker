namespace Linker.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("/error")]
    public class ErrorController : ControllerBase
    {
        public IActionResult Index()
        {
            return this.Problem();
        }
    }
}
