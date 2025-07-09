using Microsoft.AspNetCore.Mvc;
using AcessePlus.Modelo;
using AcessePlus.Models.ViewModels;

namespace AcessePlus.Controllers
{
    [Route("gerenciador/adm-eventos")]
    public class AdminEventoController : Controller
    {
        [HttpGet("")]
        public IActionResult List([FromQuery] int? page)
        {
            int currentPage = page ?? 1;
            var resultado = PaginacaoEventos(currentPage, 5);

            ViewBag.CurrentPage = resultado.CurrentPage;
            ViewBag.TotalPages = resultado.TotalPages;
            ViewBag.TotalItems = resultado.TotalItems;
            ViewBag.PageSize = resultado.PageSize;
            ViewBag.Eventos = resultado.Data;

            return View();
        }

        private static dynamic PaginacaoEventos(int page, int pageSize)
        {
            var eventos = new Negocio.Evento().BuscarTodos();

            var eventosPaginados = eventos
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalItems = eventos.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize,
                Data = eventosPaginados
            };
        }

        [HttpGet("edit-view/{Id?}")]
        public IActionResult Edit(string? Id)
        {
            if (!UsuarioEstaLogado())
                return RedirectToAction("Index", "Login");

            Evento vm;

            if (!string.IsNullOrEmpty(Id))
            {
                var evento = new Negocio.Evento().BuscarPorId(Convert.ToInt32(Id));
                if (evento == null)
                    return NotFound();

                vm = evento;
            }
            else
            {
                vm = new Evento();
            }

            CarregarListasParaView();
            return View("Edit", vm);
        }

        private void CarregarListasParaView()
        {
            ViewBag.Locais = new Negocio.Local().BuscarTodos();
            ViewBag.Tipos = new Negocio.TipoEvento().BuscarTodos();
        }

        [HttpPost("Criar")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventoViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Nome))
                ModelState.AddModelError("Nome", "O nome do evento é obrigatório.");

            if (string.IsNullOrWhiteSpace(vm.Descricao))
                ModelState.AddModelError("Descricao", "A descrição é obrigatória.");

            if (vm.LocalId == 0)
                ModelState.AddModelError("Local", "Selecione um local válido.");

            if (vm.TipoEventoId == 0)
                ModelState.AddModelError("TipoEvento", "Selecione um tipo válido.");

            if (!ModelState.IsValid)
            {
                CarregarListasParaView();
                return View("Edit", vm);
            }

            var evento = vm.Id == 0 ? new Evento() : new Negocio.Evento().BuscarPorId(vm.Id) ?? new Evento();

            evento.Nome = vm.Nome;
            evento.Descricao = vm.Descricao;
            evento.Local = new Negocio.Local().BuscarPorId(vm.LocalId);
            evento.TipoEvento = new Negocio.TipoEvento().BuscarPorId(vm.TipoEventoId);

            new Negocio.Evento().Salvar(evento);

            TempData["Sucesso"] = true;
            return RedirectToAction("Edit", new { id = evento.Id });
        }

        private bool UsuarioEstaLogado()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogado"));
        }
    }
}
