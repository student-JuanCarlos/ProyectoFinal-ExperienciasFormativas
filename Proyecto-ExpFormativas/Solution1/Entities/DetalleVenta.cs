using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class DetalleVenta
    {

        public int IdDetalleVenta { get; set; }

        public int IdVenta { get; set; }

        public int IdPlatillo { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public Venta venta {  get; set; }

        public Platillo platillo { get; set; }

    }
}
