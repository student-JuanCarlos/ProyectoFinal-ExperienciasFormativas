using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class TipoPlan
    {

        public int IdTipoPlan { get; set; }

        public string NombrePlan { get; set; }

        public decimal PrecioMensual { get; set; }

        public bool Estado {  get; set; }

    }
}
