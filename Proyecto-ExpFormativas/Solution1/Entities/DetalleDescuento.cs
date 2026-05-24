using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class DetalleDescuento
    {

        public int IdDetalleDescuento {  get; set; }

        public int IdVenta {  get; set; }

        public int IdDescuento { get; set; }

        public decimal DescuentoUnitario { get; set; }

        public Venta venta { get; set; }

        public Descuento descuento { get; set; }

    }
}
