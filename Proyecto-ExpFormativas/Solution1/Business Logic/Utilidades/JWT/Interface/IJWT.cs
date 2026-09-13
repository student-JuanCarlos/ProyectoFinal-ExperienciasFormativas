using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Utilidades.JWT.Interface
{
    public interface IJWT
    {

        public string GenerarJWTUsuario(Usuario usuario);

    }
}
