using Entities;
using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class DetalleReservaVM
    {

        public int IdDetalleReserva { get; set; }

        [Required(ErrorMessage = "Debe haber una venta enlazada")]
        [Range(1, int.MaxValue, ErrorMessage = "Venta no valida")]
        public int IdReserva { get; set; }

        [Required(ErrorMessage = "Debe haber una mesa enlazada")]
        [Range(1, int.MaxValue, ErrorMessage = "Mesa no valida")]
        public int IdMesa { get; set; }

        public ReservaVM reserva { get; set; }

        public MesaVM mesa { get; set; }

    }
}
