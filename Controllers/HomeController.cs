using CodigoComun.Negocio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppStock.Models;

namespace WebAppStock.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Prueba()
        {
            ProfesorServices profesorServices = new ProfesorServices();
            var profesor = profesorServices.GetProfesorPorId(6);
            return View(profesor);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}