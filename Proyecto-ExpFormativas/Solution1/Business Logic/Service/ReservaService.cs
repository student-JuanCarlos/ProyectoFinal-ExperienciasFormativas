using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Business_Logic.Service
{
    public class ReservaService
    {
        private readonly IReserva reservaDB;

        public ReservaService(IReserva service)
        {
            reservaDB = service;
        }

        public int InsertarReserva(Reserva r, List<DetalleReserva> d)
        {
            return reservaDB.InsertarReserva(r, d);
        }

        public int ActualizarReserva(Reserva r)
        {
            return reservaDB.ActualizarReserva(r);
        }

        public List<Reserva> ListadoReserva(string Busqueda, int? Estado)
        {
            return reservaDB.Listado(Busqueda, Estado);
        }

        public Reserva Detalle(int id)
        {
            return reservaDB.Detalle(id);
        }

        public int CancelarReserva(int IdReserva, DataTable mesas)
        {
            return reservaDB.CancelarReserva(IdReserva, mesas);
        }

        public int ActualizarMesas(int IdReserva, DataTable mesas)
        {
            return reservaDB.ActualizarMesas(IdReserva, mesas);
        }

    }
}
