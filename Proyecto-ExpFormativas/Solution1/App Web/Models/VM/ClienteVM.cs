namespace App_Web.Models.VM
{
    public class ClienteVM
    {
        public int IdCliente { get; set; }

        public string NombreCompleto { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public IFormFile Fotografia { get; set; }

        public string Documento { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string Contraseña { get; set; }

        public string FotoActual { get; set; }

    }
}
