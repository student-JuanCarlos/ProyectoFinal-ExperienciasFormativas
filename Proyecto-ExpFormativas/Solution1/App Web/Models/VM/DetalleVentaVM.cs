using Entities;

namespace App_Web.Models.VM
{
    public class DetalleVentaVM
    {

        public int IdDetalleVenta { get; set; }

        public int IdVenta { get; set; }

        public int IdPlatillo { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public VentaVM venta { get; set; }

        public PlatilloVM platillo { get; set; }

    }
}
