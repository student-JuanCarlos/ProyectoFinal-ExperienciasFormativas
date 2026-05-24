using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Venta
    {

        public int IdVenta { get; set; }

        public int? IdCliente { get; set; }

        public int IdReserva { get; set; }

        public int IdUsuario { get; set; }

        public string? NombreCliente { get; set; }

        public DateTime FechaVenta { get; set; }

        public String MetodoPago { get; set; }

        public decimal Total {  get; set; }

        public Cliente cliente {  get; set; }

        public Reserva reserva { get; set; }

        public Usuario usuario { get; set; }

        public List<DetalleVenta> detalles { get; set; }

        public List<DetalleDescuento> descuentos { get; set; }

    }
}
