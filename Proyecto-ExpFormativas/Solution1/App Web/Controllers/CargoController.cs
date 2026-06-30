using App_Web.Models.Extension;
using App_Web.Models.VM;
using Business_Logic.Service;
using Microsoft.AspNetCore.Mvc;

namespace App_Web.Controllers
{
    public class CargoController : Controller
    {
        private readonly CargoService cargoService;

        public CargoController(CargoService cargo)
        {
            cargoService = cargo;
        }

        public IActionResult Index(string Busqueda)
        {

            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var listado = cargoService.ListadoCargo(Busqueda).Select(c => c.ToViewModel());

            return View(listado);
        }

        [HttpPost]
        public IActionResult GestionarCargo(CargoVM cargo)
        {
            cargoService.GestionarCargo(cargo.ToEntity());

            return RedirectToAction("Index", "Cargo");
        }

        [HttpGet]
        public JsonResult Detalle(int id)
        {
            var cargo = cargoService.Detalle(id).ToViewModel();

            return Json(cargo);
        }
    }
}
