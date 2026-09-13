using Entities;
using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class DetalleDescuentoVM
    {

        public int IdDetalleDescuento { get; set; }

        [Required(ErrorMessage = "Debe haber una venta enlazada")]
        [Range(1, int.MaxValue, ErrorMessage = "Venta no valida")]
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "Debe haber un descuento enlazado")]
        [Range(1, int.MaxValue, ErrorMessage = "Descuento no valida")]
        public int IdDescuento { get; set; }

        public decimal DescuentoUnitario { get; set; }

        public VentaVM venta { get; set; }

        public DescuentoVM descuento { get; set; }

    }
}
