using Data.Context;
using Data.DTOs.ReservaDTO.Reserva;
using Data.DTOs.VentaDTO;
using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Repository
{
    public class VentaRepository: IVenta
    {
        private readonly AppDbContext _context;

        public VentaRepository(AppDbContext context)
        {
            _context = context;
        }

        public int RegistrarVenta(Venta v, List<DetalleVenta> detalle, List<DetalleDescuento> descuentos)
        {

            if (detalle == null)
            {
                throw new InvalidOperationException("No se encontraron platillos agregados a la venta");
            }

            //calcular descuentoUnitario de los descuentos
            #region
            var IdDescuentos = descuentos.Select(dd => dd.IdDescuento).ToList();

            var porcentajes = _context.Descuentos
                                      .Where(d => IdDescuentos.Contains(d.IdDescuento))
                                      .ToDictionary(d => d.IdDescuento, d => d.PorcentajeDescuento); //diccionario = clave - valor

            foreach(var d in descuentos)
            {
                d.DescuentoUnitario = porcentajes[d.IdDescuento];
            }

            var porcentajeDescuento = descuentos != null && descuentos.Any() ? 
                                      descuentos.Sum(d => d.DescuentoUnitario / 100) : 
                                      0;
            #endregion

            //encontrar mesas
            var IdMesas = _context.DetalleReserva
                                  .Where(dr => dr.IdReserva == v.IdReserva)
                                  .Select(dr => dr.IdMesa);

            //calcular precios de los platillos
            #region
            var IdPlatillos = detalle.Select(dv => dv.IdPlatillo).ToList();

            var precios = _context.Platillos
                                  .Where(p => IdPlatillos.Contains(p.IdPlatillo))
                                  .ToDictionary(p => p.IdPlatillo, p => p.Precio); //dicionario = clave - valor

            foreach(var d in detalle)
            {
                d.PrecioUnitario = precios[d.IdPlatillo];
            }

            var costoVenta = detalle.Sum(dv => dv.Cantidad * dv.PrecioUnitario); //al estar en memoria, tomar el dv.SubTotal directamente
                                                                                 // siempre seria 0, hay que calcularlo manualmente
            #endregion

            var costoReserva = _context.Reservas
                                       .Where(r => r.IdReserva == v.IdReserva)
                                       .Select(r => r.CostoTotal)
                                       .FirstOrDefault();

            var TotalVenta = costoVenta + costoReserva;

            var TotalConDescuento = TotalVenta - (TotalVenta * porcentajeDescuento);

            var venta = new Venta()
            {
                IdReserva = v.IdReserva,
                IdUsuario = v.IdUsuario,
                MetodoPago = v.MetodoPago,
                Total = TotalConDescuento,
                detalles = detalle,
                descuentos = descuentos,
            };

            using var transaccion = _context.Database.BeginTransaction();

            try
            {

                _context.Ventas.Add(venta);
                _context.SaveChanges();

                _context.Mesas
                        .Where(m => IdMesas.Contains(m.IdMesa))
                        .ExecuteUpdate(setters => setters.SetProperty(m => m.Estado, (int)1));

                _context.Reservas
                        .Where(r => r.IdReserva == venta.IdReserva)
                        .ExecuteUpdate(setters => setters.SetProperty(r => r.Estado, (int)2));

                transaccion.Commit();
                return venta.IdVenta;

            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }

        public List<Venta> Listado(string Busqueda)
        {
            var query = _context.Ventas.Include(v => v.reserva).Include(v => v.reserva.cliente).AsQueryable();

            if(Busqueda != null)
            {
                query = query.Where(v => Busqueda.Contains(v.reserva.cliente.Nombres) || Busqueda.Contains(v.reserva.NombreCliente));
            }

            return query.AsNoTracking().ToList();
        }

        public VentaDetalleCompletoDTO Detalle(int id)
        {
            var encabezado = _context.Database
                .SqlQuery<VentaEncabezadoDTO>($"EXEC sp_DetalleVenta_Encabezado @IdVenta = {id}")
                .AsEnumerable()
                .FirstOrDefault();

            var platillos = _context.Database
                .SqlQuery<VentaPlatilloDTO>($"EXEC sp_DetalleVenta_Platillos @IdVenta = {id}")
                .AsEnumerable()
                .ToList();

            var descuentos = _context.Database
                .SqlQuery<VentaDescuentoDTO>($"EXEC sp_DetalleVenta_Descuentos @IdVenta = {id}")
                .AsEnumerable()
                .ToList();

            return new VentaDetalleCompletoDTO
            {
                Encabezado = encabezado,
                Platillos = platillos,
                Descuentos = descuentos
            };
        }

    }
}
