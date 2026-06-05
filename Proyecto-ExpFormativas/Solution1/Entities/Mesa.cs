using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Mesa
    {
        public int IdMesa {  get; set; }

        public int NumeroMesa { get; set; }

        public int EspacioOcupable {  get; set; }

        public int Estado {  get; set; }

        public string OcupadoPor { get; set; }
        public TimeSpan? HoraReserva { get; set; }
        public DateTime? FechaReserva { get; set; }
    }
}
