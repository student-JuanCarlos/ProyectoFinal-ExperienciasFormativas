using Data.Context;
using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class SuscripcionRepository: ISuscripcion
    {
        public readonly AppDbContext _context;

        public SuscripcionRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Actualizar(Suscripcion entity)
        {
            throw new NotImplementedException();
        }

        public int Agregar(Suscripcion entity)
        {
            throw new NotImplementedException();
        }

        public Suscripcion Detalle(int id)
        {
            throw new NotImplementedException();
        }

        #region
        public List<Suscripcion> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
