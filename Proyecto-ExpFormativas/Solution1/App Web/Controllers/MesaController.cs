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

        public IActionResult Index(string Busqueda = null)
        {
            var precio = confireservice.DetallePrecioReserva();

            ViewBag.PrecioReserva = precio.ToViewModel();
            var listado = mesaservice.ListadoMesa(Busqueda);

            return View(listado.Select(m => m.ToViewModel()));
        }

        [HttpPost]
        public IActionResult GestionarMesa(MesaVM model)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                mesaservice.GestionarMesa(model.ToEntity());
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

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

            bool resultado = true;
            string mensaje = "";
            try
            {
                confireservice.ActualizarPrecioReserva(precio);
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return RedirectToAction("Index", "Mesa");
        }
    }
}
