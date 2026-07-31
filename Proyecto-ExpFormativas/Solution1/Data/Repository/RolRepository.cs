using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class RolRepository: IRol
    {
        public readonly AppDbContext _context;

        public RolRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Rol> Listado()
        {
            return _context.Roles.AsNoTracking().ToList();
        }
    }
}
