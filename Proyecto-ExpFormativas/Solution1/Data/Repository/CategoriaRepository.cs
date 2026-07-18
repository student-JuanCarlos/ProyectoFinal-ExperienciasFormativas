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
    public class CategoriaRepository : ICategoria
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Actualizar(Categoria c)
        {
            _context.Categorias.Update(c);
            return _context.SaveChanges();
        }

        public int Agregar(Categoria c)
        {
            _context.Categorias.Add(c);
            return _context.SaveChanges();
        }

        public Categoria Detalle(int id)
        {
            return _context.Categorias.Find(id);
        }

        public List<Categoria> Listado(string Busqueda)
        {
            var query = _context.Categorias.AsQueryable();

            if (!string.IsNullOrEmpty(Busqueda))
            {
                query = _context.Categorias.Where(c => c.NombreCategoria.Contains(Busqueda));
            }

            return query.AsNoTracking().ToList();
        }

        public int Eliminar(int id)
        {
            _context.Platillos
                .Where(p => p.IdCategoria == id)
                .ExecuteUpdate(setters => setters.SetProperty(p => p.IdCategoria, (int?)null));

            return _context.Categorias
                .Where(c => c.IdCategoria == id)
                .ExecuteDelete();
        }
    }
}
