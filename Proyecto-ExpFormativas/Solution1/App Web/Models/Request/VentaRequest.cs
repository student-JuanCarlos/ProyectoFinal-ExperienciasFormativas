using App_Web.Models.VM;

namespace App_Web.Models.Request
{
    public class VentaRequest
    {

        public VentaVM venta {  get; set; }

        public List<DetalleVentaVM> detalleVenta { get; set; }

        public List<DetalleDescuentoVM> detalleDescuento { get; set; }

    }
}

