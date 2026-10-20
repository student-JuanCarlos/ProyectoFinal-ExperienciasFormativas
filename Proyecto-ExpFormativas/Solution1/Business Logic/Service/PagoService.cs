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
                }

                var orden = ordenPagoDB.ObtenerPorReferenciaExterna(webhook.ReferenciaExterna);

                if(orden == null)
                {
                    _unidadTrabajo.Confirmar();
                }

                if(orden.Estado != "Confirmado")
                {

                }

            }
        }

    }
}
