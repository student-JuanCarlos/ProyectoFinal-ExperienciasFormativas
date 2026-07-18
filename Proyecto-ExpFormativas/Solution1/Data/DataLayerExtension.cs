using Data.Context;
using Data.Infraestructure;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DataLayerExtension
    {

        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("database")));

            services.AddScoped<ICargo, CargoRepository>();

            services.AddScoped<ICategoria, CategoriaRepository>();

            services.AddScoped<ICliente, ClienteRepository>();

            services.AddScoped<IDescuento, DescuentoRepository>();

            services.AddScoped<IMesa, MesaRepository>();

            services.AddScoped<IConfigurationReserva, PrecioReservaRepository>();

            services.AddScoped<IPlatillo, PlatilloRepository>();

            services.AddScoped<IReserva, ReservaRepository>();

            services.AddScoped<IRol, RolRepository>();

            services.AddScoped<IUsuario, UsuarioRepository>();

            services.AddScoped<IVenta, VentaRepository>();

            return services;
        }

    }
}
