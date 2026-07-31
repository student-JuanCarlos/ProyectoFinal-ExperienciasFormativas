using Data.Context;
using Data.DTOs.MesaDTO;
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
    public class MesaRepository : IMesa
    {
        private readonly AppDbContext _context;

        public MesaRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Actualizar(Mesa m)
        {
            var mesa = _context.Mesas.Find(m.IdMesa);

            if(mesa == null)
            {
                return 0;
            }

            mesa.NumeroMesa = m.NumeroMesa;
            mesa.EspacioOcupable = m.EspacioOcupable;

            return _context.SaveChanges();
        }

        public void ActualizarEstadoMesasHoy()
        {
            _context.Database
                .ExecuteSqlRaw("EXEC sp_ActualizarEstadoMesasHoy");
        }

        public int Agregar(Mesa m)
        {

            var mesa = new Mesa()
            {
                NumeroMesa = m.NumeroMesa,
                EspacioOcupable = m.EspacioOcupable,
            };

            _context.Mesas.Add(mesa);

            return _context.SaveChanges();
        }

        public MesaDetalleDTO Detalle(int id)
        {
            return _context.Database
                   .SqlQuery<MesaDetalleDTO>($"EXEC sp_DetalleMesa @IdMesa = {id}")
                   .AsEnumerable()
                   .FirstOrDefault();
            
        }

        public List<MesaListadoDTO> FiltradoMesas_Cliente(DateTime FechaReserva, TimeSpan HoraReserva)
        {
            var fechaHoraReserva = FechaReserva.Date.Add(HoraReserva);  
            var fechaHoraLimite = fechaHoraReserva.AddHours(3);

            var limiteFecha = fechaHoraReserva.Date;
            var limiteHora = fechaHoraLimite.TimeOfDay;

            return _context.Mesas
                .Select(m => new MesaListadoDTO()
                {
                    IdMesa = m.IdMesa,
                    NumeroMesa = m.NumeroMesa,
                    EspacioOcupable = m.EspacioOcupable,
                    Estado = _context.Reservas
                             .Any(r => r.DetalleMesa.Any(dr => dr.IdMesa == m.IdMesa)
                                 && r.FechaReserva == limiteFecha && r.HoraReserva < limiteHora
                                 && r.Estado == 1)
                             ? 3 : 1
                })
                .ToList();
        }

        public List<MesaListadoDTO> Listado()
        {
            return _context.Database
                   .SqlQuery<MesaListadoDTO>($"EXEC sp_FiltradoMesa")
                   .AsNoTracking()
                   .ToList();
        }

        #region
        public List<Mesa> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
