using Business_Logic.Utilidades.JWT.Interface;
using Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Business_Logic.Utilidades.JWT.Generate
{
    public class GenerarJWTService : IJWT
    {
        private readonly IConfiguration _configuration;
        private readonly IClaims _claims;

        public GenerarJWTService(IConfiguration configuration, IClaims claims)
        {
            _configuration = configuration;
            _claims = claims;
        }

        public string GenerarJWTUsuario(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var claimsUsuario = _claims.ClaimsUsuario(usuario);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(

                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claimsUsuario,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credenciales

            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
