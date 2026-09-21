using Entities;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace Business_Logic.Utilidades.EMAIL.Usuario.Interface
{
    public interface IEmailUsuario
    {

        public Task EnviarCorreo(Entities.Usuario usuario);

    }
}
