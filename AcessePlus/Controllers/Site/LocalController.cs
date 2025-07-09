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
                JaAvaliado = usuarioLogado != null ? l.Avaliacoes.Any(x => x.Usuario.Id == usuarioLogado) : false,
                QtdAvaliacoesPositivas = l.Avaliacoes.Count(x => x.Tipo_Enum == Modelo.Avaliacao.eTipo.Positiva),
                QtdAvaliacoesNegativas = l.Avaliacoes.Count(x => x.Tipo_Enum == Modelo.Avaliacao.eTipo.Negativa),
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
        private void CarregarListasParaView(int? paisId, int? ufId, int? tipoLocalId = null)
        {
            if (paisId.HasValue)
                ViewBag.Ufs = new AcessePlus.Negocio.Uf().BuscarTodos().Where(u => u.Pais.Id == paisId).ToList();
            else
                ViewBag.Ufs = new List<AcessePlus.Modelo.Uf>();

            if (ufId.HasValue)
                ViewBag.Cidades = new AcessePlus.Negocio.Cidade().BuscarTodos().Where(c => c.Uf.Id == ufId).ToList();
            else
                ViewBag.Cidades = new List<AcessePlus.Modelo.Cidade>();

            var tipos = new AcessePlus.Negocio.TipoLocal().BuscarTodos();
            ViewBag.Tipos = tipos;

            ViewBag.TipoLocalSelecionado = tipoLocalId;
        }

        [HttpGet("Criar")]
        public IActionResult Create()
        {
            var usuarioLogado = HttpContext.Session.GetString("UsuarioLogado");
            if (string.IsNullOrEmpty(usuarioLogado))
                return RedirectToAction("Index", "Login");

            var paises = new AcessePlus.Negocio.Pais().BuscarTodos();
            ViewBag.Paises = paises;

            CarregarListasParaView(null, null, null);

            return View(new LocalCreateViewModel());
        }

        [HttpPost("Criar")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LocalCreateViewModel vm)
        {
            if (vm.Capacidade <= 0)
            {
                ModelState.AddModelError("Capacidade", "A capacidade deve ser maior que zero.");
            }

            if (vm.Imagens == null || vm.Imagens.Count == 0)
            {
                ModelState.AddModelError("Imagens", "Por favor, envie pelo menos uma foto do local.");
            }

            if (vm.TipoLocalId == 0)
            {
                ModelState.AddModelError("TipoLocalId", "Selecione um tipo de local válido.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Paises = new AcessePlus.Negocio.Pais().BuscarTodos();
                CarregarListasParaView(vm.PaisId, vm.UfId, vm.TipoLocalId);
                return View(vm);
            }

            var cidade = new AcessePlus.Negocio.Cidade().BuscarTodos()
                .FirstOrDefault(c => c.Id == vm.CidadeId);

            if (cidade == null)
            {
                ModelState.AddModelError("", "Cidade inválida.");
                ViewBag.Paises = new AcessePlus.Negocio.Pais().BuscarTodos();
                CarregarListasParaView(vm.PaisId, vm.UfId, vm.TipoLocalId);
                return View(vm);
            }

            var tipoLocal = new AcessePlus.Negocio.TipoLocal()
                .BuscarTodos()
                .FirstOrDefault(t => t.Id == vm.TipoLocalId);

            if (tipoLocal == null)
            {
                ModelState.AddModelError("TipoLocalId", "Tipo de local inválido.");
                ViewBag.Paises = new AcessePlus.Negocio.Pais().BuscarTodos();
                CarregarListasParaView(vm.PaisId, vm.UfId, vm.TipoLocalId);
                return View(vm);
            }

            var local = new Modelo.Local
            {
                Nome = vm.Nome,
                Capacidade = vm.Capacidade,
                Endereco = vm.Endereco,
                Cidade = cidade,
                TipoLocal = tipoLocal
            };

            var negocio = new AcessePlus.Negocio.Local();
            negocio.Salvar(local);

            if (vm.Imagens != null && vm.Imagens.Count > 0)
            {
                var negocioImagem = new AcessePlus.Negocio.LocalImagem();
                int ordem = 0;

                foreach (var imagem in vm.Imagens)
                {
                    using var ms = new MemoryStream();
                    imagem.CopyTo(ms);
                    var bytes = ms.ToArray();

                    var localImagem = new Modelo.LocalImagem
                    {
                        LocalId = local.Id,
                        Imagem = bytes,
                        NomeArquivo = imagem.FileName,
                        Ordem = ordem++,
                        DataCadastro = DateTime.Now
                    };

                    negocioImagem.Salvar(localImagem);
                }
            }

            TempData["Sucesso"] = true;
            return RedirectToAction("Create");
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

            // Aqui você pode salvar no banco, ex:
            // _context.Avaliacoes.Add(new Avaliacao { ... });

            TempData["Sucesso"] = true;
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
            ViewBag.Local = new Negocio.Local().BuscarPorId(Id);
            return View();
        }
    }
}