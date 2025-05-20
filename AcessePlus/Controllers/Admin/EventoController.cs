using AcessePlus.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using AcessePlus.DataTransferObjects;

namespace AcessePlus.Controllers;

[Route("gerenciador")]
public class EventoController : Controller
{
    // Mocked data list
    List<EventoDTO> eventos = new List<EventoDTO>
{
    new EventoDTO { Id = 1, Nome = "Feira de Tecnologia", Descricao = "Evento sobre inovações tecnológicas.", Local = new LocalDTO { Nome = "Centro de Convenções", Endereco = "Av. Tech 1000" }, TipoEvento = new TipoEventoDTO { Nome = "Feira" } },
    new EventoDTO { Id = 2, Nome = "Simpósio de Saúde", Descricao = "Debates sobre saúde pública.", Local = new LocalDTO { Nome = "Auditório Municipal", Endereco = "Rua da Saúde, 200" }, TipoEvento = new TipoEventoDTO { Nome = "Simpósio" } },
    new EventoDTO { Id = 3, Nome = "Hackathon Universitário", Descricao = "Competição de programação.", Local = new LocalDTO { Nome = "Universidade X", Endereco = "Rua dos Estudantes, 300" }, TipoEvento = new TipoEventoDTO { Nome = "Hackathon" } },
    new EventoDTO { Id = 4, Nome = "Congresso de Direito", Descricao = "Palestras e debates jurídicos.", Local = new LocalDTO { Nome = "Centro Jurídico", Endereco = "Av. Justiça, 400" }, TipoEvento = new TipoEventoDTO { Nome = "Congresso" } },
    new EventoDTO { Id = 5, Nome = "Workshop de UX Design", Descricao = "Práticas de design centrado no usuário.", Local = new LocalDTO { Nome = "Espaço Criativo", Endereco = "Rua Design, 55" }, TipoEvento = new TipoEventoDTO { Nome = "Workshop" } },
    new EventoDTO { Id = 6, Nome = "Encontro de Startups", Descricao = "Networking entre empreendedores.", Local = new LocalDTO { Nome = "Incubadora ABC", Endereco = "Av. Start, 123" }, TipoEvento = new TipoEventoDTO { Nome = "Encontro" } },
    new EventoDTO { Id = 7, Nome = "Festival de Música", Descricao = "Atrações musicais diversas.", Local = new LocalDTO { Nome = "Parque Central", Endereco = "Av. das Flores, 987" }, TipoEvento = new TipoEventoDTO { Nome = "Festival" } },
    new EventoDTO { Id = 8, Nome = "Seminário de Educação", Descricao = "Discussões sobre políticas educacionais.", Local = new LocalDTO { Nome = "Instituto de Educação", Endereco = "Rua Ensino, 101" }, TipoEvento = new TipoEventoDTO { Nome = "Seminário" } },
    new EventoDTO { Id = 9, Nome = "Palestra Motivacional", Descricao = "Conferência com coach renomado.", Local = new LocalDTO { Nome = "Teatro Principal", Endereco = "Av. Cultura, 202" }, TipoEvento = new TipoEventoDTO { Nome = "Palestra" } },
    new EventoDTO { Id = 10, Nome = "Oficina de Robótica", Descricao = "Atividade prática para jovens.", Local = new LocalDTO { Nome = "Escola Técnica", Endereco = "Rua das Engenharias, 60" }, TipoEvento = new TipoEventoDTO { Nome = "Oficina" } }
};

    // GET: /gerenciador/evento
    [Route("")]
    public IActionResult List([FromQuery] int? page)
    {
        int pageSize = 5; // Número de itens por página
        int currentPage = page ?? 1; // Página atual, padrão é 1

        // Paginar a lista de eventos mockados
        var eventosPaginados = eventos
            .OrderBy(e => e.Id)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        int totalItems = eventos.Count;
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        ViewBag.CurrentPage = currentPage;
        ViewBag.TotalPages = totalPages;
        ViewBag.Eventos = eventosPaginados;

        return View();
    }

    // GET: /gerenciador/evento/edit/{id?}
    // [Route("edit/{id?}")]
    // public IActionResult Edit(int? id)
    // {
    //     var model = id.HasValue
    //         ? eventos.FirstOrDefault(x => x.Id == id.Value)
    //         : new EventoDTO();

    //     return View(model);
    // }

    // POST: /gerenciador/evento/edit-form/{id?}
    // [Route("edit-form/{id?}")]
    // public IActionResult Insert([FromForm] EventoDTO model, int? id)
    // {
    //     if (id.HasValue)
    //     {
    //         var existing = _mockData.FirstOrDefault(x => x.Id == id.Value);
    //         if (existing != null)
    //         {
    //             existing.Name = model.Name;
    //             existing.Description = model.Description;
    //         }
    //     }
    //     else
    //     {
    //         model.Id = _mockData.Any() ? _mockData.Max(x => x.Id) + 1 : 1;
    //         _mockData.Add(model);
    //     }

    //     return RedirectToAction("List");
    // }

    // POST: /gerenciador/evento/delete-form/{id}
    // [Route("delete-form/{id}")]
    // public IActionResult Delete(int id)
    // {
    //     var item = _mockData.FirstOrDefault(x => x.Id == id);
    //     if (item != null)
    //     {
    //         _mockData.Remove(item);
    //     }

    //     return RedirectToAction("List");
    // }
}
