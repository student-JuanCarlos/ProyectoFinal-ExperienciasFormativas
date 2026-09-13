using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class ClienteVM
    {
        public int IdCliente { get; set; }

        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100, ErrorMessage = "El nombre  no debe sobrepasar los 150 caracteres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(200, ErrorMessage = "El apellido no debe sobrepasar los 150 caracteres")]
        public string Apellidos { get; set; }

        public IFormFile Fotografia { get; set; }

        [Required(ErrorMessage = "El Documento es obligatorio")]
        [StringLength(12, ErrorMessage = "El Documento no debe sobrepasar los 12 caracteres")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "El Telefono es obligatorio")]
        [StringLength(11, ErrorMessage = "El Telefono no debe sobrepasar los 11 caracteres")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio")]
        [EmailAddress(ErrorMessage = "El Email no tiene el formato valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña es obligatoria")]
        [MinLength(10, ErrorMessage = "La contraseña debe tener minimo 10 caracteres")]
        public string Contraseña { get; set; }

        public string FotoActual { get; set; }

    }
}
