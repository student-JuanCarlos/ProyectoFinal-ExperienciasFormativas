using App_Web.Models.VM;

namespace App_Web.Models.Request
{
    public class ReservaRequest
    {

        public ReservaVM Reserva { get; set; }

        public List<DetalleReservaVM> DetalleReserva { get; set; }

    }
}
