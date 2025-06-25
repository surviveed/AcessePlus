using Microsoft.AspNetCore.Mvc;
using AcessePlus.Negocio;
using AcessePlus.Models.ViewModels;
using AcessePlus.Modelo;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace AcessePlus.Controllers.Site
{
    [Route("Locais")]
    public class LocalController : Controller
    {
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult Index(string pesquisa = "", string filtro = "Nome")
        {
            var negocio = new AcessePlus.Negocio.Local();
            var locais = negocio.BuscarTodos();

            var viewModels = locais.Select(l => new LocalViewModel
            {
                Id = l.Id,
                Nome = l.Nome,
                Endereco = l.Endereco,
                Cidade = l.Cidade?.Descricao ?? "",
                Uf = l.Cidade?.Uf?.Descricao ?? "",
                Capacidade = l.Capacidade,
                Rating = null, //TODO: implementar depois
                ImagemUrl = "/images/default-local.jpg"
            }).ToList();

            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                if (filtro == "Nome")
                    viewModels = viewModels.Where(v => v.Nome.Contains(pesquisa, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(viewModels);
        }

        [HttpGet("Criar")]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Login");

            return View(new Modelo.Local());
        }

        [HttpPost("Criar")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Modelo.Local modelo)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
                return View(modelo);

            var negocio = new AcessePlus.Negocio.Local();
            negocio.Salvar(modelo);

            return RedirectToAction("Index");
        }
    }
}
