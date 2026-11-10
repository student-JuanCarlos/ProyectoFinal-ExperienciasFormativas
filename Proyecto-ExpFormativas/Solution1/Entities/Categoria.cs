using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Categoria
    {

        public int IdCategoria { get; set; }

        public string NombreCategoria { get; set; }

        public string? Descripcion { get; set; }

        public int IdRestaurante { get; set; }

        public Restaurante restaurante { get; set; }

    }
}
