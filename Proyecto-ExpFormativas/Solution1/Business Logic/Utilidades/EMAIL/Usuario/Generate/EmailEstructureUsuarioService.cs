using Business_Logic.Utilidades.EMAIL.Usuario.Interface;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Utilidades.EMAIL.Usuario.Generate
{
    public class EmailEstructureUsuarioService : IEmailUsuarioEstructure
    {
        public (string asunto, BodyBuilder cuerpo) BienvenidaUsuario(Entities.Usuario usuario)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = $"<p>Hola {usuario.NombreUsuario}, Bienvenido a la Estancia Gourmet</p>";

            return ("Bienvenido a la Estancia Gourmet", builder);
        }
    }
}
