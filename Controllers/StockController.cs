using Codigo_Comun.Datos;
using Codigo_Comun.Entity;
using Codigo_Comun.Modelos;
using Codigo_Comun.Modelos.DTO;
using Codigo_Comun.Negocio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppStock.ViewModels;

namespace WebAppStock.Controllers
{
    public class StockController : Controller
    {
        private StockServices stockServices = new StockServices();
        private StockRepository repository = new StockRepository();

        public IActionResult Index()
        {
                                 
            var stocks = stockServices.ObtenerTodosStocks();
            List<StockDTO> stockDTO = stocks
            .Select(item => new StockDTO { Id = item.Id, IdArticulo = item.IdArticulo, IdDeposito = item.IdDeposito, Cantidad = item.Cantidad}).ToList();
            foreach (var stock in stockDTO)
            {
                var obj = stockServices.ObtenerStocksConDatos(stock);
            }
            return View(stockDTO);
        }
        [HttpGet]
        public IActionResult Create()
        {

            StockViewModel stockViewModel = new StockViewModel();
            ArticuloServices articuloServices = new ArticuloServices();
            DepositoServices depositoServices = new DepositoServices();
            stockViewModel.Articulos = articuloServices.GetArticulos();
            stockViewModel.Depositos = depositoServices.ObtenerTodosDepositos();

            stockViewModel.selectDepositoList = new SelectList(stockViewModel.Depositos, "Id", "Nombre");
            stockViewModel.selectArticuloList = new SelectList(stockViewModel.Articulos, "Id", "Nombre");

            return View(stockViewModel);
        }

        [HttpPost]
        public IActionResult Create(StockViewModel stockViewModel)
        {
            stockViewModel.StockDTO = stockServices.AgregarStock(stockViewModel.StockDTO);

            if (stockViewModel.StockDTO.Error == false)
            {
                stockViewModel.StockDTO.Mensaje = "Articulo Agregado";
                ViewBag.Mensaje = stockViewModel.StockDTO.Mensaje;
                return RedirectToAction("Index");
            }
            else
            {
                stockViewModel.StockDTO.Error = true;
                //stockViewModel.StockDTO.Mensaje = "Ocurrio un error al Crear el Stock";
                ViewBag.Mensaje = stockViewModel.StockDTO.Mensaje;
                return View(stockViewModel);
            }
            
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            StockViewModel stockViewModel = new StockViewModel();
            ArticuloServices articuloServices = new ArticuloServices();
            DepositoServices depositoServices = new DepositoServices();
            stockViewModel.Articulos = articuloServices.GetArticulos();
            stockViewModel.Depositos = depositoServices.ObtenerTodosDepositos();
            stockViewModel.StockDTO = stockServices.GetStockById(Id);
            stockViewModel.selectDepositoList = new SelectList(stockViewModel.Depositos, "Id", "Nombre");
            stockViewModel.selectArticuloList = new SelectList(stockViewModel.Articulos, "Id", "Nombre");

            return View(stockViewModel);
        }

        [HttpPost]
        public IActionResult Edit(StockViewModel stockViewModel)
        {
            ArticuloServices articuloServices = new ArticuloServices();
            DepositoServices depositoServices = new DepositoServices();
            stockViewModel.Articulos = articuloServices.GetArticulos();
            stockViewModel.Depositos = depositoServices.ObtenerTodosDepositos();
            stockViewModel.StockDTO = stockServices.GetStockById(stockViewModel.StockDTO.Id);
            stockViewModel.selectDepositoList = new SelectList(stockViewModel.Depositos, "Id", "Nombre");
            stockViewModel.selectArticuloList = new SelectList(stockViewModel.Articulos, "Id", "Nombre");

            StockDTO stockDTO = stockServices.ModificarStock(stockViewModel.StockDTO);
            stockViewModel.StockDTO = stockDTO;

            if (stockViewModel.StockDTO.Error == false)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = stockViewModel.StockDTO.Mensaje;
                return View(stockViewModel);
            }
        }

        public IActionResult Delete(int Id)
        {
            string mensaje = stockServices.EliminarStock(Id);

            if (mensaje == "Stock Eliminado de la Base de Dato")
            {
                TempData["Mensaje"] = mensaje;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Mensaje"] = mensaje;
                return View();
            }
        }
    }
}
