using App_Web.Models.VM;
using Entities;
using Microsoft.IdentityModel.Tokens;

namespace App_Web.Models.Extension
{
    public static class ConfiguracionReservaExtension
    {

        public static ConfiguracionReservaVM ToViewModel(this ConfiguracionReserva confi)
        {
            return new ConfiguracionReservaVM()
            {
                IdConfiguracion = confi.IdConfiguracion,
                PrecioReserva = confi.PrecioReserva,
            };
        }

        public static ConfiguracionReserva ToEntity(this ConfiguracionReservaVM model)
        {
            return new ConfiguracionReserva()
            {
                IdConfiguracion = model.IdConfiguracion,
                PrecioReserva = model.PrecioReserva,
            };
        }

    }
}
