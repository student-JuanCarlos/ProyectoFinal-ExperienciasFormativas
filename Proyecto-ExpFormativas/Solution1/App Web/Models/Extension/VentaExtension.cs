using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;

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
