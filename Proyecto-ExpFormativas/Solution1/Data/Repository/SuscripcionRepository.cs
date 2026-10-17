using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.EntityFrameworkCore;
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

        public int Actualizar(Suscripcion s)
        {
            var suscripcion = _context.Suscripcion.Find(s.IdSuscripcion);

            if(suscripcion == null)
            {
                return 0;
            }

            suscripcion.Estado = s.Estado;
            suscripcion.FechaInicio = s.FechaInicio;
            suscripcion.FechaVencimiento = s.FechaVencimiento;

            return _context.SaveChanges();
        }

        public int Agregar(Suscripcion s)
        {
            _context.Suscripcion.Add(s);
            return _context.SaveChanges();
        }

        public Suscripcion Detalle(int id)
        {
            return _context.Suscripcion.Include(s => s.restaurante).Include(s => s.tipoPlan).AsNoTracking().FirstOrDefault(s => s.IdSuscripcion == id);
        }

        public Suscripcion ObtenerActivaPorRestaurante(int idRestaurante)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            return _context.Suscripcion
                            .AsNoTracking()
                            .FirstOrDefault(s => s.IdRestaurante == idRestaurante && s.Estado == "Activa" && s.FechaVencimiento >= hoy);

        }

        #region
        public List<Suscripcion> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
