using App_Web.Models.Extension;
using App_Web.Models.Request;
using App_Web.Models.VM;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        private readonly BlobContainerClient _blobContainer;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(ClienteService cliente, MesaService mesa, ReservaService reserva, ConfiReservaService confi, BlobServiceClient blob, ILogger<ClienteController> logger)
        {
            clienteService = cliente;
            mesaService = mesa;
            reservaService = reserva;
            configService = confi;
            _blobContainer = blob.GetBlobContainerClient("clientes");
            _logger = logger;
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
            var detalle = reservaService.DetalleReservaCliente(IdReserva).DetalleClienteDTOtoVM();

            return View(detalle);
        }

        [HttpPost]
        public async Task<IActionResult> GestionarCliente(ClienteVM model)
        {
            string urlImagen = model.FotoActual ?? "";
            try
            {

                if (model.Fotografia != null)
                {
                    var nombreBlob = $"{Guid.NewGuid()}{Path.GetExtension(model.Fotografia.FileName)}";
                    var blobClient = _blobContainer.GetBlobClient(nombreBlob);

                    using var stream = model.Fotografia.OpenReadStream();

                    var opcionesClient = new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = model.Fotografia.ContentType
                        }
                    };

                    await blobClient.UploadAsync(stream, opcionesClient);

                    urlImagen = blobClient.Uri.ToString();

                    if(model.IdCliente != 0 && !string.IsNullOrEmpty(model.FotoActual))
                    {
                        var nombreAnterior = Path.GetFileName(new Uri(model.FotoActual).LocalPath);
                        await _blobContainer.DeleteBlobIfExistsAsync(nombreAnterior);
                    }
                    
                }

                model.FotoActual = urlImagen;
                clienteService.GestionarCliente(model.ToEntity());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Fallo al gestionar el cliente {IdCliente}", model.IdCliente);
            }

            return RedirectToAction("MiCuenta", "Cliente");
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

            var detalle = reservaService.DetalleReservaCliente(id).DetalleClienteDTOtoVM();

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
            var mesas = mesaService.FiltradoMesas_Cliente(FechaReserva, HoraReserva).Select(m => m.ListadoDTOtoVM()).ToList();

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

            reservaService.CancelarReserva(request.IdReserva, request.IdMesas);

            return RedirectToAction("MiCuenta", "Cliente");
        }

        [HttpPost]
        public IActionResult ActualizarMesas([FromBody] CancelarReservaRequest request)
        {

            reservaService.ActualizarMesas(request.IdReserva, request.IdMesas);

            return RedirectToAction("DetalleReserva", new { IdReserva = request.IdReserva});
            
        }
            
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Cliente");
        }

    }
}
