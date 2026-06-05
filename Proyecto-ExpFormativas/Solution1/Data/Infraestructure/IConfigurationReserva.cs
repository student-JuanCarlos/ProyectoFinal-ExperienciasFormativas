using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IConfigurationReserva
    {

        public int ActualizarPrecio(decimal precio);

        public ConfiguracionReserva DetallePrecioReserva();

    }
}
