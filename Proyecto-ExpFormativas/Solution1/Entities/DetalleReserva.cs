using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class DetalleReserva
    {
        public int IdDetalleReserva { get; set; }

        public int IdReserva { get; set; }

        public Reserva reserva { get; set; }

        public int IdMesa { get; set; }

        public Mesa mesa { get; set; }

        public int IdRestaurante { get; set; }

        public Restaurante restaurante { get; set; }

    }
}
