using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs.ReservaDTO.Reserva
{
    public class ReservaEncabezadoDTO
    {
        public int IdReserva { get; set; }
        public string GeneradoPor { get; set; }
        public string? NombreCliente { get; set; }
        public string? TelefonoCliente { get; set; }
        public string TipoReserva { get; set; }
        public DateTime? FechaReserva { get; set; }
        public TimeSpan? HoraReserva { get; set; }
        public int CantidadPersonas { get; set; }
        public decimal CostoTotal { get; set; }
        public int Estado { get; set; }

    }
}
