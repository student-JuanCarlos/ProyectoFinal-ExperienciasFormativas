using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.EntityFrameworkCore;
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

        public int Actualizar(OrdenPago o)
        {
            _context.OrdenPago.Add(o);
            return _context.SaveChanges();
        }

        public int Agregar(OrdenPago o)
        {
            var orden = _context.OrdenPago.Find(o.IdOrdenPago);

            if (orden == null)
                return 0;

            orden.Estado = o.Estado;
            orden.FechaResolucion = o.FechaResolucion;
            orden.ReferenciaExterna = o.ReferenciaExterna;

            return _context.SaveChanges();
        }

        public OrdenPago ObtenerPendienteReciente(int idRestaurante, int idTipoPlan)
        {
            var limite = DateTime.Now.AddMinutes(-10);

            return _context.OrdenPago
                           .AsNoTracking()
                           .Where(o => o.Estado == "Pendiente"
                           && o.FechaCreacion >= limite
                           && o.suscripcion.IdRestaurante == idRestaurante
                           && o.suscripcion.IdTipoPlan == idTipoPlan)
                           .OrderByDescending(o => o.FechaCreacion)
                           .FirstOrDefault();
        }

        public OrdenPago ObtenerPorReferenciaExterna(string referencia)
        {
            return _context.OrdenPago
                           .AsNoTracking()
                           .Where(o => o.ReferenciaExterna == referencia)
                           .FirstOrDefault();
        }
        
        public OrdenPago Detalle(int id)
        {
            return _context.OrdenPago
                           .AsNoTracking()
                           .Include(o => o.suscripcion)
                           .FirstOrDefault(o => o.IdOrdenPago == id);
        }

        #region
        public List<OrdenPago> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
