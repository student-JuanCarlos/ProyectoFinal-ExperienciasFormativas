using App_Web.Models.Extension;
using App_Web.Models.VM;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Business_Logic.Service;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace App_Web.Controllers
{
    public class PlatilloController : Controller
    {
        private readonly PlatilloService platilloservice;
        private readonly CategoriaService categoriaService;
        private readonly BlobContainerClient _blobContainer;
        private readonly ILogger<PlatilloController> _logguer;

        public PlatilloController(PlatilloService platillo, CategoriaService categoria, BlobContainerClient blob, ILogger<PlatilloController> logguer)
        {
            platilloservice = platillo;
            categoriaService = categoria;
            _blobContainer = blob;
            _logguer = logguer;
        }

        public IActionResult Index(int page = 1, string Busqueda = null)
        {

            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.Categorias = categoriaService.ListadoCategoria(null).Select(c => c.ToViewModel()).ToList();

            var listado = platilloservice.ListadoPlatillo(Busqueda).Select(p => p.ToViewModel()).ToList();

            int registrosPorPagina = 6;
            int totalPlatillos = listado.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalPlatillos / registrosPorPagina));

            int paginasPorOmitir = registrosPorPagina * (page - 1);

            ViewBag.Paginas = cantidadPaginas;
            ViewBag.PaginaActual = page;

            return View(listado.Skip(paginasPorOmitir).Take(registrosPorPagina));
        }

        [HttpPost]
        public async Task<IActionResult> GestionarPlatillo(PlatilloVM platillo)
        {
            string urlImagen = platillo.FotoActual ?? "";

            try
            {
                if(platillo.Fotografia != null)
                {
                    var nombreBlob = $"{Guid.NewGuid()}{Path.GetExtension(platillo.Fotografia.FileName)}";
                    var blobClient = _blobContainer.GetBlobClient(nombreBlob);

                    using var stream = platillo.Fotografia.OpenReadStream();

                    var opcionesContent = new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = platillo.Fotografia.ContentType // el navegador dara png. jpeg, etc...
                        }
                    };

                    await blobClient.UploadAsync(stream, opcionesContent);

                    urlImagen = blobClient.Uri.ToString();
                }
                if(platillo.IdPlatillo != 0 && !string.IsNullOrEmpty(platillo.FotoActual))
                {
                    var nombreAnterior = Path.GetFileName(new Uri(platillo.FotoActual).LocalPath);
                    await _blobContainer.DeleteBlobIfExistsAsync(nombreAnterior);
                }

                platillo.FotoActual = urlImagen;
                platilloservice.GestionarPlatillo(platillo.ToEntity());
            }
            catch(Exception ex)
            {
                _logguer.LogError(ex, "Fallo al gestionar el platillo {IdPlatillo}", platillo.IdPlatillo);
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
