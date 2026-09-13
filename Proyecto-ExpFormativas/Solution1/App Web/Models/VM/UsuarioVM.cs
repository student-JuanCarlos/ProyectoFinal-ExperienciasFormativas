using Entities;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace App_Web.Models.VM
{
    public class UsuarioVM
    {

        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de Usuario es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre de Usuario no debe sobrepasar los 150 caracteres")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El Documento es obligatorio")]
        [StringLength(15, ErrorMessage = "El Documento no puede tener mas de 15 numeros")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "El Telefono es obligatorio")]
        [Phone(ErrorMessage = "El numero no puede tener mas de 9 numeros")]
        public string Telefono { get; set; }

        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio")]
        [EmailAddress(ErrorMessage = "El Email no tiene formato valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña es obligatoria")]
        [MinLength(10, ErrorMessage = "La contraseña debe tener minimo 10 caracteres")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "El Sueldo es obligatorio")]
        [Range(400, 20000, ErrorMessage = "El Sueldo es muy bajo")]
        public decimal Sueldo { get; set; }

        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "El Cargo es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Cargo no valido")]
        public int IdCargo { get; set; }

        [Required(ErrorMessage = "El Rol es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Rol no valido")]
        public int IdRol { get; set; }

        public RolVM rol { get; set; }

        public CargoVM cargo { get; set; }

    }
}
