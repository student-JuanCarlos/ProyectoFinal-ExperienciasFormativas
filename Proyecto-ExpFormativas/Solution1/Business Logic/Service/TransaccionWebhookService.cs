using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class TransaccionWebhookService : ITransaccion
    {
        private readonly ITransaccion transaccionDB;

        public TransaccionWebhookService(ITransaccion service)
        {
            transaccionDB = service;
        }

        public int MarcarProcesado(int IdTransaccion, int IdOrdenPago)
        {
            return transaccionDB.MarcarProcesado(IdTransaccion, IdOrdenPago);
        }

        public bool RegistrarTransaccion(TransaccionWebhook t)
        {
            return transaccionDB.RegistrarTransaccion(t);
        }
    }
}
