using Business_Logic.Utilidades.EMAIL.Cliente.Interface;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Utilidades.EMAIL.Cliente.Generate
{
    public class EmailEstructureClienteService : IEmailClienteEstructure
    {
        public (string asunto, BodyBuilder cuerpo) BienvenidaCliente(Entities.Cliente cliente)
        {
            throw new NotImplementedException();
        }
    }
}
