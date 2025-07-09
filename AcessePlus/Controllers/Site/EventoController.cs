using Microsoft.AspNetCore.Mvc;
using AcessePlus.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AcessePlus.Modelo;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcessePlus.Controllers.Site
{
    [Route("eventos")]
    public class EventoController : Controller
    {
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult Index(string nome, int? tipoEventoId)
        {
            var todosEventos = new Negocio.Evento().BuscarTodos();

            IEnumerable<Evento> eventosFiltrados = todosEventos;

            if (!string.IsNullOrWhiteSpace(nome))
            {
                eventosFiltrados = eventosFiltrados.Where(l => l.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
            }

            if (tipoEventoId.HasValue && tipoEventoId > 0)
            {
                eventosFiltrados = eventosFiltrados.Where(l => l.TipoEvento.Id == tipoEventoId.Value);
            }

            var usuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado");

            var resultadosViewModel = eventosFiltrados.Select(l => new EventoViewModel
            {
                Id = l.Id,
                Nome = l.Nome,
                Descricao = l.Descricao
            }).ToList();

            var tipos = new Negocio.TipoLocal().BuscarTodos();

            var viewModel = new EventosBuscaViewModel
            {
                Resultados = resultadosViewModel,
                TiposDeEventos = new SelectList(tipos, "Id", "Descricao", tipoEventoId),
                Nome = nome,
                TipoEventoId = tipoEventoId,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Avaliar(int LocalId, string comentario, string tipoAcessibilidade, string tipo)
        {
            var usuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado");
            if (usuarioLogado == 0 || usuarioLogado == null)
                return RedirectToAction("Index", "Login");

            // Validações básicas
            if (string.IsNullOrWhiteSpace(comentario) || string.IsNullOrWhiteSpace(tipoAcessibilidade) || string.IsNullOrWhiteSpace(tipo))
            {
                TempData["Erro"] = "Todos os campos são obrigatórios.";
                return RedirectToAction("Index");
            }

            new Negocio.Avaliacao().Salvar(new Modelo.Avaliacao
            {
                Local = new Local() { Id = LocalId },
                Usuario = new Usuario() { Id = usuarioLogado.Value },
                Comentario = comentario,
                TipoAcessibilidade_Enum = (Modelo.Avaliacao.eTipoAcessibilidade)Convert.ToChar(tipoAcessibilidade),
                Tipo_Enum = (Modelo.Avaliacao.eTipo)Convert.ToChar(tipo)
            });

            TempData["Sucesso"] = true;
            return RedirectToAction("Index");
        }

        [HttpGet("Imagem/{localId}")]
        public IActionResult Imagem(int localId)
        {
            var negocioImagem = new AcessePlus.Negocio.LocalImagem();
            var imagens = negocioImagem.BuscarTodos()
                          .Where(img => img.LocalId == localId)
                          .OrderBy(img => img.Ordem)
                          .ToList();

            if (!imagens.Any())
                return NotFound();

            var primeiraImagem = imagens.First();

            return File(primeiraImagem.Imagem, "image/jpeg");
        }

        [Route("/eventos/{Id}")]
        public IActionResult Detail(int Id)
        {
            var local = new Negocio.Local().BuscarPorId(Id);
            var imagemUrl = Url.Action("Imagem", "Local", new { localId = local.Id });

            var avaliacoes = new Negocio.Avaliacao().BuscarTodos();

            ViewBag.JaAvaliado = avaliacoes.FirstOrDefault(x => x.Usuario.Id == Id) != null ? true : false;
            ViewBag.ImagemUrl = imagemUrl;
            ViewBag.Local = local;

            return View();
        }
    }
}