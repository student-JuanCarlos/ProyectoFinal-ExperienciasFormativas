using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs.VentaDTO
{
    public class VentaEncabezadoDTO
    {
        public int IdVenta { get; set; }
        public string NombreCompleto { get; set; }
        public string? TipoReserva { get; set; }
        public string Contacto { get; set; }
        public int CantidadPersonas { get; set; }
        public decimal CostoTotal { get; set; }
        public DateTime FechaVenta { get; set; }
        public string MetodoPago { get; set; }
        public decimal Total { get; set; }
        public string? NombreUsuario { get; set; }

    }
}
