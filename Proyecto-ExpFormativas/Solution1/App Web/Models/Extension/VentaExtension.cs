using App_Web.Models.VM;
using Data.DTOs.ReservaDTO.Reserva;
using Data.DTOs.VentaDTO;
using Data.Infraestructure;
using Entities;
using Microsoft.Extensions.Options;

namespace App_Web.Models.Extension
{
    public static class VentaExtension
    {

        public static VentaVM ToViewModel(this Venta venta)
        {
            return new VentaVM()
            {
                IdVenta = venta.IdVenta,
                IdReserva = venta.IdReserva,
                IdUsuario = venta.IdUsuario,
                FechaVenta = venta.FechaVenta,
                MetodoPago = venta.MetodoPago,
                Total = venta.Total,
                reserva = venta.reserva != null ? new ReservaVM()
                {
                    NombreCliente = venta.reserva.NombreCliente,
                    TelefonoCliente = venta.reserva.TelefonoCliente,
                    TipoReserva = venta.reserva.TipoReserva,
                    CantidadPersonas = venta.reserva.CantidadPersonas,
                    CostoTotal = venta.reserva.CostoTotal,
                    cliente = venta.reserva.cliente != null ? new ClienteVM()
                    {
                        IdCliente = venta.reserva.cliente.IdCliente,
                        NombreCompleto = venta.reserva.NombreCliente,
                        Nombres = venta.reserva.cliente.Nombres,
                        Apellidos = venta.reserva.cliente.Apellidos,
                        Email = venta.reserva.cliente.Email
                    } : null,
                } : null,
                usuario = venta.usuario != null ? new UsuarioVM()
                {
                    NombreUsuario = venta.usuario.NombreUsuario
                } : null,
                detalles = venta.detalles?.Select(d => new DetalleVentaVM()
                {
                    platillo = new PlatilloVM()
                    {
                        NombrePlatillo = d.platillo.NombrePlatillo
                    },
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList(),
                descuentos = venta.descuentos?.Select(dd => new DetalleDescuentoVM()
                {
                    descuento = new DescuentoVM()
                    {
                        NombreDescuento = dd.descuento.NombreDescuento,
                        ColorCard = dd.descuento.ColorCard
                    },
                    DescuentoUnitario = dd.DescuentoUnitario
                }).ToList()
            };
        }

        public static VentaVM DetalleDTOtoVM(this VentaDetalleCompletoDTO dto)
        {
            return new VentaVM()
            {
                IdVenta = dto.Encabezado.IdVenta,
                FechaVenta = dto.Encabezado.FechaVenta,
                MetodoPago = dto.Encabezado.MetodoPago,
                Total = dto.Encabezado.Total,
                reserva = dto != null ? new ReservaVM()
                {
                    NombreCliente = dto.Encabezado.NombreCompleto,
                    TipoReserva = dto.Encabezado.TipoReserva,
                    TelefonoCliente = dto.Encabezado.Contacto,
                    CantidadPersonas = dto.Encabezado.CantidadPersonas,
                    CostoTotal = dto.Encabezado.CostoTotal,
                    cliente = dto != null ? new ClienteVM()
                    {
                        NombreCompleto = dto.Encabezado.NombreCompleto,
                        Email = dto.Encabezado.Contacto
                    } : null,
                } : null,
                usuario = dto != null ? new UsuarioVM()
                {
                    NombreUsuario = dto.Encabezado.NombreUsuario
                } : null,
                detalles = dto.Platillos.Select(dv => new DetalleVentaVM()
                {
                    platillo = new PlatilloVM()
                    {
                        NombrePlatillo = dv.NombrePlatillo
                    },
                    Cantidad = dv.Cantidad,
                    PrecioUnitario = dv.PrecioUnitario
                }).ToList(),
                descuentos = dto.Descuentos.Select(dd => new DetalleDescuentoVM()
                {
                    descuento = new DescuentoVM()
                    {
                        NombreDescuento = dd.NombreDescuento,
                        ColorCard = dd.ColorCard
                    },
                    DescuentoUnitario = dd.DescuentoUnitario
                }).ToList()
            };
        }

        public static Venta DetalleEntitytoDTO(this VentaDetalleCompletoDTO dto)
        {
            return new Venta()
            {
                IdVenta = dto.Encabezado.IdVenta,
                FechaVenta = dto.Encabezado.FechaVenta,
                MetodoPago = dto.Encabezado.MetodoPago,
                Total = dto.Encabezado.Total,
                reserva = dto != null ? new Reserva()
                {
                    TipoReserva = dto.Encabezado.TipoReserva,
                    CantidadPersonas = dto.Encabezado.CantidadPersonas,
                    CostoTotal = dto.Encabezado.CostoTotal,
                    NombreCliente = dto.Encabezado.NombreCompleto,
                    TelefonoCliente = dto.Encabezado.Contacto,
                    cliente = new Cliente()
                    {
                        Email = dto.Encabezado.Contacto
                    },
                    usuario = new Usuario()
                    {
                        NombreUsuario = dto.Encabezado.NombreUsuario,
                    }
                } : null,
                detalles = dto.Platillos.Select(dv => new DetalleVenta()
                {
                    platillo = new Platillo()
                    {
                        NombrePlatillo = dv.NombrePlatillo,
                    },
                    Cantidad = dv.Cantidad,
                    PrecioUnitario = dv.PrecioUnitario
                }).ToList(),
                descuentos = dto.Descuentos.Select(dd => new DetalleDescuento()
                {
                    descuento = new Descuento()
                    {
                        NombreDescuento = dd.NombreDescuento,
                        ColorCard = dd.ColorCard,
                    },
                    DescuentoUnitario = dd.DescuentoUnitario
                }).ToList()
            };
        }

        public static Venta ToEntity(this VentaVM model)
        {
            return new Venta()
            {
                IdVenta = model.IdVenta,
                IdReserva = model.IdReserva,
                IdUsuario = model.IdUsuario,
                FechaVenta = model.FechaVenta,
                MetodoPago = model.MetodoPago,
                Total = model.Total
            };
        }

    }
}
