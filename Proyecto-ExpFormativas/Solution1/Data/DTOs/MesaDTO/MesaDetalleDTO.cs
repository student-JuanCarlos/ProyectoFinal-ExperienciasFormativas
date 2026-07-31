using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs.MesaDTO
{
    public class MesaDetalleDTO
    {

        public int IdMesa { get; set; }

        public int NumeroMesa { get; set; }

        public int EspacioOcupable { get; set; }

        public int Estado { get; set; }

        public TimeSpan? HoraReserva { get; set; }

        public string? OcupadoPor { get; set; }
    }
}
