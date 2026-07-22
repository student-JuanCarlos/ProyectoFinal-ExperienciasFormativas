using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class DescuentoRepository: IDescuento
    {
        private readonly AppDbContext _context;

        public DescuentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Actualizar(Descuento d)
        {
            _context.Descuentos.Update(d);
            return _context.SaveChanges();
        }

        public int ActualizarEstadoDescuentosHoy()
        {
            _context.Descuentos
                .Where(d => d.FechaInicio != null && d.FechaFin != null && (DateTime.Now < d.FechaInicio || DateTime.Now > d.FechaFin))
                .ExecuteUpdate(setters => setters.SetProperty(d => d.Estado, (bool)false));

            _context.Descuentos
                .Where(d => d.FechaInicio != null && d.FechaFin != null && (DateTime.Now >= d.FechaInicio && DateTime.Now <= d.FechaFin))
                .ExecuteUpdate(setters => setters.SetProperty(d => d.Estado, (bool)true));

            return 0;
        }

        public int Agregar(Descuento d)
        {
            _context.Descuentos.Add(d);
            return _context.SaveChanges();
        }

        public int CambiarEstado(int id)
        {
            var descuento = _context.Descuentos.Find(id);

            if(descuento == null)
            {
                return 0;
            }

            descuento.Estado = !descuento.Estado;

            return _context.SaveChanges();

        }

        public Descuento Detalle(int id)
        {
            return _context.Descuentos.Find(id);
        }

        public List<Descuento> Listado(string Busqueda, bool? Estado)
        {
            var query = _context.Descuentos.AsQueryable();

            if(Busqueda != null)
            {
                query = _context.Descuentos.Where(d => d.NombreDescuento.Contains(Busqueda));
            }

            if(Estado != null)
            {
                query = _context.Descuentos.Where(d => d.Estado == Estado);
            }

            return query.AsNoTracking().ToList();
        }

        #region
        public List<Descuento> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
