using System.ComponentModel.DataAnnotations;

namespace App_Web.Models.VM
{
    public class MesaVM
    {

        public int IdMesa { get; set; }

        [Required(ErrorMessage = "El numero de Mesa es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Numero no valido")]
        public int NumeroMesa { get; set; }

        [Required(ErrorMessage = "El Espacio Ocupable es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Numero no valido")]
        public int EspacioOcupable { get; set; }

        public int Estado { get; set; } = 1;

        public ReservaVM reserva {  get; set; }

        public ClienteVM cliente { get; set; }

    }
}
