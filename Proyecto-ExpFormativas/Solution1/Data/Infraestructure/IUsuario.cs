using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    internal interface IUsuario: IGeneric<Usuario>
    {
        public Usuario Login(string Email, String Contraseña);

        public int CambiarEstado(int id);

        public List<Usuario> Listado(string Busqueda, bool estado);
    }
}
