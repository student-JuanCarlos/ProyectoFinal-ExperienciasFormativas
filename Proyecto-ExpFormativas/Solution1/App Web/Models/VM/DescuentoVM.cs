namespace App_Web.Models.VM
{
    public class DescuentoVM
    {

            public int IdDescuento { get; set; }

            public string NombreDescuento { get; set; }

            public string TipoDescuento { get; set; }

            public decimal PorcentajeDescuento { get; set; }

            public DateTime? FechaInicio { get; set; }

            public DateTime? FechaFin { get; set; }

            public string ColorCard { get; set; }

            public bool Estado { get; set; }

    }
}
