using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
                return usuarioDB.Agregar(u);
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
            return usuarioDB.Login(Email, Contraseña);
        }

        public int CambiarEstado(int id)
        {
            return usuarioDB.CambiarEstado(id);
        }

    }
}
