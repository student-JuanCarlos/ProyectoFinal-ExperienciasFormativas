using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IPasarelaPago
    {

        public string CrearCheckOut(OrdenPago orden, TipoPlan plan);

    }
}
