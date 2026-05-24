using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Platillo
    {

        public int IdPlatillo { get; set; }

        public string NombrePlatillo { get; set; }

        public string Fotografia { get; set; }

        public decimal Precio { get; set; }

        public int IdCategoria { get; set; }

        public Categoria categoria { get; set; }

    }
}
