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

        public List<Reserva> Listado(string Busqueda, int? Estado);

        public Reserva Detalle(int id);

        public int ActualizarMesas(int IdReserva, DataTable mesas);

        public int CancelarReserva(int IdReserva, DataTable mesas);

        public int ActualizarReserva(Reserva r);

        public List<Reserva> ListadoReserva_Cliente(int IdCliente);

        public Reserva DetalleReserva_Cliente(int id);

        public int ActualizarReserva_Cliente(Reserva r);

    }
}
