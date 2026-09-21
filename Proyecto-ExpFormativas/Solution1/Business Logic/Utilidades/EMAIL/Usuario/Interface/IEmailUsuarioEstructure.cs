using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Utilidades.EMAIL.Usuario.Interface
{
    public interface IEmailUsuarioEstructure
    {

        public (string asunto, BodyBuilder cuerpo) BienvenidaUsuario(Entities.Usuario usuario);

    }
}
