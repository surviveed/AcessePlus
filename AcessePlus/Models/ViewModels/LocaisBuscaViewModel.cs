using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AcessePlus.Models.ViewModels
{
    public class LocaisBuscaViewModel
    {
        public string? Nome { get; set; }
        public int? TipoLocalId { get; set; }
        public int? Capacidade { get; set; }
        public SelectList? TiposDeLocal { get; set; }
        public List<LocalViewModel> Resultados { get; set; } = new List<LocalViewModel>();
    }
}
