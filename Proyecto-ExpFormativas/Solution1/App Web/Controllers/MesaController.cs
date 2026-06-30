using App_Web.Models.Extension;
using App_Web.Models.VM;
using Business_Logic.Service;
using Data.Infraestructure;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace App_Web.Controllers
{
    public class MesaController : Controller
    {
        private readonly MesaService mesaservice;
        private readonly ConfiReservaService confireservice;

        public MesaController(MesaService mesa, ConfiReservaService confi)
        {
            mesaservice = mesa;
            confireservice = confi;
        }

        public IActionResult Index()
        {

            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var precio = confireservice.DetallePrecioReserva();

            ViewBag.PrecioReserva = precio.ToViewModel();
            var listado = mesaservice.ListadoMesa();

            return View(listado.Select(m => m.ToViewModel()));
        }

        [HttpPost]
        public IActionResult GestionarMesa(MesaVM model)
        {
            mesaservice.GestionarMesa(model.ToEntity());

            return RedirectToAction("Index", "Mesa");
        }

        [HttpGet]
        public JsonResult DetalleMesa(int id)
        {
            var mesa = mesaservice.Detalle(id).ToViewModel();

            return Json(mesa);
        }

        [HttpPost]
        public IActionResult ActualizarPrecioReserva(decimal precio)
        {
            confireservice.ActualizarPrecioReserva(precio);

            return RedirectToAction("Index", "Mesa");
        }
    }
}
