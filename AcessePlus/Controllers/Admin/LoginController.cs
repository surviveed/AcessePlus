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
        var usuario = new Usuario().SearchEmailAndPassword(email, senha);
        
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

        if (usuario != null)
        {
            HttpContext.Session.SetString("UsuarioLogado", usuario.Email);
            return RedirectToAction("Index", "Dashboard");
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
            return RedirectToAction("Index"); 
        }
        catch (Exception ex)
        {
            ViewBag.Erro = ex.Message;
            return View(usuario); 
        }
    }

}
