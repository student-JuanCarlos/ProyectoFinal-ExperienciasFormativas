using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Suscripcion
    {

        public int IdSuscripcion {  get; set; }

        public string Estado { get; set; } //'Activa´ 'Vencida' 'Cancelada'

        public DateOnly FechaInicio {  get; set; }

        public DateOnly FechaVencimiento { get; set; }

        public bool RenovacionAutomatica { get; set; }

        public int IdRestaurante { get; set; }

        public Restaurante restaurante { get; set; }

        public int IdTipoPlan {  get; set; }

        public TipoPlan tipoPlan { get; set; }

    }
}
