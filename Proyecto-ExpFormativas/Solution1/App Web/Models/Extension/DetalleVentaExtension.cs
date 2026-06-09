using App_Web.Models.VM;
using Entities;

namespace App_Web.Models.Extension
{
    public static class DetalleVentaExtension
    {

        public static DetalleVentaVM ToViewModel(this DetalleVenta dt)
        {
            return new DetalleVentaVM()
            {
                IdDetalleVenta = dt.IdDetalleVenta,
                IdVenta = dt.IdVenta,
                IdPlatillo = dt.IdPlatillo,
                Cantidad = dt.Cantidad,
                PrecioUnitario = dt.PrecioUnitario,
                SubTotal = dt.SubTotal,
                platillo = dt.platillo != null ? new PlatilloVM()
                {
                    NombrePlatillo = dt.platillo.NombrePlatillo
                } : null
            };
        }

        public static DetalleVenta ToEntity(this DetalleVentaVM model)
        {
            return new DetalleVenta()
            {
                IdDetalleVenta = model.IdDetalleVenta,
                IdVenta = model.IdVenta,
                IdPlatillo = model.IdPlatillo,
                Cantidad = model.Cantidad,
                PrecioUnitario = model.PrecioUnitario,
                SubTotal = model.SubTotal
            };
        }

    }
}
