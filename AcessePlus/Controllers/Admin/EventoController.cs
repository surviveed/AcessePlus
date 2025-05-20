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

    // Mocked locais
    List<LocalDTO> locais = new List<LocalDTO>
{
    new LocalDTO { Id = 1, Nome = "Centro de Convenções", Endereco = "Av. Tech 1000" },
    new LocalDTO { Id = 2, Nome = "Auditório Municipal", Endereco = "Rua da Saúde, 200" },
    new LocalDTO { Id = 3, Nome = "Universidade X", Endereco = "Rua dos Estudantes, 300" },
    new LocalDTO { Id = 4, Nome = "Centro Jurídico", Endereco = "Av. Justiça, 400" },
    new LocalDTO { Id = 5, Nome = "Espaço Criativo", Endereco = "Rua Design, 55" },
    new LocalDTO { Id = 6, Nome = "Incubadora ABC", Endereco = "Av. Start, 123" },
    new LocalDTO { Id = 7, Nome = "Parque Central", Endereco = "Av. das Flores, 987" },
    new LocalDTO { Id = 8, Nome = "Instituto de Educação", Endereco = "Rua Ensino, 101" },
    new LocalDTO { Id = 9, Nome = "Teatro Principal", Endereco = "Av. Cultura, 202" },
    new LocalDTO { Id = 10, Nome = "Escola Técnica", Endereco = "Rua das Engenharias, 60" }
};

    // Mocked tipos de evento
    List<TipoEventoDTO> tiposEvento = new List<TipoEventoDTO>
{
    new TipoEventoDTO { Id = 1, Nome = "Feira" },
    new TipoEventoDTO { Id = 2, Nome = "Simpósio" },
    new TipoEventoDTO { Id = 3, Nome = "Hackathon" },
    new TipoEventoDTO { Id = 4, Nome = "Congresso" },
    new TipoEventoDTO { Id = 5, Nome = "Workshop" },
    new TipoEventoDTO { Id = 6, Nome = "Encontro" },
    new TipoEventoDTO { Id = 7, Nome = "Festival" },
    new TipoEventoDTO { Id = 8, Nome = "Seminário" },
    new TipoEventoDTO { Id = 9, Nome = "Palestra" },
    new TipoEventoDTO { Id = 10, Nome = "Oficina" }
};

    // GET: /gerenciador/evento
    [Route("evento")]
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
    [Route("evento/edit/{id?}")]
    public IActionResult Edit(int? id)
    {
        var model = id.HasValue
            ? eventos.FirstOrDefault(x => x.Id == id.Value)
            : new EventoDTO();

        ViewBag.Locais = locais;
        ViewBag.Tipos = tiposEvento;

        return View(model);
    }

    // POST: /gerenciador/evento/edit-form/{id?}
    [Route("edit-form/{id?}")]
    public IActionResult Insert([FromForm] EventoDTO model, int? id)
    {
        if (id.HasValue)
        {
            var existing = eventos.FirstOrDefault(x => x.Id == id.Value);
            if (existing != null)
            {
                // Atualiza os campos básicos
                existing.Nome = model.Nome;
                existing.Descricao = model.Descricao;

                // Atualiza os dados do local
                if (existing.Local == null)
                    existing.Local = new LocalDTO();

                existing.Local.Nome = model.Local?.Nome;
                existing.Local.Endereco = model.Local?.Endereco;

                // Atualiza os dados do tipo de evento
                if (existing.TipoEvento == null)
                    existing.TipoEvento = new TipoEventoDTO();

                existing.TipoEvento.Nome = model.TipoEvento?.Nome;
            }
        }
        else
        {
            // Define novo ID para o novo evento
            model.Id = eventos.Any() ? eventos.Max(x => x.Id) + 1 : 1;

            // Cria instâncias para Local e TipoEvento caso venham nulas
            model.Local ??= new LocalDTO();
            model.TipoEvento ??= new TipoEventoDTO();

            eventos.Add(model);
        }

        return RedirectToAction("List");
    }

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
