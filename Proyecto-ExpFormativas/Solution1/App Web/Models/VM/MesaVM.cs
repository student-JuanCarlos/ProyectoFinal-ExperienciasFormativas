namespace App_Web.Models.VM
{
    public class MesaVM
    {

        public int IdMesa { get; set; }

        public int NumeroMesa { get; set; }

        public int EspacioOcupable { get; set; }

        public int Estado { get; set; }

        public ReservaVM reserva {  get; set; }

        public ClienteVM cliente { get; set; }

    }
}
