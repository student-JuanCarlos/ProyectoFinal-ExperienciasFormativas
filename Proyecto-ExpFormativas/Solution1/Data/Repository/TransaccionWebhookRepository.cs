using Data.Context;
using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class TransaccionWebhookRepository: ITransaccion
    {
        public readonly AppDbContext _contex;

        public TransaccionWebhookRepository(AppDbContext context)
        {
            _contex = context;
        }

        public int MarcarProcesado(int IdTransaccion, int IdOrdenPago)
        {
            throw new NotImplementedException();
        }

        public bool RegistrarTransaccion(TransaccionWebhook t)
        {
            throw new NotImplementedException();
        }
    }
}
