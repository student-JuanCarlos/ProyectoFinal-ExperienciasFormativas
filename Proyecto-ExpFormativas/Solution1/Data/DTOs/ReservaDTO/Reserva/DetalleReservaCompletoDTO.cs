using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs.ReservaDTO.Reserva
{
    public class ReservaDetalleCompletoDTO
    {
        public ReservaEncabezadoDTO Encabezado { get; set; }
        public List<ReservaMesaDTO> Mesas { get; set; }
        public ReservaClienteDTO Cliente { get; set; }
    }
}
