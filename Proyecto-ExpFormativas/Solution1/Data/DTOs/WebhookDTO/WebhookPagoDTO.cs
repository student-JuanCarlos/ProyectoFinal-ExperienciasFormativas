using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Data.DTOs.WebhookDTO
{
    public enum EstadoPagoPasarela { Pendiente, Aprobado, Rechazado }

    public class WebhookPagoDTO
    {

        public string IdEvento { get; set; }
        public string TipoEvento { get; set; }
        public string ReferenciaExterna { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))] //para escribir cadenas en Postman
        public EstadoPagoPasarela Estado { get; set; }

        public string PayloadCrudo { get; set; }

    }
}
