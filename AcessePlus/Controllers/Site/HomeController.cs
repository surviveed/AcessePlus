using Microsoft.AspNetCore.Mvc;

namespace AcessePlus.Controllers;

[Route("/")]
public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
