using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;

namespace App_Web.Models.Extension
{
    public static class MesaExtension
    {

        public static MesaVM ToViewModel(this Mesa mesa)
        {
            return new MesaVM()
            {
                IdMesa = mesa.IdMesa,
                NumeroMesa = mesa.NumeroMesa,
                EspacioOcupable = mesa.EspacioOcupable,
                Estado = mesa.Estado,
                OcupadoPor = mesa.OcupadoPor,
                HoraReserva = mesa.HoraReserva,
                FechaReserva = mesa.FechaReserva,
            };
        }

        public static Mesa ToEntity(this MesaVM model)
        {
            return new Mesa()
            {
                IdMesa = model.IdMesa,
                NumeroMesa = model.NumeroMesa,
                EspacioOcupable = model.EspacioOcupable,
                Estado = model.Estado,
            };
        }

    }
}
