using Entities;
using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class DetalleVentaVM
    {

        public int IdDetalleVenta { get; set; }

        [Required(ErrorMessage = "Debe haber una venta enlazada")]
        [Range(1, int.MaxValue, ErrorMessage = "Venta no valida")]
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un platillo")]
        [Range(1, int.MaxValue, ErrorMessage = "Platillo no valido")]
        public int IdPlatillo { get; set; }

        [Required(ErrorMessage = "Debe haber una Cantidad del platillo especificada")]
        [Range(1, int.MaxValue, ErrorMessage = "Numero no valido")]
        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public VentaVM venta { get; set; }

        public PlatilloVM platillo { get; set; }

    }
}
