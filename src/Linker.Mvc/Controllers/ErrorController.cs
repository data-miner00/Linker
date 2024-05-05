namespace Linker.Mvc.Controllers;

using Linker.Core.V2.Models;
using Microsoft.AspNetCore.Mvc;

public sealed class ErrorController : Controller
{
    public IActionResult Index()
    {
        return this.View();
    }
}
