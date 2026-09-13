using Entities;
using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class VentaVM
    {

        public int IdVenta { get; set; }

        [Required(ErrorMessage = "Debe haber una reserva enlazada")]
        [Range(1, int.MaxValue, ErrorMessage = "Reserva no valida")]
        public int IdReserva { get; set; }

        [Required(ErrorMessage = "Debe haber un usuario que realizo la Venta")]
        [Range(1, int.MaxValue, ErrorMessage = "Usuario no valido")]
        public int IdUsuario { get; set; }

        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "El Metodo de pago es oligatorio")]
        public string MetodoPago { get; set; }

        public decimal Total { get; set; }

        public ReservaVM reserva { get; set; }

        public UsuarioVM usuario { get; set; }

        [Required(ErrorMessage = "Debe haber platillos seleccionados")]
        public List<DetalleVentaVM> detalles { get; set; }

        public List<DetalleDescuentoVM> descuentos { get; set; }

    }
}
