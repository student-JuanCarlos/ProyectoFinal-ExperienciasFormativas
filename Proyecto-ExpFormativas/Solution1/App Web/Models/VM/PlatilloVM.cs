using Entities;

namespace App_Web.Models.VM
{
    public class PlatilloVM
    {

        public int IdPlatillo { get; set; }

        public string NombrePlatillo { get; set; }

        public IFormFile Fotografia { get; set; }

        public decimal Precio { get; set; }

        public int IdCategoria { get; set; }

        public CategoriaVM categoria { get; set; }

        public string FotoActual { get; set; }

    }
}
