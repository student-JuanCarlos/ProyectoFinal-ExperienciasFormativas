using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Infraestructure
{
    public interface IReserva
    {

        public int InsertarReserva(Reserva r, List<DetalleReserva> d);

        public List<Reserva> Listado(string Busqueda);

        public Reserva Detalle(int id);

        public int ActualizarMesas(int IdReserva, DataTable mesas);

        public int CancelarReserva(int IdReserva, DataTable mesas);

        public int ActualizarReserva(Reserva r);

    }
}
