using Entities;

namespace App_Web.Models.VM
{
    public class DetalleReservaVM
    {

        public int IdDetalleReserva { get; set; }

        public int IdReserva { get; set; }

        public int IdMesa { get; set; }

        public ReservaVM reserva { get; set; }

        public MesaVM mesa { get; set; }

    }
}
