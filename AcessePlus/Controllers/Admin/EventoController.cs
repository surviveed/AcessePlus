using AcessePlus.DataTransferObjects;
using AcessePlus.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace AcessePlus.Controllers;

[Route("gerenciador/eventos")]
public class EventoController : Controller
{
    // Lista eventos paginados e retorna View
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

    // Método privado para paginação
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

        var resultado = new
        {
            CurrentPage = page,
            TotalPages = totalPages,
            TotalItems = totalItems,
            PageSize = pageSize,
            Data = eventosPaginados
        };

        return resultado;
    }

    // GET: /gerenciador/eventos/edit/{id?}
    [HttpGet("edit/{id?}")]
    public IActionResult Edit(int? id)
    {
        // Buscar todos os locais
        var locais = new Negocio.Local().BuscarTodos();
        ViewBag.Locais = locais;

        // Buscar todos os tipos de evento
        var tipos = new Negocio.TipoEvento().BuscarTodos();
        ViewBag.Tipos = tipos;


        if (id.HasValue)
        {
            // Busca evento existente para edição
            var evento = new Negocio.Evento().BuscarTodos().FirstOrDefault(e => e.Id == id.Value);
            if (evento == null)
            {
                return NotFound();
            }
            ViewBag.Evento = evento;
        }
        else
        {
            // Novo evento
            ViewBag.Evento = new Modelo.Evento();
        }

        return View();
    }

    // POST: /gerenciador/eventos/insert
    [HttpPost("insert")]
    public IActionResult Insert([FromForm] Modelo.Evento model)
    {
        // Gera novo ID
        var eventos = new Negocio.Evento().BuscarTodos();
        model.Id = eventos.Any() ? eventos.Max(e => e.Id) + 1 : 1;

        // Busca e atribui o Local com base no ID
        if (model.Local != null && model.Local.Id > 0)
        {
            var local = new Negocio.Local().BuscarTodos().FirstOrDefault(l => l.Id == model.Local.Id);
            if (local == null)
            {
                return BadRequest("Local inválido.");
            }
            model.Local = local;
        }

        // Busca e atribui o TipoEvento com base no ID
        if (model.TipoEvento != null && model.TipoEvento.Id > 0)
        {
            var tipoEvento = new Negocio.TipoEvento().BuscarTodos().FirstOrDefault(t => t.Id == model.TipoEvento.Id);
            if (tipoEvento == null)
            {
                return BadRequest("Tipo de Evento inválido.");
            }
            model.TipoEvento = tipoEvento;
        }

        // Salva o novo evento
        new Negocio.Evento().Salvar(model);

        return RedirectToAction("List");
    }


    // POST: /gerenciador/eventos/update/{id}
    [HttpPost("update/{id}")]
    public IActionResult Update(int id, [FromForm] EventoDTO model)
    {
        var eventos = new Negocio.Evento().BuscarTodos();
        var existing = eventos.FirstOrDefault(e => e.Id == id);

        if (existing == null)
        {
            return NotFound();
        }

        // Atualiza campos simples
        existing.Nome = model.Nome;
        existing.Descricao = model.Descricao;

        // Atualiza Local, se necessário
        if (existing.Local == null || existing.Local.Id != model.LocalId)
        {
            var newLocal = new Negocio.Local().BuscarTodos().FirstOrDefault(x => x.Id == model.LocalId);
            if (newLocal == null)
            {
                return BadRequest("Local não encontrado.");
            }
            existing.Local = newLocal;
        }

        // Atualiza TipoEvento, se necessário
        if (existing.TipoEvento == null || existing.TipoEvento.Id != model.TipoEventoId)
        {
            var newTipoEvento = new Negocio.TipoEvento().BuscarTodos().FirstOrDefault(x => x.Id == model.TipoEventoId);
            if (newTipoEvento == null)
            {
                return BadRequest("Tipo de Evento não encontrado.");
            }
            existing.TipoEvento = newTipoEvento;
        }

        new Negocio.Evento().Salvar(existing);

        return RedirectToAction("List");
    }


    // POST: /gerenciador/eventos/delete/{id}
    [HttpPost("delete/{id}")]
    public IActionResult Delete(int id)
    {
        var negocioEvento = new Negocio.Evento();
        var evento = negocioEvento.BuscarTodos().FirstOrDefault(e => e.Id == id);

        if (evento == null)
        {
            return NotFound();
        }

        negocioEvento.Excluir(evento.Id);

        return RedirectToAction("List");
    }
}
