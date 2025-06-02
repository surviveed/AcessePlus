using Microsoft.AspNetCore.Mvc;
using AcessePlus.Modelo;
using AcessePlus.Models.ViewModels;

namespace AcessePlus.Controllers.Site
{
    [Route("/")]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            // ajustar p buscar do banco
            var locais = new List<Local>
            {
                new Local { Id = 1, Nome = "Praça Central", Endereco = "Rua Principal, 123", Observacoes = "Possui rampa e sinalização tátil" },
                new Local { Id = 2, Nome = "Biblioteca Municipal", Endereco = "Av. Leitura, 45", Observacoes = "Banheiro adaptado e elevador" },
                new Local { Id = 3, Nome = "Teatro da Cidade", Endereco = "Rua das Artes, 78", Observacoes = "Acesso com guia rebaixada e assentos reservados" }
            };

            var vm = new HomeViewModel
            {
                Locais = locais
            };

            return View(vm);
        }
    }
}
