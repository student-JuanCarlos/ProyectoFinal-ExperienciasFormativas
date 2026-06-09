using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;

namespace App_Web.Models.Extension
{
    public static class RolExtension
    {

        public static RolVM ToViewModel(this Rol rol)
        {
            return new RolVM()
            {
                IdRol = rol.IdRol,
                NombreRol = rol.NombreRol,
                Descripcion = rol.Descripcion == null ? null : rol.Descripcion
            };
        }

        public static Rol ToEntity(this RolVM model)
        {
            return new Rol()
            {
                IdRol = model.IdRol,
                NombreRol = model.NombreRol,
                Descripcion = model.Descripcion == null ? null : model.Descripcion
            };
        }

    }
}
