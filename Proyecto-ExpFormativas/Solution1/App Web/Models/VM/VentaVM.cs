using Entities;

namespace App_Web.Models.VM
{
    public class VentaVM
    {

        public int IdVenta { get; set; }

        public int? IdCliente { get; set; }

        public int IdReserva { get; set; }

        public int IdUsuario { get; set; }

        public string? NombreCliente { get; set; }

        public DateTime FechaVenta { get; set; }

        public string MetodoPago { get; set; }

        public decimal Total { get; set; }

        public ClienteVM cliente { get; set; }

        public ReservaVM reserva { get; set; }

        public UsuarioVM usuario { get; set; }

        public List<DetalleVentaVM> detalles { get; set; }

        public List<DetalleDescuentoVM> descuentos { get; set; }

    }
}
