using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs.ReservaDTO.Cliente
{
    public class DetalleReservaClienteDTO
    {
        public int IdReserva { get; set; }
        public DateTime? FechaReserva { get; set; }
        public TimeSpan? HoraReserva { get; set; }
        public int CantidadPersonas { get; set; }
        public decimal CostoTotal { get; set; }
        public int Estado { get; set; }

    }
}
