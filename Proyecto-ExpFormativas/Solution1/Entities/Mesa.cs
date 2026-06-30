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

        public Cliente cliente { get; set; }

        public Reserva reserva { get; set; }
    }
}
