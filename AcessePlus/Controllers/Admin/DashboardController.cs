using Microsoft.AspNetCore.Mvc;

namespace AcessePlus.Controllers;

[Route("gerenciador")]
public class DashboardController : Controller
{

    // GET: /gerenciador
    [Route("/gerenciador")]
    public IActionResult Index()
    {
        var usuarioLogado = HttpContext.Session.GetString("UsuarioLogado");
        if (string.IsNullOrEmpty(usuarioLogado))
        {
            return RedirectToAction("Index", "Login");
        }
        return View();
    }
}
