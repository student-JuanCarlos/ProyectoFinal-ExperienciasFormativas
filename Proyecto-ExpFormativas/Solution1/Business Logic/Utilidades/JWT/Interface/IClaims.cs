using Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Business_Logic.Utilidades.JWT.Interface
{
    public interface IClaims
    {

        public List<Claim> ClaimsUsuario(Usuario usuario);

    }
}
