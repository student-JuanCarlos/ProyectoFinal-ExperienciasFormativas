using Entities;

namespace App_Web.Models.VM
{
    public class ReservaVM
    {

        public int IdReserva { get; set; }

        public int? IdCliente { get; set; }

        public string TipoReserva { get; set; }

        public string NombreCliente { get; set; }

        public string TelefonoCliente { get; set; }

        public DateTime? FechaReserva { get; set; }

        public TimeSpan? HoraReserva { get; set; }

        public int CantidadPersonas { get; set; }

        public decimal CostoTotal { get; set; }

        public int Estado { get; set; }

        public int? IdUsuario { get; set; }

        public UsuarioVM usuario { get; set; }

        public ClienteVM cliente { get; set; }

        public List<DetalleReservaVM> DetalleMesa { get; set; }

    }
}
