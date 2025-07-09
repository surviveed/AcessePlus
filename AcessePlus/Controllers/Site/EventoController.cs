using Microsoft.AspNetCore.Mvc;
using AcessePlus.Modelo;
using AcessePlus.Negocio;
using AcessePlus.Models.ViewModels;
using System.Threading.Tasks;
using System;

namespace AcessePlus.Controllers.Site
{
    [Route("/eventos")]
    public class EventoController : Controller
    {
        [Route("/eventos")]
        public async Task<IActionResult> Index()
        {
            // var localNegocio = new AcessePlus.Negocio.Local();
            // var imagemNegocio = new AcessePlus.Negocio.LocalImagem();

            // var locais = localNegocio.BuscarTodos();

            // foreach (var local in locais)
            // {
            //     var imagens = imagemNegocio.BuscarPorLocal(local.Id);
            //     if (imagens.Any())
            //     {
            //         var imagemBase64 = Convert.ToBase64String(imagens.First().Imagem);
            //         local.ImagemUrl = $"data:image/png;base64,{imagemBase64}";
            //     }
            //     else
            //     {
            //         local.ImagemUrl = "/images/default.png";
            //     }
            // }

            // var vm = new HomeViewModel
            // {
            //     Locais = locais
            // };

            return View();
        }
    }
}