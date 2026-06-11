namespace App_Web.Models.Request
{
    public class CancelarReservaRequest
    {

        public int IdReserva { get; set; }
        public List<int> Mesas { get; set; }

    }
}
