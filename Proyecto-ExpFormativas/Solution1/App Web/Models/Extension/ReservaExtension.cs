using App_Web.Models.VM;
using Data.DTOs.ReservaDTO.Cliente;
using Data.DTOs.ReservaDTO.Reserva;
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
                NombreCliente = reserva.NombreCliente == null ? null : reserva.NombreCliente,
                TelefonoCliente = reserva.TelefonoCliente == null ? null : reserva.TelefonoCliente,
                FechaReserva = reserva.FechaReserva,
                HoraReserva = reserva.HoraReserva,
                CantidadPersonas = reserva.CantidadPersonas,
                CostoTotal = reserva.CostoTotal,
                Estado = reserva.Estado,
                cliente = reserva.cliente != null ? new ClienteVM()
                {
                    IdCliente = reserva.cliente.IdCliente,
                    Nombres = reserva.cliente.Nombres,
                    FotoActual = reserva.cliente.Fotografia,
                    Telefono = reserva.cliente.Telefono,
                    Email = reserva.cliente.Email,
                    Documento = reserva.cliente.Documento
                } : null,
                usuario = reserva.usuario != null ? new UsuarioVM()
                {
                    NombreUsuario = reserva.usuario.NombreUsuario
                } : null,
                DetalleMesa = reserva.DetalleMesa?.Select(dr => new DetalleReservaVM()
                {
                    mesa = dr.mesa == null ? null : new MesaVM()
                    {
                        IdMesa = dr.mesa.IdMesa,
                        NumeroMesa = dr.mesa.NumeroMesa,
                    }
                }).ToList()
            };
        }

        public static ReservaVM DetalleDTOtoVM(this ReservaDetalleCompletoDTO dto)
        {
            return new ReservaVM()
            {
                IdReserva = dto.Encabezado.IdReserva,
                NombreCliente = dto.Encabezado.NombreCliente == null ? null : dto.Encabezado.NombreCliente,
                TelefonoCliente = dto.Encabezado.TelefonoCliente == null ? null : dto.Encabezado.TelefonoCliente,
                TipoReserva = dto.Encabezado.TipoReserva,
                FechaReserva = dto.Encabezado.FechaReserva,
                HoraReserva = dto.Encabezado.HoraReserva,
                CantidadPersonas = dto.Encabezado.CantidadPersonas,
                CostoTotal = dto.Encabezado.CostoTotal,
                Estado = dto.Encabezado.Estado,
                cliente = dto.Cliente != null ? new ClienteVM()
                {
                    IdCliente = dto.Cliente.IdCliente,
                    NombreCompleto = dto.Encabezado.GeneradoPor,
                    FotoActual = dto.Cliente.Fotografia,
                    Telefono = dto.Cliente.Telefono,
                    Email = dto.Cliente.Email,
                    Documento = dto.Cliente.Documento
                } : null,
                usuario = dto.Cliente != null ? new UsuarioVM()
                {
                    NombreUsuario = dto.Encabezado.GeneradoPor
                } : null,
                DetalleMesa = dto.Mesas.Select(dr => new DetalleReservaVM()
                {
                    mesa = new MesaVM()
                    {
                        IdMesa = dr.IdMesa,
                        NumeroMesa = dr.NumeroMesa
                    }
                }).ToList(),
            };
        }

        public static ReservaVM DetalleClienteDTOtoVM(this DetalleReservaClienteCompletoDTO dto)
        {
            return new ReservaVM()
            {
                IdReserva = dto.Encabezado.IdReserva,
                FechaReserva = dto.Encabezado.FechaReserva,
                HoraReserva = dto.Encabezado.HoraReserva,
                CantidadPersonas = dto.Encabezado.CantidadPersonas,
                CostoTotal = dto.Encabezado.CostoTotal,
                Estado = dto.Encabezado.Estado,
                DetalleMesa = dto.Mesas.Select(dr => new DetalleReservaVM()
                {
                    mesa = new MesaVM()
                    {
                        IdMesa = dr.IdMesa,
                        NumeroMesa = dr.NumeroMesa
                    }
                }).ToList(),
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
                NombreCliente = model.NombreCliente,
                TelefonoCliente = model.TelefonoCliente,
                CantidadPersonas = model.CantidadPersonas,
                CostoTotal = model.CostoTotal,
                Estado = model.Estado
            };
        }

    }
}
