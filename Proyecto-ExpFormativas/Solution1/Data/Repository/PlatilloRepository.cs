using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Repository
{
    public class PlatilloRepository: IPlatillo
    {
        private readonly AppDbContext _context;

        public PlatilloRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Agregar(Platillo p)
        {
            _context.Platillos.Add(p);
            return _context.SaveChanges();
        }

        public int Actualizar(Platillo p)
        {
            _context.Platillos.Update(p);
            return _context.SaveChanges();
        }

        public List<Platillo> Listado(string Busqueda)
        {
            var query = _context.Platillos.Include(p => p.categoria).AsQueryable();

            if(Busqueda != null)
            {
                query = _context.Platillos.Include(p => p.categoria).Where(p => p.NombrePlatillo.Contains(Busqueda) || p.categoria.NombreCategoria.Contains(Busqueda));
            }

            return query.AsNoTracking().ToList();
        }

        public Platillo Detalle(int id)
        {
            return _context.Platillos.Include(p => p.categoria).AsNoTracking().FirstOrDefault(p => p.IdPlatillo == id);
        }

    }
}
