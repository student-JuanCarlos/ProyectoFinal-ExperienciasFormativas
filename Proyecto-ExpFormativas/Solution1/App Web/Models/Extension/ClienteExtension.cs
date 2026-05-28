using App_Web.Models.VM;
using Data.Infraestructure;
using Entities;

namespace App_Web.Models.Extension
{
    public static class ClienteExtension
    {

        public static ClienteVM ToViewModel(this Cliente cliente)
        {
            return new ClienteVM()
            {
                IdCliente = cliente.IdCliente,
                NombreCompleto = cliente.NombreCompleto,
                Nombres = cliente.Nombres,
                Apellidos = cliente.Apellidos,
                FotoActual = cliente.Fotografia,
                Documento = cliente.Documento,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Contraseña = string.Empty
            };
        }

        public static Cliente ToEntity(this ClienteVM model)
        {
            return new Cliente()
            {
                IdCliente = model.IdCliente,
                NombreCompleto = model.NombreCompleto,
                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                Fotografia = model.FotoActual,
                Documento = model.Documento,
                Telefono = model.Telefono,
                Email = model.Email,
                Contraseña = model.Contraseña
            };
        }

    }
}
