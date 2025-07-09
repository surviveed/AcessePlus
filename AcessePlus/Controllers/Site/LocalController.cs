using Microsoft.AspNetCore.Mvc;
using AcessePlus.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AcessePlus.Modelo;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcessePlus.Controllers.Site
{
    [Route("Locais")]
    public class LocalController : Controller
    {
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult Index(string nome, int? tipoLocalId, int? capacidade)
        {
            var negocioLocal = new AcessePlus.Negocio.Local();
            var todosLocais = negocioLocal.BuscarTodos();

            IEnumerable<AcessePlus.Modelo.Local> locaisFiltrados = todosLocais;

            if (!string.IsNullOrWhiteSpace(nome))
            {
                locaisFiltrados = locaisFiltrados.Where(l => l.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
            }

            if (tipoLocalId.HasValue && tipoLocalId > 0)
            {
                locaisFiltrados = locaisFiltrados.Where(l => l.TipoLocal.Id == tipoLocalId.Value);
            }

            if (capacidade.HasValue && capacidade > 0)
            {
                locaisFiltrados = locaisFiltrados.Where(l => l.Capacidade >= capacidade.Value);
            }
            var usuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado");

            var resultadosViewModel = locaisFiltrados.Select(l => new LocalViewModel
            {
                Id = l.Id,
                Nome = l.Nome,
                Endereco = l.Endereco,
                Cidade = l.Cidade?.Descricao ?? "",
                Uf = l.Cidade?.Uf?.Descricao ?? "",
                Capacidade = l.Capacidade,
                TipoLocalDescricao = l.TipoLocal?.Descricao ?? "",
                ImagemUrl = Url.Action("Imagem", "Local", new { localId = l.Id }),
            }).ToList();

            var tipos = new AcessePlus.Negocio.TipoLocal().BuscarTodos();

            var viewModel = new LocaisBuscaViewModel
            {
                Resultados = resultadosViewModel,
                TiposDeLocal = new SelectList(tipos, "Id", "Descricao", tipoLocalId),
                Nome = nome,
                TipoLocalId = tipoLocalId,
                Capacidade = capacidade
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

        [Route("/locais/{Id}")]
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