using Codigo_Comun.Entity;
using Codigo_Comun.Modelos;
using Codigo_Comun.Modelos.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAppStock.ViewModels
{
    public class StockViewModel
    {
        public StockDTO StockDTO { get; set; }

        public List<Articulo> Articulos { get; set; }
        public List<Deposito> Depositos { get; set; }

        public SelectList selectArticuloList { get; set; }
        public SelectList selectDepositoList { get; set; }
    }
}
