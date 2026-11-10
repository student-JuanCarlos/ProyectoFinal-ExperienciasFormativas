using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class ConfiguracionReserva
    {

        public int IdConfiguracion {  get; set; }

        public decimal PrecioReserva { get; set; }

        public int IdRestaurante { get; set; }

        public Restaurante restaurante { get; set; }

    }
}
