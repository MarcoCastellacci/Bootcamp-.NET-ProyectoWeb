using Codigo_Comun.Entity;
using Codigo_Comun.Modelos.DTO;
using Codigo_Comun.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace WebAppStock.Controllers
{
    public class DepositosController : Controller
    {
        private DepositoServices depositosServices = new DepositoServices();
        public IActionResult Index(int Id)
        {
            var depositos = depositosServices.ObtenerTodosDepositos();
            return View(depositos);
        }

        public IActionResult Create()
        {
            var depositoACompletar = new DepositoDTO();
            return View(depositoACompletar);
        }

        [HttpPost]
        public IActionResult Create(DepositoDTO depositoDTOAAgregar)
        {
            depositoDTOAAgregar = depositosServices.AddDeposito(depositoDTOAAgregar);

            if (depositoDTOAAgregar.Error == false)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = depositoDTOAAgregar.Mensaje;
                return View(depositoDTOAAgregar);
            }
        }


        public IActionResult Edit(int Id)
        {
            var depositoAEditar = depositosServices.ObtenerDepositoById(Id);
            return View(depositoAEditar);
        }

        [HttpPost]
        public IActionResult Edit(DepositoDTO depositoAGuardar)
        {

            depositoAGuardar = depositosServices.Modificardeposito(depositoAGuardar);

            if (depositoAGuardar.Error == false)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = depositoAGuardar.Mensaje;
                return View(depositoAGuardar);
            }
        }

        public IActionResult Delete(int Id)
        {
            string mensaje = depositosServices.EliminarDeposito(Id);

            if (mensaje == "Deposito Eliminado de la Base de Dato")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = mensaje;
                return View();
            }
        }

        public IActionResult Stock(DepositoDTO depositoAVerificar)
        {
            StockServices stockServices = new StockServices();
            List<Stock> stocks = stockServices.ObtenerTodosStocks();
            List<StockDTO> stockDTO = stocks.Select(item => new StockDTO { Id = item.Id, IdArticulo = item.IdArticulo, IdDeposito = item.IdDeposito, Cantidad = item.Cantidad }).ToList();

            int idParaFiltrar = depositoAVerificar.Id;
            stockDTO = stockDTO.Where(obj => obj.IdDeposito == idParaFiltrar).ToList();

            foreach (var stock in stockDTO)
            {
                var obj = stockServices.ObtenerStocksConDatos(stock);
            }


            return View(stockDTO);
        }
    }
}

