using Data.DTOs.WebhookDTO;
using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class PagoService
    {
        private readonly ITransaccion transaccionDB;
        private readonly IOrdenPago ordenPagoDB;
        private readonly ISuscripcion suscripcionDB;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public PagoService(ITransaccion transaccion, IOrdenPago ordenPago,ISuscripcion suscripcion, IUnidadTrabajo unidadTrabajo)
        {
            transaccionDB = transaccion;
            ordenPagoDB = ordenPago;
            suscripcionDB = suscripcion;
            _unidadTrabajo = unidadTrabajo;
        }

        public void ProcesarWebhook(WebhookPagoDTO webhook)
        {
            _unidadTrabajo.Iniciar();
            try
            {
                var evento = new TransaccionWebhook
                {
                    IdEventoExterno = webhook.IdEvento,
                    TipoEvento = webhook.TipoEvento,
                    Payload = webhook.PayloadCrudo,
                    Estado = false, //no se procesa aun
                    FechaRecibido = DateTime.Now
                };

                if(transaccionDB.RegistrarTransaccion(evento) == false)
                {
                    _unidadTrabajo.Revertir();
                    return;
                }

                var orden = ordenPagoDB.ObtenerPorReferenciaExterna(webhook.ReferenciaExterna);

                if(orden == null)
                {
                    _unidadTrabajo.Confirmar();
                    return;
                }

                if(orden.Estado != "Confirmado")
                {
                    switch (webhook.Estado)
                    {
                        case EstadoPagoPasarela.Aprobado:
                            orden.Estado = "Confirmado";
                            orden.FechaResolucion = DateTime.Now;
                            ActivarSuscripcion(orden.suscripcion);
                            break;

                        case EstadoPagoPasarela.Rechazado:
                            orden.Estado = "Fallido";
                            orden.FechaResolucion = DateTime.Now;
                            break;

                        case EstadoPagoPasarela.Pendiente:
                            orden.Estado = "Procesando";
                            break;
                    }
                    ordenPagoDB.Actualizar(orden);
                }

                transaccionDB.MarcarProcesado(evento.IdTransaccionWebhook, orden.IdOrdenPago);
                _unidadTrabajo.Confirmar();
            }
            catch
            {
                _unidadTrabajo.Revertir();
                throw;
            }
        }

        public void ActivarSuscripcion(Suscripcion s)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            if (s.Estado == "Activa" && s.FechaVencimiento >= hoy)
            {
                s.FechaVencimiento = s.FechaVencimiento.AddMonths(1);
            }
            else
            {
                s.Estado = "Activa";
                s.FechaInicio = hoy;
                s.FechaVencimiento = hoy.AddMonths(1);
            }

            suscripcionDB.Actualizar(s);
        }

    }
}
