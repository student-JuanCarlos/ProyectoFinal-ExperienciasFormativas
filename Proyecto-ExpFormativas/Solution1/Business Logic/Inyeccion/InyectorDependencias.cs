using Business_Logic.Service;
using Data.Infraestructure;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Inyeccion
{
    public static class InyectorDependencias
    {

        public static void Inyeccion(this IServiceCollection services)
        {
            services.AddScoped<ICargo, CargoRepository>();
            services.AddScoped<CargoService>();
        }

    }
}
