using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class TipoPlanRepository: ITipoPlan
    {

        public readonly AppDbContext _context;

        public TipoPlanRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<TipoPlan> ListarPlanesActivos()
        {
            return _context.TipoPlan
                           .AsNoTracking()
                           .Where(p => p.Estado)
                           .ToList();
        }
    }
}
