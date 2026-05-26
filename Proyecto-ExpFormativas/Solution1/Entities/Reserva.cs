using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Reserva
    {

        public int IdReserva { get; set; }

        public int? IdCliente {  get; set; }

        public string TipoReserva { get; set; }

        public DateTime FechaReserva { get; set; }

        public TimeSpan HoraReserva { get; set; }

        public int CantidadPersonas { get; set; }

        public decimal CostoTotal { get; set; }

        public bool Estado {  get; set; }

        public int? IdUsuario { get; set; }

        public Usuario usuario { get; set; }

        public Cliente cliente {  get; set; }

        public List<Mesa> DetalleMesa { get; set; }

    }
}
