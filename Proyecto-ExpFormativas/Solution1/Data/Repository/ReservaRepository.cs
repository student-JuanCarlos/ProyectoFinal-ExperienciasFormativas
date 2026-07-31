using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Data.DTOs.ReservaDTO.Reserva;
using Data.DTOs.ReservaDTO.Cliente;

namespace Data.Repository
{
    public class ReservaRepository: IReserva
    {
        private readonly AppDbContext _context;

        public ReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public int ActualizarMesas(int IdReserva, List<int> IdMesas)
        {
            var detalleReserva = _context.DetalleReserva
                                         .Where(dr => dr.IdReserva == IdReserva)
                                         .Select(dr => dr.IdMesa).ToList();

            var reservaHoy = _context.Reservas
                                     .Any(r => r.IdReserva == IdReserva && r.FechaReserva.Value.Date == DateTime.Now.Date);

            using var transaccion = _context.Database.BeginTransaction();

            try
            {
                _context.Mesas
                    .Where(m => detalleReserva.Contains(m.IdMesa) && !IdMesas.Contains(m.IdMesa))
                    .ExecuteUpdate(setters => setters.SetProperty(m => m.Estado, (int)1));

                _context.DetalleReserva
                    .Where(dr => dr.IdReserva == IdReserva)
                    .ExecuteDelete();

                var nuevoDetalle = IdMesas.Select(id => new DetalleReserva()
                {
                    IdReserva = IdReserva,
                    IdMesa = id
                }).ToList();

                _context.DetalleReserva.AddRange(nuevoDetalle);

                _context.SaveChanges();

                _context.Mesas
                    .Where(m => IdMesas.Contains(m.IdMesa) && reservaHoy)
                    .ExecuteUpdate(setters => setters.SetProperty(m => m.Estado, (int)2));

                transaccion.Commit();
                return IdReserva;

            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }

        public int ActualizarReserva(Reserva r)
        {
            var reserva = _context.Reservas.Find(r.IdReserva);

            if(reserva == null)
            {
                return 0;
            }

            reserva.NombreCliente = r.NombreCliente;
            reserva.TelefonoCliente = r.TelefonoCliente;
            reserva.CantidadPersonas = r.CantidadPersonas;

            _context.Reservas.Update(reserva);

            return _context.SaveChanges();
        }

        public int ActualizarReserva_Cliente(Reserva r)
        {
            var reserva = _context.Reservas.Find(r.IdReserva);

            if(reserva == null)
            {
                return 0;
            }

            reserva.FechaReserva = r.FechaReserva;
            reserva.HoraReserva = r.HoraReserva;
            reserva.CantidadPersonas = r.CantidadPersonas;
            reserva.IdReserva = r.IdReserva;

            _context.Reservas.Update(reserva);

            return _context.SaveChanges();
        }

        public int CancelarReserva(int IdReserva, List<int> IdMesas)
        {
            using var transaccion = _context.Database.BeginTransaction();
            try
            {
                _context.Reservas
                        .Where(r => r.Estado == 1 && r.IdReserva == IdReserva)
                        .ExecuteUpdate(setters => setters.SetProperty(r => r.Estado, (int)3));

                _context.Mesas
                        .Where(m => IdMesas.Contains(m.IdMesa))
                        .ExecuteUpdate(setters => setters.SetProperty(m => m.Estado, (int)1));

                transaccion.Commit();
                return IdReserva;
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }

        public ReservaDetalleCompletoDTO Detalle(int id)
        {
            var encabezado = _context.Database
                .SqlQuery<ReservaEncabezadoDTO>($"EXEC sp_DetalleReserva_Encabezado @IdReserva = {id}")
                .AsEnumerable()
                .FirstOrDefault();

            var mesas = _context.Database
                .SqlQuery<ReservaMesaDTO>($"EXEC sp_DetalleReserva_Mesas @IdReserva = {id}")
                .AsEnumerable()
                .ToList();

            var cliente = _context.Database
                .SqlQuery<ReservaClienteDTO>($"EXEC sp_DetalleReserva_Cliente @IdReserva = {id}")
                .AsEnumerable()
                .FirstOrDefault();

            return new ReservaDetalleCompletoDTO
            {
                Encabezado = encabezado,
                Mesas = mesas,
                Cliente = cliente
            };
        }

        public DetalleReservaClienteCompletoDTO DetalleReservaCliente(int id)
        {
            var encabezado = _context.Database
                .SqlQuery<DetalleReservaClienteDTO>($"EXEC sp_DetalleReserva_Cliente_Encabezado @IdReserva = {id}")
                .AsEnumerable()
                .FirstOrDefault();

            var mesas = _context.Database
                .SqlQuery<ReservaMesaDTO>($"EXEC sp_DetalleReserva_Mesas @IdReserva = {id}")
                .AsEnumerable()
                .ToList();

            return new DetalleReservaClienteCompletoDTO
            {
                Encabezado = encabezado,
                Mesas = mesas
            };
        }

        public int InsertarReserva(Reserva r, List<DetalleReserva> detalle)
        {

            var precioReserva = _context.ConfiReserva.FirstOrDefault();

            var IdsMesas = detalle.Select(d => d.IdMesa);

            var espacioTotal = _context.Mesas
                .Where(m => IdsMesas.Contains(m.IdMesa))
                .Sum(m => m.EspacioOcupable);

            if(precioReserva == null)
            {
                throw new InvalidOperationException("No hay un costo de reserva registrado aun");
            }

            if (espacioTotal < r.CantidadPersonas)
            {
                throw new InvalidOperationException("Espacio insuficiente para la Cantidad de Personas");
            }

            if (r.TipoReserva == "Web")
            {
                r.CostoTotal = precioReserva.PrecioReserva * r.CantidadPersonas;
            }

            var reserva = new Reserva()
            {
                IdCliente = r.IdCliente,
                TipoReserva = r.TipoReserva,
                NombreCliente = r.NombreCliente,
                TelefonoCliente = r.TelefonoCliente,
                FechaReserva = r.FechaReserva,
                HoraReserva = r.HoraReserva,
                CantidadPersonas = r.CantidadPersonas,
                CostoTotal = r.CostoTotal,
                IdUsuario = r.IdUsuario
            };

            reserva.DetalleMesa = detalle;

            using var transaccion = _context.Database.BeginTransaction();

            try
            {
                _context.Reservas.Add(reserva);
                _context.SaveChanges();

                if (r.FechaReserva.Value.Date == DateTime.Now.Date)
                {
                    _context.Mesas
                        .Where(m => IdsMesas.Contains(m.IdMesa))
                        .ExecuteUpdate(setters => setters.SetProperty(m => m.Estado, (int)2));
                }

                transaccion.Commit();
                return reserva.IdReserva;
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }

        public List<Reserva> Listado(string Busqueda, int? Estado)
        {
            var query = _context.Reservas.Include(r => r.cliente).Where(r => r.FechaReserva.Value.Date == DateTime.Now.Date).AsQueryable();

            if(Busqueda != null)
            {
                query = query.Where(r => r.NombreCliente.Contains(Busqueda) || r.cliente.Nombres.Contains(Busqueda));
            }

            if(Estado.HasValue)
            {
                query = query.Where(r => r.Estado == Estado);
            }

            return query.AsNoTracking().ToList();
        }

        public List<Reserva> ListadoReserva_Cliente(int IdCliente)
        {
            return _context.Reservas
                .Include(dr => dr.DetalleMesa)
                .Include(c => c.cliente)
                .Where(r => r.IdCliente == IdCliente && r.Estado == 1)
                .AsNoTracking()
                .OrderBy(r => r.FechaReserva).ToList();
        }
    }
}
