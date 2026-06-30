using App_Web.Models.Extension;
using App_Web.Models.Request;
using Business_Logic.Service;
using Business_Logic.Utilidades.PDF.Generate;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App_Web.Controllers
{
    public class VentaController : Controller
    {
        private readonly VentaService ventaService;
        private readonly PlatilloService platilloService;
        private readonly DescuentoService descuentoService;
        private readonly ReservaService reservaService;
        private readonly ClienteService clienteService;
        private readonly VentaPDFService ventaPDFService;

        public VentaController(VentaService venta, PlatilloService platillo, DescuentoService descuento, ReservaService reserva, ClienteService cliente, VentaPDFService ventapdf)
        {
            ventaService = venta;
            platilloService = platillo;
            descuentoService = descuento;
            reservaService = reserva;
            clienteService = cliente;
            ventaPDFService = ventapdf;
        }

        public IActionResult Index(string Busqueda) 
        {

            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var listado = ventaService.Listado(Busqueda).Select(v => v.ToViewModel());

            ViewBag.Platillos = platilloService.ListadoPlatillo(null).Select(p => p.ToViewModel()).ToList();
            ViewBag.Descuentos = descuentoService.ListadoDescuento(null, true).Select(d => d.ToViewModel()).ToList();
            ViewBag.Reservas = reservaService.ListadoReserva(null, 1).Select(r => r.ToViewModel()).ToList();

            return View(listado);
        }

        [HttpPost]
        public IActionResult InsertarVenta([FromBody] VentaRequest request)
        {
            var json = HttpContext.Session.GetString("Usuario");
            var usuario = JsonConvert.DeserializeObject<App_Web.Models.VM.UsuarioVM>(json);

            request.venta.IdUsuario = usuario.IdUsuario;

            ventaService.RegistrarVenta(request.venta.ToEntity(), request.detalleVenta.Select(dv => dv.ToEntity()).ToList(), request.detalleDescuento.Select(dd => dd.ToEntity()).ToList());

            return RedirectToAction("Index", "Venta");
        }

        [HttpGet]
        public JsonResult Detalle(int id)
        {
            var venta = ventaService.Detalle(id).ToViewModel();

            return Json(venta);
        }

        [HttpGet]
        public IActionResult ExportarVentaPDF(int id)
        {
            var venta = ventaService.Detalle(id);

            var (bytes, nombre) = ventaPDFService.ObtenerArchivo(venta);

            return File(bytes, "application/pdf", nombre);
        }
    }
}
