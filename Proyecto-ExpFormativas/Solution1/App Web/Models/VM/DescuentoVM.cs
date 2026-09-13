using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class DescuentoVM
    {

            public int IdDescuento { get; set; }

            [Required(ErrorMessage = "El Nombre del descuento es obligatorio")]
            [StringLength(200, ErrorMessage = "El nombre no debe sobrepasar 200 caracteres")]
            public string NombreDescuento { get; set; }

            [Required(ErrorMessage = "El Tipo de Descuento es obligatorio")]
            public string TipoDescuento { get; set; }

            public decimal PorcentajeDescuento { get; set; }

            [Required(ErrorMessage = "La Fecha de Inicio es obligatoria")]
            public DateTime? FechaInicio { get; set; }

            [Required(ErrorMessage = "La Fecha de Fin es obligatoria")]
            public DateTime? FechaFin { get; set; }

            [Required(ErrorMessage = "El Color es obligatorio")]
            public string ColorCard { get; set; }

            public bool Estado { get; set; } = true;

    }
}
