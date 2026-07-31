using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs.ReservaDTO.Reserva
{
    public class ReservaClienteDTO
    {
        public int IdCliente { get; set; }
        public string NombreCompleto { get; set; }
        public string? Fotografia { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Documento { get; set; }

    }
}
