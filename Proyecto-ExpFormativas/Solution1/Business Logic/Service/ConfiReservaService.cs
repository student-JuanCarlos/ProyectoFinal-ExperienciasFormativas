using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class ConfiReservaService
    {
        private readonly IConfigurationReserva confiDB;

        public ConfiReservaService(IConfigurationReserva service)
        {
            confiDB = service;
        }

        public int ActualizarPrecioReserva(decimal precio)
        {
            return confiDB.ActualizarPrecio(precio);
        }

        public ConfiguracionReserva DetallePrecioReserva()
        {
            return confiDB.DetallePrecioReserva();
        }
    }
}
