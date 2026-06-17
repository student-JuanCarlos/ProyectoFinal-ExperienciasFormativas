using App_Web.Models.Extension;
using App_Web.Models.Request;
using Business_Logic.Service;
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

        public VentaController(VentaService venta, PlatilloService platillo, DescuentoService descuento, ReservaService reserva, ClienteService cliente)
        {
            ventaService = venta;
            platilloService = platillo;
            descuentoService = descuento;
            reservaService = reserva;
            clienteService = cliente;
        }

        public IActionResult Index(string Busqueda) 
        {

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
    }
}
