using Microsoft.AspNetCore.Mvc;
using AcessePlus.Models.ViewModels;
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

        private void CarregarListasParaView(int? paisId, int? ufId)
        {
            if (paisId.HasValue)
                ViewBag.Ufs = new AcessePlus.Negocio.Uf().BuscarTodos().Where(u => u.Pais.Id == paisId).ToList();
            else
                ViewBag.Ufs = new List<AcessePlus.Modelo.Uf>();

            if (ufId.HasValue)
                ViewBag.Cidades = new AcessePlus.Negocio.Cidade().BuscarTodos().Where(c => c.Uf.Id == ufId).ToList();
            else
                ViewBag.Cidades = new List<AcessePlus.Modelo.Cidade>();
        }

        [HttpGet("Criar")]
        public IActionResult Create()
        {
            var usuarioLogado = HttpContext.Session.GetString("UsuarioLogado");
            if (string.IsNullOrEmpty(usuarioLogado))
                return RedirectToAction("Index", "Login");

            var paises = new AcessePlus.Negocio.Pais().BuscarTodos();
            ViewBag.Paises = paises;

            CarregarListasParaView(null, null);

            return View(new LocalCreateViewModel());
        }

        [HttpPost("Criar")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LocalCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = new AcessePlus.Negocio.Pais().BuscarTodos();
                CarregarListasParaView(vm.PaisId, vm.UfId);
                return View(vm);
            }

            var cidade = new AcessePlus.Negocio.Cidade().BuscarTodos().FirstOrDefault(c => c.Id == vm.CidadeId);
            if (cidade == null)
            {
                ModelState.AddModelError("", "Cidade inválida.");
                ViewBag.Paises = new AcessePlus.Negocio.Pais().BuscarTodos();
                CarregarListasParaView(vm.PaisId, vm.UfId);
                return View(vm);
            }

            var local = new Modelo.Local
            {
                Nome = vm.Nome,
                Capacidade = vm.Capacidade,
                Endereco = vm.Endereco,
                Observacoes = vm.Observacoes,
                Cidade = cidade
            };

            var negocio = new AcessePlus.Negocio.Local();
            negocio.Salvar(local);

            return RedirectToAction("Index");
        }

        [HttpGet("BuscarUFsPorPais/{id}")]
        public IActionResult BuscarUFsPorPais(int id)
        {
            var ufs = new AcessePlus.Negocio.Uf().BuscarTodos().Where(uf => uf.Pais.Id == id).ToList();
            var resultado = ufs.Select(uf => new { uf.Id, uf.Descricao }).ToList();
            return Json(resultado);
        }

        [HttpGet("BuscarCidadesPorUf/{id}")]
        public IActionResult BuscarCidadesPorUf(int id)
        {
            var cidades = new AcessePlus.Negocio.Cidade().BuscarTodos().Where(c => c.Uf != null && c.Uf.Id == id).ToList();
            var resultado = cidades.Select(c => new { c.Id, c.Descricao }).ToList();
            return Json(resultado);
        }
    }
}
