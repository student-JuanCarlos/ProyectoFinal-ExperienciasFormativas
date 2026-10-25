using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Business_Logic.Service
{
    public class SuscripcionService : ISuscripcion
    {
        private readonly ISuscripcion suscripcionDB;
        private readonly IOrdenPago ordenDB;
        private readonly ITipoPlan planDB;
        private readonly IPasarelaPago pasarela;

        public SuscripcionService(ISuscripcion service, IOrdenPago orden, ITipoPlan plan, IPasarelaPago pasarelaservice)
        {
            suscripcionDB = service;
            ordenDB = orden;
            planDB = plan;
            pasarela = pasarelaservice;
        }

        public int Actualizar(Suscripcion s)
        {
            return suscripcionDB.Actualizar(s);
        }

        public int Agregar(Suscripcion s)
        {
            return suscripcionDB.Agregar(s);
        }

        public Suscripcion Detalle(int id)
        {
            return suscripcionDB.Detalle(id);
        }

        public Suscripcion ObtenerActivaPorRestaurante(int idRestaurante)
        {
            return suscripcionDB.ObtenerActivaPorRestaurante(idRestaurante);
        }

        public string IniciarSuscripcion(int IdRestaurante, int IdTipoPlan)
        {
            var plan = planDB.Detalle(IdTipoPlan);

            if (plan == null || !plan.Estado)
            {
                throw new InvalidOperationException("El plan no esta disponible");
            }

            var orden = ordenDB.ObtenerPendienteReciente(IdRestaurante, IdTipoPlan);

            if (orden == null)
            {
                var suscripcion = suscripcionDB.ObtenerActivaPorRestaurante(IdRestaurante);

                if (suscripcion != null && suscripcion.IdTipoPlan != IdTipoPlan)
                {
                    throw new InvalidOperationException("Para cambiar el plan contacta a soporte"); //por cambiar
                }

                if (suscripcion == null) //cliente nuevo o suscripcion nueva
                {
                    var hoy = DateOnly.FromDateTime(DateTime.Today);

                    suscripcion = new Suscripcion()
                    {
                        Estado = "Pendiente",
                        FechaInicio = hoy,
                        FechaVencimiento = hoy.AddMonths(1),
                        RenovacionAutomatica = false,
                        IdRestaurante = IdRestaurante,
                        IdTipoPlan = IdTipoPlan,
                    };

                    suscripcionDB.Agregar(suscripcion);
                }

                orden = new OrdenPago()
                {
                    Monto = plan.PrecioMensual,
                    Moneda = "PEN",
                    Estado = "Pendiente",
                    FechaCreacion = DateTime.Now,
                    ReferenciaExterna = Guid.NewGuid().ToString(),
                    IdSuscripcion = suscripcion.IdSuscripcion
                };

                ordenDB.Agregar(orden);
            }

            try
            {
                return pasarela.CrearCheckOut(orden, plan);
            }
            catch
            {
                orden.Estado = "Fallido";
                orden.FechaResolucion = DateTime.Now;
                ordenDB.Actualizar(orden);
                throw;
            }
        }

        #region
        public List<Suscripcion> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
