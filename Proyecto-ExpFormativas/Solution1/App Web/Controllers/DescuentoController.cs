using App_Web.Models.Extension;
using App_Web.Models.VM;
using Business_Logic.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App_Web.Controllers
{
    public class DescuentoController : Controller
    {
        private readonly DescuentoService descuentoService;

        public DescuentoController(DescuentoService descuento)
        {
            descuentoService = descuento;
        }

        public IActionResult Index(int page = 1, string Busqueda = null, bool? Estado = null)
        {

            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var listado = descuentoService.ListadoDescuento(Busqueda, Estado).Select(d => d.ToViewModel()).ToList();

            int registrosPorPagina = 8;
            int totalDescuentos = listado.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalDescuentos / registrosPorPagina));

            int paginasPorOmitir = registrosPorPagina * (page - 1);

            ViewBag.Paginas = cantidadPaginas;
            ViewBag.PaginaActual = page;

            return View(listado.Skip(paginasPorOmitir).Take(registrosPorPagina));
        }

        [HttpPost]
        public IActionResult GestionarDescuento (DescuentoVM descuento)
        {

            if(descuento.TipoDescuento == "Sin Fecha")
            {
                descuento.FechaInicio = null;
                descuento.FechaFin = null;
            }

            descuentoService.GestionarDescuento(descuento.ToEntity());

            return RedirectToAction("Index", "Descuento");
        }

        [HttpPost]
        public IActionResult CambiarEstadoDescuento(int id)
        {
            descuentoService.CambiarEstadoDescuento(id);

            return RedirectToAction("Index", "Descuento");
        }

        [HttpGet]
        public JsonResult Detalle(int id)
        {
            var descuento = descuentoService.Detalle(id).ToViewModel();

            return Json(descuento);
        }
    }
}
