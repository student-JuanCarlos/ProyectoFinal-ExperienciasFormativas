using Entities;

namespace App_Web.Models.VM
{
    public class DetalleDescuentoVM
    {

        public int IdDetalleDescuento { get; set; }

        public int IdVenta { get; set; }

        public int IdDescuento { get; set; }

        public decimal DescuentoUnitario { get; set; }

        public VentaVM venta { get; set; }

        public DescuentoVM descuento { get; set; }

    }
}
