using Business_Logic.Service;
using Data.Infraestructure;
using Data.Repository;
using Entities;
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

            services.AddScoped<ICategoria, CategoriaRepository>();
            services.AddScoped<CategoriaService>();

            services.AddScoped<ICliente, ClienteRepository>();
            services.AddScoped<ClienteService>();

            services.AddScoped<IDescuento, DescuentoRepository>();
            services.AddScoped<DescuentoService>();

            services.AddScoped<IMesa, MesaRepository>();
            services.AddScoped<MesaService>();

            services.AddScoped<IConfigurationReserva, PrecioReservaRepository>();
            services.AddScoped<ConfiReservaService>();

            services.AddScoped<IPlatillo, PlatilloRepository>();
            services.AddScoped<PlatilloService>();

            services.AddScoped<IReserva, ReservaRepository>();
            services.AddScoped<ReservaService>();

            services.AddScoped<IRol, RolRepository>();
            services.AddScoped<RolService>();

            services.AddScoped<IUsuario, UsuarioRepository>();
            services.AddScoped<UsuarioService>();

            services.AddScoped<IVenta, VentaRepository>();
            services.AddScoped<VentaService>();

        }

    }
}
