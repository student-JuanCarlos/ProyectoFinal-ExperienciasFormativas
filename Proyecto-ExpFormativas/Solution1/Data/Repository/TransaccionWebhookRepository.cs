using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
            var transaccion = _contex.Transaccion.Find(IdTransaccion);

            if (transaccion == null)
                return 0;

            transaccion.Estado = true;
            transaccion.IdOrdenPago = IdOrdenPago;

            return _contex.SaveChanges();
        }

        public bool RegistrarTransaccion(TransaccionWebhook t)
        {
            try
            {
                _contex.Transaccion.Add(t);
                _contex.SaveChanges();
                return true;
            }
            catch(DbUpdateException ex) when (ex.InnerException is SqlException { Number : 2601 or 2627 })
            {
                _contex.Entry(t).State = EntityState.Detached;
                return false;
            }
        }
    }
}
