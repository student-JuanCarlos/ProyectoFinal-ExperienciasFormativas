using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IMesa : IGeneric<Mesa>
    {
        public List<Mesa> Listado(string Busqueda);

        public void ActualizarEstadoMesasHoy();

        public List<Mesa> FiltradoMesas_Cliente(DateTime FechaReserva, TimeSpan HoraReserva);
    }
}
