namespace Linker.Mvc.Controllers;

using Microsoft.AspNetCore.Mvc;

public class PlaylistController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
