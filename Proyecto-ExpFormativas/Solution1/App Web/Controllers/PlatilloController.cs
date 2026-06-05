using App_Web.Models.Extension;
using App_Web.Models.VM;
using Business_Logic.Service;
using Microsoft.AspNetCore.Mvc;

namespace App_Web.Controllers
{
    public class PlatilloController : Controller
    {
        private readonly PlatilloService platilloservice;
        private readonly CategoriaService categoriaService;

        public PlatilloController(PlatilloService platillo, CategoriaService categoria)
        {
            platilloservice = platillo;
            categoriaService = categoria;
        }

        public IActionResult Index(int page = 1, string Busqueda = null)
        {
            ViewBag.Categorias = categoriaService.ListadoCategoria(null).Select(c => c.ToViewModel()).ToList();

            var listado = platilloservice.ListadoPlatillo(Busqueda).Select(p => p.ToViewModel()).ToList();

            int registrosPorPagina = 10;
            int totalPlatillos = listado.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalPlatillos / registrosPorPagina));

            int paginasPorOmitir = registrosPorPagina * (page - 1);

            ViewBag.Paginas = cantidadPaginas;
            ViewBag.PaginaActual = page;

            return View(listado.Skip(paginasPorOmitir).Take(registrosPorPagina));
        }

        [HttpPost]
        public IActionResult GestionarPlatillo(PlatilloVM platillo)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                platilloservice.GestionarPlatillo(platillo.ToEntity());
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return RedirectToAction("Index", "Platillo");
        }

        [HttpGet]
        public JsonResult Detalle(int id)
        {
            var platillo = platilloservice.Detalle(id).ToViewModel();

            return Json(platillo);
        }
    }
}
