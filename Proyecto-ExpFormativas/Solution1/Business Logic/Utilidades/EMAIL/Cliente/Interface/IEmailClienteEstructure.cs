using Entities;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Utilidades.EMAIL.Cliente.Interface
{
    public interface IEmailClienteEstructure
    {

        public (string asunto, BodyBuilder cuerpo) BienvenidaCliente(Entities.Cliente cliente);

    }
}
