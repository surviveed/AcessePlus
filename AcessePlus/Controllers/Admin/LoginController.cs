using AcessePlus.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace AcessePlus.Controllers;

[Route("gerenciador/login")]
public class LoginController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    [Route("/gerenciador/check")]
    public IActionResult Check(string username, string password)
    {
        if (username == "admin" && password == "admin")
            return Redirect("/gerenciador");
        else
        {
            TempData["ErrorMessage"] = "Username or password is incorrect.";
            return Redirect("/gerenciador/login");
        }
    }
}
