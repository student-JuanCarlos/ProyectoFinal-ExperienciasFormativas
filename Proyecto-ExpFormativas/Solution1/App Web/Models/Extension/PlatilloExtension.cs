using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;
using System.Runtime.CompilerServices;

namespace App_Web.Models.Extension
{
    public static class PlatilloExtension
    {

        public static PlatilloVM ToViewModel(this Platillo platillo)
        {
            return new PlatilloVM()
            {
                IdPlatillo = platillo.IdPlatillo,
                NombrePlatillo = platillo.NombrePlatillo,
                FotoActual = platillo.Fotografia,
                Precio = platillo.Precio,
                IdCategoria = platillo.IdCategoria,
                categoria = platillo.categoria != null ? new CategoriaVM()
                {
                    NombreCategoria = platillo.categoria.NombreCategoria
                } : null
            };
        }

        public static Platillo ToEntity(this PlatilloVM model)
        {
            return new Platillo()
            {
                IdPlatillo = model.IdPlatillo,
                NombrePlatillo = model.NombrePlatillo,
                Fotografia = model.FotoActual,
                Precio = model.Precio,
                IdCategoria = model.IdCategoria
            };
        }

    }
}
