using App_Web.Models.VM;
using Entities;

namespace App_Web.Models.Extension
{
    public static class DetalleReservaExtension
    {

        public static DetalleReservaVM ToViewModel(this DetalleReserva dr)
        {
            return new DetalleReservaVM()
            {
                IdDetalleReserva = dr.IdDetalleReserva,
                IdMesa = dr.IdMesa,
                IdReserva = dr.IdReserva,
                mesa = dr.mesa != null ? new MesaVM()
                {
                    IdMesa = dr.mesa.IdMesa,
                    NumeroMesa = dr.mesa.NumeroMesa
                } : null
            };
        }

        public static DetalleReserva ToEntity(this DetalleReservaVM model)
        {
            return new DetalleReserva()
            {
                IdDetalleReserva = model.IdDetalleReserva,
                IdMesa = model.IdMesa,
                IdReserva = model.IdReserva
            };
        }

    }
}
