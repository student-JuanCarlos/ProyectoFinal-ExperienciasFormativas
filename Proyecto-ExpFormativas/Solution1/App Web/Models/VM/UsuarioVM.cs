using Entities;

namespace App_Web.Models.VM
{
    public class UsuarioVM
    {

        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; }

        public string Documento { get; set; }

        public string Telefono { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string Email { get; set; }

        public string Contraseña { get; set; }

        public decimal Sueldo { get; set; }

        public bool Estado { get; set; }

        public int IdCargo { get; set; }

        public int IdRol { get; set; }

        public RolVM rol { get; set; }

        public CargoVM cargo { get; set; }

    }
}
