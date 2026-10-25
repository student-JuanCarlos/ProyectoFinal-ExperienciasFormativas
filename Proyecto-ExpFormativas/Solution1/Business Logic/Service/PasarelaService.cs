using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class PasarelaService : IPasarelaPago
    {
        public string CrearCheckOut(OrdenPago orden, TipoPlan plan) => "/Suscripcion/Resultado";
    }
}
