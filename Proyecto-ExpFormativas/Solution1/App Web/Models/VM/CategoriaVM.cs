using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class CategoriaVM
    {

        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El Nombre de la Categoria es obligatoria")]
        [StringLength(150, ErrorMessage = "EL Nombre de la Categoria no debe sobrepasar los 150 caracteres")]
        public string NombreCategoria { get; set; }

        public string? Descripcion { get; set; }


    }
}
