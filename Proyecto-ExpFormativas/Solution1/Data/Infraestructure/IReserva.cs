using Data.DTOs.ReservaDTO.Cliente;
using Data.DTOs.ReservaDTO.Reserva;
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

        public ReservaDetalleCompletoDTO Detalle(int id);

        public int ActualizarMesas(int IdReserva, List<int> IdMesas);

        public int CancelarReserva(int IdReserva, List<int> IdMesas);

        public int ActualizarReserva(Reserva r);

        public List<Reserva> ListadoReserva_Cliente(int IdCliente);

        public DetalleReservaClienteCompletoDTO DetalleReservaCliente(int id);

        public int ActualizarReserva_Cliente(Reserva r);

    }
}
