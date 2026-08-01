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
    public class PrecioReservaRepository: IConfigurationReserva
    {
        private readonly AppDbContext _context;

        public PrecioReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public int ActualizarPrecio(decimal precio)
        {
            return _context.ConfiReserva
                .ExecuteUpdate(setters => setters.SetProperty(c => c.PrecioReserva, (decimal)precio));
        }

        public ConfiguracionReserva DetallePrecioReserva()
        {
            return _context.ConfiReserva.FirstOrDefault();
        }
    }
}
