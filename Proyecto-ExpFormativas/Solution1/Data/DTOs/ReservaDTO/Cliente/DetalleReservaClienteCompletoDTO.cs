using Data.DTOs.ReservaDTO.Reserva;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs.ReservaDTO.Cliente
{
    public class DetalleReservaClienteCompletoDTO
    {

        public DetalleReservaClienteDTO Encabezado { get; set; }
        public List<ReservaMesaDTO> Mesas { get; set; }

    }
}
