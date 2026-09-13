using Entities;
using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class PlatilloVM
    {

        public int IdPlatillo { get; set; }

        [Required(ErrorMessage = "El Nombre del platillo es obligatorio")]
        [StringLength(150, ErrorMessage = "El Nombre del platillo no debe sobrepasar los 150 caracteres")]
        public string NombrePlatillo { get; set; }

        public IFormFile Fotografia { get; set; }

        [Required(ErrorMessage = "El Precio es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Precio no valido")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La Categoria es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria no valida")]
        public int? IdCategoria { get; set; }

        public CategoriaVM? categoria { get; set; }

        public string FotoActual { get; set; }

    }
}
