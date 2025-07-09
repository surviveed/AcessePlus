using Microsoft.AspNetCore.Mvc;
using AcessePlus.Modelo;

namespace AcessePlus.Controllers
{
    [Route("gerenciador/adm-locais")]
    public class AdminLocalController : Controller
    {
        [HttpGet("")]
        public IActionResult List([FromQuery] int? page)
        {
            int currentPage = page ?? 1;

            var resultado = PaginacaoLocais(currentPage, 5);

            ViewBag.CurrentPage = resultado.CurrentPage;
            ViewBag.TotalPages = resultado.TotalPages;
            ViewBag.TotalItems = resultado.TotalItems;
            ViewBag.PageSize = resultado.PageSize;
            ViewBag.Locais = resultado.Data;

            return View();
        }

        private static dynamic PaginacaoLocais(int page, int pageSize)
        {
            var locais = new Negocio.Local().BuscarTodos();

            var locaisPaginados = locais
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalItems = locais.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var resultado = new
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize,
                Data = locaisPaginados
            };

            return resultado;
        }

        private void CarregarListasParaView(int? paisId, int? ufId, int? tipoLocalId = null)
        {
            ViewBag.Ufs = paisId.HasValue
                ? new Negocio.Uf().BuscarTodos().Where(u => u.Pais.Id == paisId).ToList()
                : new List<Uf>();

            ViewBag.Cidades = ufId.HasValue
                ? new AcessePlus.Negocio.Cidade().BuscarTodos().Where(c => c.Uf.Id == ufId).ToList()
                : new List<Cidade>();

            ViewBag.Paises = new Negocio.Pais().BuscarTodos();
            ViewBag.Tipos = new Negocio.TipoLocal().BuscarTodos();
            ViewBag.TipoLocalSelecionado = tipoLocalId;
        }

        [Route("edit-view/{Id?}")]
        public IActionResult Edit(string? Id)
        {
            var usuarioLogado = HttpContext.Session.GetString("UsuarioLogado");
            if (string.IsNullOrEmpty(usuarioLogado))
                return RedirectToAction("Index", "Login");

            LocalCreateViewModel vm;

            if (!string.IsNullOrEmpty(Id))
            {
                var local = new Negocio.Local().BuscarPorId(Convert.ToInt32(Id));
                if (local == null)
                    return NotFound();

                vm = new LocalCreateViewModel
                {
                    Id = local.Id,
                    Nome = local.Nome,
                    Capacidade = local.Capacidade,
                    Endereco = local.Endereco,
                    CidadeId = local.Cidade?.Id ?? 0,
                    UfId = local.Cidade?.Uf?.Id ?? 0,
                    PaisId = local.Cidade?.Uf?.Pais?.Id ?? 0,
                    TipoLocalId = local.TipoLocal?.Id ?? 0
                };

                CarregarListasParaView(vm.PaisId, vm.UfId, vm.TipoLocalId);
            }
            else
            {
                vm = PrepararViewModelParaCriacao();
            }

            return View("Edit", vm);
        }

        private LocalCreateViewModel PrepararViewModelParaCriacao()
        {
            var vm = new LocalCreateViewModel();
            ViewBag.Paises = new Negocio.Pais().BuscarTodos();
            CarregarListasParaView(null, null, null);
            return vm;
        }

        [HttpPost("Criar")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LocalCreateViewModel vm)
        {
            if (vm.Capacidade <= 0)
                ModelState.AddModelError("Capacidade", "A capacidade deve ser maior que zero.");

            if ((vm.Imagens == null || vm.Imagens.Count == 0) && vm.Id == 0)
                ModelState.AddModelError("Imagens", "Por favor, envie pelo menos uma foto do local.");

            if (vm.TipoLocalId == 0)
                ModelState.AddModelError("TipoLocalId", "Selecione um tipo de local válido.");

            if (!ModelState.IsValid)
            {
                ViewBag.Paises = new Negocio.Pais().BuscarTodos();
                CarregarListasParaView(vm.PaisId, vm.UfId, vm.TipoLocalId);
                return View("Edit", vm);
            }

            var cidade = new Negocio.Cidade().BuscarTodos().FirstOrDefault(c => c.Id == vm.CidadeId);
            var tipoLocal = new Negocio.TipoLocal().BuscarTodos().FirstOrDefault(t => t.Id == vm.TipoLocalId);

            if (cidade == null || tipoLocal == null)
            {
                ModelState.AddModelError("", "Cidade ou Tipo de local inválido.");
                ViewBag.Paises = new Negocio.Pais().BuscarTodos();
                CarregarListasParaView(vm.PaisId, vm.UfId, vm.TipoLocalId);
                return View("Edit", vm);
            }

            var negocio = new Negocio.Local();
            Modelo.Local local;

            if (vm.Id == 0)
            {
                local = new Modelo.Local();
            }
            else
            {
                local = negocio.BuscarPorId(vm.Id);
                if (local == null)
                    return NotFound();
            }

            local.Nome = vm.Nome;
            local.Capacidade = vm.Capacidade;
            local.Endereco = vm.Endereco;
            local.Cidade = cidade;
            local.TipoLocal = tipoLocal;

            negocio.Salvar(local);

            SalvarImagens(vm.Imagens, local.Id);

            TempData["Sucesso"] = true;
            return RedirectToAction("Edit", new { id = local.Id });
        }

        private void SalvarImagens(IList<IFormFile> imagens, int localId)
        {
            if (imagens == null || imagens.Count == 0) return;

            var negocioImagem = new AcessePlus.Negocio.LocalImagem();
            int ordem = 0;

            foreach (var imagem in imagens)
            {
                using var ms = new MemoryStream();
                imagem.CopyTo(ms);
                var bytes = ms.ToArray();

                var localImagem = new Modelo.LocalImagem
                {
                    LocalId = localId,
                    Imagem = bytes,
                    NomeArquivo = imagem.FileName,
                    Ordem = ordem++,
                    DataCadastro = DateTime.Now
                };

                negocioImagem.Salvar(localImagem);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Avaliar(int LocalId, string comentario, string tipoAcessibilidade, string tipo)
        {
            var usuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado");
            if (usuarioLogado == 0 || usuarioLogado == null)
                return RedirectToAction("Index", "Login");

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

        [Route("buscar-ufs-por-pais/{id}")]
        public IActionResult BuscarUFsPorPais(int id)
        {
            var ufs = new Negocio.Uf().BuscarTodos().Where(uf => uf.Pais.Id == id).ToList();
            var resultado = ufs.Select(uf => new { uf.Id, uf.Descricao }).ToList();
            return Json(resultado);
        }

        [HttpGet("BuscarCidadesPorUf/{id}")]
        public IActionResult BuscarCidadesPorUf(int id)
        {
            var cidades = new Negocio.Cidade().BuscarTodos().Where(c => c.Uf != null && c.Uf.Id == id).ToList();
            var resultado = cidades.Select(c => new { c.Id, c.Descricao }).ToList();
            return Json(resultado);
        }

        [HttpGet("Imagem/{localId}")]
        public IActionResult Imagem(int localId)
        {
            var imagens = new Negocio.LocalImagem().BuscarTodos()
                          .Where(img => img.LocalId == localId)
                          .OrderBy(img => img.Ordem)
                          .ToList();

            if (!imagens.Any())
                return NotFound();

            var primeiraImagem = imagens.First();
            return File(primeiraImagem.Imagem, "image/jpeg");
        }

        private bool UsuarioEstaLogado()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogado"));
        }
    }
}
