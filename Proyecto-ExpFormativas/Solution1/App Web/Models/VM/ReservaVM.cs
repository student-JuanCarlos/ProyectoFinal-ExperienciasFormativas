using Entities;
using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class ReservaVM
    {

        public int IdReserva { get; set; }

        public int? IdCliente { get; set; }

        [Required(ErrorMessage = "el Tipo de Reserva es obligatorio")]
        public string TipoReserva { get; set; }

        public string NombreCliente { get; set; }

        public string TelefonoCliente { get; set; }

        [Required(ErrorMessage = "La Fecha es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaReserva { get; set; }

        [Required(ErrorMessage = "La Hora es obligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan? HoraReserva { get; set; }

        [Required(ErrorMessage = "La Cantidad de Personas es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Ingrese un Numero valido")]
        public int CantidadPersonas { get; set; }

        public decimal CostoTotal { get; set; }

        public int Estado { get; set; } = 1;

        public int? IdUsuario { get; set; }

        public UsuarioVM usuario { get; set; }

        public ClienteVM cliente { get; set; }

        [Required(ErrorMessage = "Debe haber mesas seleccionadas")]
        public List<DetalleReservaVM> DetalleMesa { get; set; }

    }
}
