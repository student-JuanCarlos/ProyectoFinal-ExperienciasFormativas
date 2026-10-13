using Data.Context;
using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class OrdenPagoRepository: IOrdenPago
    {
        public readonly AppDbContext _context;

        public OrdenPagoRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Actualizar(OrdenPago entity)
        {
            throw new NotImplementedException();
        }

        public int Agregar(OrdenPago entity)
        {
            throw new NotImplementedException();
        }

        public OrdenPago ObtenerPendienteReciente(int idRestaurante)
        {
            throw new NotImplementedException();
        }

        public OrdenPago ObtenerPorReferenciaExterna(string referencia)
        {
            throw new NotImplementedException();
        }

        #region
        public OrdenPago Detalle(int id)
        {
            throw new NotImplementedException();
        }

        public List<OrdenPago> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
