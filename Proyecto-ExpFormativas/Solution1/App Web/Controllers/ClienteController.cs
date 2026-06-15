using App_Web.Models.Extension;
using App_Web.Models.Request;
using App_Web.Models.VM;
using Business_Logic.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App_Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteService clienteService;
        private readonly MesaService mesaService;
        private readonly ReservaService reservaService;
        private readonly ConfiReservaService configService;

        public ClienteController(ClienteService cliente, MesaService mesa, ReservaService reserva, ConfiReservaService confi)
        {
            clienteService = cliente;
            mesaService = mesa;
            reservaService = reserva;
            configService = confi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Reserva()
        {

            if (HttpContext.Session.GetString("Cliente") == null)
                return RedirectToAction("Index", "Cliente");

            var json = HttpContext.Session.GetString("Cliente");

            if(json != null)
            {
                var usuario = JsonConvert.DeserializeObject<App_Web.Models.VM.ClienteVM>(json);
                ViewBag.Usuario = usuario;
            }

            ViewBag.PrecioReserva = configService.DetallePrecioReserva().ToViewModel();

            return View();
        }

        [HttpGet]
        public IActionResult MiCuenta()
        {

            if (HttpContext.Session.GetString("Cliente") == null)
            {
                return RedirectToAction("Login", "Cliente");
            }

            var json = HttpContext.Session.GetString("Cliente");
            var cliente = JsonConvert.DeserializeObject<App_Web.Models.VM.ClienteVM>(json);

            ViewBag.ReservaCliente = reservaService.ListadoReserva_Cliente(cliente.IdCliente).Select(r => r.ToViewModel());

            return View();
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DetalleReserva(int IdReserva)
        {
            var detalle = reservaService.DetalleReserva_Cliente(IdReserva).ToViewModel();

            return View(detalle);
        }

        [HttpPost]
        public IActionResult GestionarCliente(ClienteVM model)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                string nombreImagen = model.FotoActual ?? "";

                if (model.Fotografia != null)
                {
                    if (model.IdCliente != 0 && !string.IsNullOrEmpty(model.FotoActual))
                    {
                        var fotoAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.FotoActual);
                        if (System.IO.File.Exists(fotoAnterior))
                            System.IO.File.Delete(fotoAnterior);
                    }

                    var nombreRealImagen = Path.GetFileName(model.Fotografia.FileName);
                    nombreImagen = $"assets/img/clientes/{nombreRealImagen}";
                    var pathImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/clientes", nombreRealImagen);

                    using (var stream = new FileStream(pathImagen, FileMode.Create))
                    {
                        model.Fotografia.CopyTo(stream);
                    }
                }

                model.FotoActual = $"{nombreImagen}";

                clienteService.GestionarCliente(model.ToEntity());
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        [HttpPost]
        public IActionResult ObtenerSesion()
        {
            var json = HttpContext.Session.GetString("Cliente");
            var cliente = JsonConvert.DeserializeObject<App_Web.Models.VM.ClienteVM>(json);

            var clientelogueado = clienteService.Detalle(cliente.IdCliente);

            return Json(clientelogueado);
        }

        [HttpGet]
        public JsonResult DetalleReserva_Cliente(int id)
        {

            var detalle = reservaService.DetalleReserva_Cliente(id).ToViewModel();

            return Json(detalle);
        }

        [HttpPost]
        public IActionResult Login(string Email, string Contraseña)
        {
            var cliente = clienteService.LoginCliente(Email, Contraseña);

            if(cliente == null)
            {
                ViewBag.Error = "Correo o Contraseña incorrecta";
                return View("Login");
            }

            HttpContext.Session.SetString("Cliente", JsonConvert.SerializeObject(cliente));
            return RedirectToAction("MiCuenta", "Cliente");
        }

        [HttpGet]
        public JsonResult FiltradoMesas(DateTime FechaReserva, TimeSpan HoraReserva)
        {
            var mesas = mesaService.FiltradoMesas_Cliente(FechaReserva, HoraReserva).Select(m => m.ToViewModel()).ToList();

            return Json(mesas);
        }

        [HttpPost]
        public IActionResult InsertarReserva([FromBody] ReservaRequest request)
        {
            var json = HttpContext.Session.GetString("Cliente");
            var cliente = JsonConvert.DeserializeObject<App_Web.Models.VM.ClienteVM>(json);

            request.Reserva.IdCliente = cliente.IdCliente;
            request.Reserva.TipoReserva = "Web";

            reservaService.InsertarReserva(request.Reserva.ToEntity(), request.DetalleReserva.Select(dr => dr.ToEntity()).ToList());

            return RedirectToAction("MiCuenta", "Cliente");

        }

        [HttpPost]
        public IActionResult ActualizarReserva(ReservaVM reserva)
        {
            reservaService.ActualizarReserva_Cliente(reserva.ToEntity());

            return RedirectToAction("DetalleReserva", new { IdReserva = reserva.IdReserva});
        }

        [HttpPost]
        public IActionResult CancelarReserva([FromBody] CancelarReservaRequest request)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("IdMesa", typeof(int));
            foreach (var id in request.Mesas)
                dt.Rows.Add(id);

            reservaService.CancelarReserva(request.IdReserva, dt);

            return RedirectToAction("MiCuenta", "Cliente");
        }

        [HttpPost]
        public IActionResult ActualizarMesas([FromBody] CancelarReservaRequest request)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IdMesa", typeof(int));
            foreach (var id in request.Mesas)
                dt.Rows.Add(id);

            reservaService.ActualizarMesas(request.IdReserva, dt);

            return RedirectToAction("DetalleReserva", new { IdReserva = request.IdReserva});
            
        }
            
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Cliente");
        }

    }
}
