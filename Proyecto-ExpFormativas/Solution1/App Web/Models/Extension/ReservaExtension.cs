using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;

namespace App_Web.Models.Extension
{
    public static class ReservaExtension
    {

        public static ReservaVM ToViewModel(this Reserva reserva)
        {
            return new ReservaVM()
            {
                IdReserva = reserva.IdReserva,
                IdCliente = reserva.IdCliente,
                IdUsuario = reserva.IdUsuario,
                TipoReserva = reserva.TipoReserva,
                FechaReserva = reserva.FechaReserva,
                HoraReserva = reserva.HoraReserva,
                CantidadPersonas = reserva.CantidadPersonas,
                CostoTotal = reserva.CostoTotal,
                Estado = reserva.Estado,
                cliente = reserva.cliente != null ? new ClienteVM()
                {
                    Nombres = reserva.cliente.Nombres
                } : null,
                usuario = reserva.usuario != null ? new UsuarioVM()
                {
                    NombreUsuario = reserva.usuario.NombreUsuario
                } : null,
                DetalleMesa = reserva.DetalleMesa?.Select(dr => new DetalleReservaVM()
                {
                    mesa = new MesaVM()
                    {
                        NumeroMesa = dr.mesa.NumeroMesa,
                    }
                }).ToList()
            };
        }

        public static Reserva ToEntity(this ReservaVM model)
        {
            return new Reserva()
            {
                IdReserva = model.IdReserva,
                IdCliente = model.IdCliente,
                IdUsuario = model.IdUsuario,
                TipoReserva = model.TipoReserva,
                FechaReserva = model.FechaReserva,
                HoraReserva = model.HoraReserva,
                CantidadPersonas = model.CantidadPersonas,
                CostoTotal = model.CostoTotal,
                Estado = model.Estado
            };
        }

    }
}
