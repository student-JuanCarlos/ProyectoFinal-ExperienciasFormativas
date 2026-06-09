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
                IdCliente = venta.IdCliente,
                IdReserva = venta.IdReserva,
                IdUsuario = venta.IdUsuario,
                NombreCliente = venta.NombreCliente == null ? null : venta.NombreCliente,
                FechaVenta = venta.FechaVenta,
                MetodoPago = venta.MetodoPago,
                Total = venta.Total,
                cliente = venta.cliente != null ? new ClienteVM()
                {
                    IdCliente = venta.cliente.IdCliente,
                    Nombres = venta.cliente.Nombres,
                    Apellidos = venta.cliente.Apellidos,
                    Email = venta.cliente.Email
                } : null,
                reserva = venta.reserva != null ? new ReservaVM()
                {
                    TipoReserva = venta.reserva.TipoReserva,
                    CantidadPersonas = venta.reserva.CantidadPersonas,
                    CostoTotal = venta.reserva.CostoTotal
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
                IdCliente = model.IdCliente,
                IdReserva = model.IdReserva,
                IdUsuario = model.IdUsuario,
                NombreCliente = model.NombreCliente == null ? null : model.NombreCliente,
                FechaVenta = model.FechaVenta,
                MetodoPago = model.MetodoPago,
                Total = model.Total
            };
        }

    }
}
