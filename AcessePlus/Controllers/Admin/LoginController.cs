using Microsoft.AspNetCore.Mvc;
using AcessePlus.Negocio;
namespace AcessePlus.Controllers;

[Route("gerenciador/login")]
public class LoginController : Controller
{

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("")]
    public IActionResult Index(string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
        {
            ViewBag.Erro = "Preencha todos os campos.";
            return View();
        }
        else if (!email.Contains("@") || !email.Contains("."))
        {
            ViewBag.Erro = "Email inválido.";
            return View();
        }

        var usuario = new Usuario().SearchEmailAndPassword(email, senha);

        if (usuario != null)
        {
            HttpContext.Session.SetString("UsuarioLogado", usuario.Email);
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            return RedirectToAction("Login", "Home");
        }

        ViewBag.Erro = "Email ou senha inválidos.";
        return View();
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    public IActionResult Register(AcessePlus.Modelo.Usuario usuario)
    {
        try
        {
            new Usuario().Cadastrar(usuario);

            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);

            return RedirectToAction("Login", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Erro = ex.Message;
            return View(usuario);
        }
    }

    [HttpGet("/gerenciador/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Home");
    }

}
