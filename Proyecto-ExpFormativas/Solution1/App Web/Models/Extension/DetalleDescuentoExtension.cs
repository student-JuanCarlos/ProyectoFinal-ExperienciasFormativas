using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;

namespace App_Web.Models.Extension
{
    public static class DetalleDescuentoExtension
    {

        public static DetalleDescuentoVM ToViewModel(this DetalleDescuento dd)
        {
            return new DetalleDescuentoVM()
            {
                IdDetalleDescuento = dd.IdDetalleDescuento,
                IdDescuento = dd.IdDescuento,
                IdVenta = dd.IdVenta,
                DescuentoUnitario = dd.DescuentoUnitario,
                descuento = dd.descuento != null ? new DescuentoVM()
                {
                    NombreDescuento = dd.descuento.NombreDescuento,
                    ColorCard = dd.descuento.ColorCard,
                } : null
            };
        }

        public static DetalleDescuento ToEntity(this DetalleDescuentoVM model)
        {
            return new DetalleDescuento()
            {
                IdDetalleDescuento = model.IdDetalleDescuento,
                IdDescuento = model.IdDescuento,
                IdVenta = model.IdVenta,
                DescuentoUnitario = model.DescuentoUnitario
            };
        }
    }
}
