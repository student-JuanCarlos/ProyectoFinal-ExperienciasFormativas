using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using BCrypt;

namespace Business_Logic.Service
{
    public class UsuarioService
    {
        private readonly IUsuario usuarioDB;

        public UsuarioService(IUsuario service)
        {
            usuarioDB = service;
        }

        public int GestionarUsuario(Usuario u)
        {
            if (u.IdUsuario == 0)
            {
                u.Contraseña = BCrypt.Net.BCrypt.HashPassword(u.Contraseña);
                return usuarioDB.Agregar(u);
            }
            else
                return usuarioDB.Actualizar(u);
        }

        public List<Usuario> ListadoUsuario(string Busqueda, bool? Estado)
        {
            return usuarioDB.Listado(Busqueda, Estado);
        }

        public Usuario Detalle(int id)
        {
            return usuarioDB.Detalle(id);
        }

        public Usuario Login(string Email, string Contraseña)
        {
            var usuario = usuarioDB.Login(Email, Contraseña); // se ignora contraseña de igual manera, no influye

            if (usuario == null)
                return null;

            bool DatosValidos = BCrypt.Net.BCrypt.Verify(Contraseña, usuario.Contraseña);

            return DatosValidos ? usuario : null;
        }

        public int CambiarEstado(int id)
        {
            return usuarioDB.CambiarEstado(id);
        }

    }
}
