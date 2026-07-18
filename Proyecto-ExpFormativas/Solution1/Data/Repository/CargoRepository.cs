using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class CargoRepository : ICargo
    {
        private readonly AppDbContext _context;

        public CargoRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Actualizar(Cargo c)
        {
            _context.Cargos.Update(c);
            return _context.SaveChanges();
        }

        public int Agregar(Cargo c)
        {
            _context.Cargos.Add(c);
            return _context.SaveChanges();
        }

        public Cargo Detalle(int id)
        {
            return _context.Cargos.Find(id);
        }

        public List<Cargo> Listado(string Busqueda)
        {
            var query = _context.Cargos.AsQueryable();

            if (!string.IsNullOrEmpty(Busqueda))
            {
                query = query.Where(c => c.NombreCargo.Contains(Busqueda));
            }

            return query.AsNoTracking().ToList();
        }
    }
}
