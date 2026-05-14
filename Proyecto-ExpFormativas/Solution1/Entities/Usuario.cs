using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Entities
{
    public class Usuario
    {

        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; }

        public string Documento {  get; set; }

        public string Telefono { get; set; }

        public DateTime FechaRegistro {  get; set; }

        public string Email { get; set; }

        public string Contraseña { get; set; }

        public decimal Sueldo { get; set; }

        public bool Estado { get; set; }

        public int IdCargo {  get; set; }

        public int IdRol {  get; set; }

        public Rol rol {  get; set; }

        public Cargo cargo { get; set; }

    }
}
