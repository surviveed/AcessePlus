using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AcessePlus.Models.ViewModels
{
    public class EventosBuscaViewModel
    {
        public string? Nome { get; set; }
        public int? TipoEventoId { get; set; }
        public int? Capacidade { get; set; }
        public SelectList? TiposDeEventos { get; set; }
        public List<EventoViewModel> Resultados { get; set; } = new List<EventoViewModel>();
    }
}
