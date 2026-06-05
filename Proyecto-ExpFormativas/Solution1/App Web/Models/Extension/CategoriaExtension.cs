using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;
using System.Runtime.CompilerServices;

namespace App_Web.Models.Extension
{
    public static class CategoriaExtension
    {

        public static CategoriaVM ToViewModel(this Categoria categoria)
        {
            return new CategoriaVM()
            {
                IdCategoria = categoria.IdCategoria,
                NombreCategoria = categoria.NombreCategoria,
                Descripcion = categoria.Descripcion == null ? string.Empty : categoria.Descripcion
            };
        }

        public static Categoria ToEntity(this CategoriaVM model)
        {
            return new Categoria()
            {
                IdCategoria = model.IdCategoria,
                NombreCategoria = model.NombreCategoria,
                Descripcion = model.Descripcion == null ? string.Empty : model.Descripcion
            };
        }

    }
}
