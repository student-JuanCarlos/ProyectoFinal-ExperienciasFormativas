using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class RolVM
    {

        public int IdRol { get; set; }

        [Required(ErrorMessage = "El Nombre del Rol es necesario")]
        public string NombreRol { get; set; }

        public string? Descripcion { get; set; }

    }
}
