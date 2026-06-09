using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;

namespace App_Web.Models.Extension
{
    public static class UsuarioExtension
    {

        public static UsuarioVM ToViewModel(this Usuario usuario)
        {
            return new UsuarioVM()
            {
                IdUsuario = usuario.IdUsuario,
                IdCargo = usuario.IdCargo,
                IdRol = usuario.IdRol,
                NombreUsuario = usuario.NombreUsuario,
                Documento = usuario.Documento,
                Telefono = usuario.Telefono,
                FechaRegistro = usuario.FechaRegistro,
                Email = usuario.Email,
                Contraseña = string.Empty,
                Sueldo = usuario.Sueldo,
                Estado = usuario.Estado,
                cargo = usuario.cargo != null ? new CargoVM()
                {
                    NombreCargo = usuario.cargo.NombreCargo
                } : null,
                rol = usuario.rol != null ? new RolVM()
                {
                    NombreRol = usuario.rol.NombreRol
                } : null
            };
        }

        public static Usuario ToEntity(this UsuarioVM model)
        {
            return new Usuario()
            {
                IdUsuario = model.IdUsuario,
                IdCargo = model.IdCargo,
                IdRol = model.IdRol,
                NombreUsuario = model.NombreUsuario,
                Documento = model.Documento,
                Telefono = model.Telefono,
                FechaRegistro = model.FechaRegistro,
                Email = model.Email,
                Contraseña = model.Contraseña,
                Sueldo = model.Sueldo,
                Estado = model.Estado
            };
        }

    }
}
