using App_Web.Models.Extension;
using App_Web.Models.VM;
using Business_Logic.Service;
using Microsoft.AspNetCore.Mvc;

namespace App_Web.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CategoriaService categoriaservice;

        public CategoriaController(CategoriaService categoria)
        {
            categoriaservice = categoria;
        }

        public IActionResult Index(string Busqueda)
        {
            var listado = categoriaservice.ListadoCategoria(Busqueda).Select(c => c.ToViewModel()).ToList();

            return View(listado);
        }

        [HttpPost]
        public IActionResult GestionarCategoria(CategoriaVM categoria)
        {
            categoriaservice.GestionarCategoria(categoria.ToEntity());

            return RedirectToAction("Index", "Categoria");
        }

        [HttpGet]
        public JsonResult Detalle(int id)
        {
            var categoria = categoriaservice.Detalle(id).ToViewModel();

            return Json(categoria);
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            categoriaservice.EliminarCategoria(id);

            return RedirectToAction("Index", "Categoria");
        }
    }
}
