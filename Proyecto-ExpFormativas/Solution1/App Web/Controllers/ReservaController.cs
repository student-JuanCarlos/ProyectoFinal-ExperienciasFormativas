using App_Web.Models.Extension;
using App_Web.Models.Request;
using App_Web.Models.VM;
using Business_Logic.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace App_Web.Controllers
{
    public class ReservaController : Controller
    {
        private readonly ReservaService reservaService;
        private readonly MesaService mesaService;

        public ReservaController(ReservaService reserva, MesaService mesa)
        {
            reservaService = reserva; 
            mesaService = mesa;
        }

        public IActionResult Index(string Busqueda = null, int? Estado = null)
        {

            ViewBag.Mesas = mesaService.ListadoMesa(null).Select(m => m.ToViewModel()).ToList();

            var listado = reservaService.ListadoReserva(Busqueda, Estado).Select(r => r.ToViewModel()).ToList();

            return View(listado);
        }

        [HttpPost]
        public IActionResult InsertarReserva([FromBody]ReservaRequest request)
        {

            var json = HttpContext.Session.GetString("Usuario");
            var usuario = JsonConvert.DeserializeObject<App_Web.Models.VM.UsuarioVM>(json);

            request.Reserva.IdUsuario = usuario.IdUsuario;
            request.Reserva.TipoReserva = "Directa";
            request.Reserva.FechaReserva = DateTime.Now;
            request.Reserva.HoraReserva = DateTime.Now.TimeOfDay;

            reservaService.InsertarReserva(request.Reserva.ToEntity(), request.DetalleReserva.Select(dr => dr.ToEntity()).ToList());

            return RedirectToAction("Index", "Reserva");
        }

        [HttpPost]
        public IActionResult ActualizarReserva(ReservaVM reserva)
        {
            reservaService.ActualizarReserva(reserva.ToEntity());

            return RedirectToAction("Index", "Reserva");
        }

        [HttpPost]
        public IActionResult CancelarReserva([FromBody]CancelarReservaRequest request)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IdMesa", typeof(int));
            foreach (var id in request.Mesas)
                dt.Rows.Add(id);

            reservaService.CancelarReserva(request.IdReserva, dt);

            return RedirectToAction("Index", "Reserva");
        }

        [HttpPost]
        public IActionResult ActualizarMesas([FromBody]CancelarReservaRequest request)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IdMesa", typeof(int));
            foreach(var id in request.Mesas)
                dt.Rows.Add(id);

            reservaService.ActualizarMesas(request.IdReserva, dt);

            return RedirectToAction("Index", "Reserva");
        }

        [HttpGet]
        public JsonResult Detalle(int id)
        {
            var reserva = reservaService.Detalle(id).ToViewModel();

            return Json(reserva);
        }
    }
}
