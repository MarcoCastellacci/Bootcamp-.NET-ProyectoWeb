using Codigo_Comun.Datos;
using Codigo_Comun.Entity;
using Codigo_Comun.Modelos.DTO;
using Codigo_Comun.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace WebAppStock.Controllers
{
    public class ArticulosController : Controller
    {
        private ArticuloServices articuloServices = new ArticuloServices();

        public IActionResult Index()
        {
            var articulo = articuloServices.GetArticulos();
            return View(articulo);
        }

        public IActionResult Create()
        {
            var articuloACompletar = new ArticuloDTO();
            return View(articuloACompletar);
        }

        [HttpPost]
        public IActionResult Create(ArticuloDTO articuloDTOAGuardar)
        {
            articuloDTOAGuardar = articuloServices.AgregarArticuloBD(articuloDTOAGuardar);

            if (articuloDTOAGuardar.Error == false)
            {
                return RedirectToAction("Index");
            }
            else
            {
                articuloDTOAGuardar.Error = true;
                articuloDTOAGuardar.Mensaje = "No se pudo Crear el Articulo";
                ViewBag.Mensaje = articuloDTOAGuardar.Mensaje;
                return View(articuloDTOAGuardar);
            }
        }

        public IActionResult Edit(int Id)
        {
            var articuloAEditar = articuloServices.ObtenerArticulosByID(Id);
            return View(articuloAEditar);
        }

        [HttpPost]
        public IActionResult Edit(ArticuloDTO articuloAGuardar)
        {
           
            articuloAGuardar = articuloServices.ModificarArticulo(articuloAGuardar);

            if (articuloAGuardar.Error == false)
            {
                return RedirectToAction("Index", "Articulos");
            }
            else
            {
                ViewBag.Mensaje = articuloAGuardar.Mensaje;
                return View(articuloAGuardar);
            }
        }

        public IActionResult Delete(int Id)
        {
            string mensaje = articuloServices.EliminarArticulo(Id);

            if (mensaje == "Articulo Eliminado de la Base de Dato")
            {
                return RedirectToAction("Index");
            }
            else
            {
               
                TempData["Mensaje"] = mensaje;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Stock(ArticuloDTO articuloAVerificar)
        {
            StockServices stockServices = new StockServices();
            List<Stock> stocks = stockServices.ObtenerTodosStocks();
            List<StockDTO> stockDTO = stocks.Select(item => new StockDTO { Id = item.Id, IdArticulo = item.IdArticulo, IdDeposito = item.IdDeposito, Cantidad = item.Cantidad }).ToList();
            
            int idParaFiltrar = articuloAVerificar.Id;
            stockDTO = stockDTO.Where(obj => obj.IdArticulo == idParaFiltrar).ToList();

            foreach (var stock in stockDTO)
            {
                var obj = stockServices.ObtenerStocksConDatos(stock);
            }
            

            return View(stockDTO);
        }
    }
}
