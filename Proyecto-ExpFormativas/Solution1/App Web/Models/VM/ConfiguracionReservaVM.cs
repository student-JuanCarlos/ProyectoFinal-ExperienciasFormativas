using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class ConfiguracionReservaVM
    {
        public int IdConfiguracion { get; set; }

        [Required(ErrorMessage = "El precio de la Reserva es obligatoria")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Precio no valido")]
        public decimal PrecioReserva { get; set; }

    }
}
