using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class TransaccionWebhook
    {

        public int IdTransaccionWebhook {  get; set; }

        public string IdEventoExterno { get; set; }

        public string TipoEvento {  get; set; }

        public string Payload { get; set; }

        public bool Estado {  get; set; } // PROCESADO / NO PROCESADO

        public DateTime FechaRecibido { get; set; }

        public int? IdOrdenPago { get; set; }

        public OrdenPago? ordenPago { get; set; }

    }
}
