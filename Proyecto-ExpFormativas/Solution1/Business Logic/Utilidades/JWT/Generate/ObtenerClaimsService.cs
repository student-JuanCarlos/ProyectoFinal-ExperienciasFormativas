using Business_Logic.Utilidades.JWT.Interface;
using Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Business_Logic.Utilidades.JWT.Generate
{
    public class ObtenerClaimsService : IClaims
    {
        public List<Claim> ClaimsUsuario(Usuario usuario)
        {
            var lista = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.rol.NombreRol)
            };

            return lista;
        }
    }
}
