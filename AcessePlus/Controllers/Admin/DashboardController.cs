using Microsoft.AspNetCore.Mvc;

namespace AcessePlus.Controllers;

[Route("gerenciador")]
public class DashboardController : Controller
{

    // GET: /gerenciador
    [Route("/gerenciador")]
    public IActionResult Index()
    {
        return View();
    }
}
