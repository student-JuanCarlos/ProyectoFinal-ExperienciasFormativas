using Data.Context;
using Data.Infraestructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaccion;

        public UnidadTrabajo(AppDbContext context)
        {
            _context = context;
        }

        public void Confirmar()
        {
            _transaccion = _context.Database.BeginTransaction();
        }

        public void Iniciar()
        {
            _transaccion.Commit();
        }

        public void Revertir()
        {
            _transaccion.Rollback();
        }
    }
}
