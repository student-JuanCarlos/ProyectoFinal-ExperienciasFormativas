using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;

namespace App_Web.Models.Extension
{
    public static class DescuentoExtension
    {

        public static DescuentoVM ToViewModel(this Descuento descuento)
        {
            return new DescuentoVM()
            {
                IdDescuento = descuento.IdDescuento,
                NombreDescuento = descuento.NombreDescuento,
                TipoDescuento = descuento.TipoDescuento,
                PorcentajeDescuento = descuento.PorcentajeDescuento,
                FechaInicio = descuento.FechaInicio == null ? null : descuento.FechaInicio,
                FechaFin = descuento.FechaFin == null ? null : descuento.FechaFin,
                ColorCard = descuento.ColorCard,
                Estado = descuento.Estado,
            };
        }

        public static Descuento ToEntity(this DescuentoVM model)
        {
            return new Descuento()
            {
                IdDescuento = model.IdDescuento,
                NombreDescuento = model.NombreDescuento,
                TipoDescuento = model.TipoDescuento,
                PorcentajeDescuento = model.PorcentajeDescuento,
                FechaInicio = model.FechaInicio == null ? null : model.FechaInicio,
                FechaFin = model.FechaFin == null ? null : model.FechaFin,
                ColorCard = model.ColorCard,
                Estado = model.Estado,
            };
        }

    }
}
