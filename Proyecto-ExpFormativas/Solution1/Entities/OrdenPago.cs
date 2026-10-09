using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class OrdenPago
    {

        public int IdOrdenPago { get; set; }

        public decimal Monto { get; set; }

        public string Moneda { get; set; } //'PEN', 'USD´

        public string Estado { get; set; } //'Pendiente' 'Procesando' 'Confirmado' 'Fallido' 'Expirado'

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaResolucion { get; set; }

        public string ReferenciaExterna { get; set; }

        public int IdSuscripcion { get; set; }

        public Suscripcion suscripcion { get; set; }

    }
}
