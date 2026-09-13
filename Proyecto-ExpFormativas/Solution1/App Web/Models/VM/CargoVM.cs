using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class CargoVM
    {

        public int IdCargo { get; set; }

        [Required(ErrorMessage = "El Nombre del Cargo es obligatorio")]
        [StringLength(150, ErrorMessage = "EL Nombre del Cargo no debe sobrepasar los 150 caracteres")]
        public string NombreCargo { get; set; }

        public string? Descripcion { get; set; }

    }
}
