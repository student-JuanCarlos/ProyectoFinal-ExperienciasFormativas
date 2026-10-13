using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface ITransaccion
    {

        public bool RegistrarTransaccion(TransaccionWebhook t);

        public int MarcarProcesado(int IdTransaccion, int IdOrdenPago);

    }
}
