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
        if (id.HasValue)
        {
            // Buscar todos os locais
            var locais = new Negocio.Local().BuscarTodos();
            ViewBag.Locais = locais;

            // Buscar todos os tipos de evento
            var tipos = new Negocio.TipoEvento().BuscarTodos();
            ViewBag.Tipos = tipos;

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

        // Cria instâncias caso sejam nulas
        model.Local ??= new Modelo.Local();
        model.TipoEvento ??= new Modelo.TipoEvento();

        // Aqui você deveria ter uma camada de serviço ou repositório:
        new Negocio.Evento().Salvar(model);

        return RedirectToAction("List");
    }

    // POST: /gerenciador/eventos/update/{id}
    [HttpPost("update/{id}")]
    public IActionResult Update(int id, [FromForm] Evento model)
    {
        var eventos = new Negocio.Evento().BuscarTodos();
        var existing = eventos.FirstOrDefault(e => e.Id == id);

        if (existing == null)
        {
            return NotFound();
        }

        // Atualiza campos
        existing.Nome = model.Nome;
        existing.Descricao = model.Descricao;

        existing.Local ??= new Local();
        existing.Local.Nome = model.Local?.Nome;
        existing.Local.Endereco = model.Local?.Endereco;

        existing.TipoEvento ??= new TipoEvento();
        existing.TipoEvento.Descricao = model.TipoEvento?.Descricao;

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
