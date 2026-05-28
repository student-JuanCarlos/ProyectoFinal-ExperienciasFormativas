using App_Web.Models.Extension;
using App_Web.Models.VM;
using Business_Logic.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App_Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteService clienteService;

        public ClienteController(ClienteService cliente)
        {
            clienteService = cliente;
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
        public IActionResult MiCuenta()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GestionarCliente(ClienteVM model)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                string nombreImagen = model.FotoActual ?? "";

                if (model.Fotografia != null)
                {
                    if (model.IdCliente != 0 && !string.IsNullOrEmpty(model.FotoActual))
                    {
                        var fotoAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.FotoActual);
                        if (System.IO.File.Exists(fotoAnterior))
                            System.IO.File.Delete(fotoAnterior);
                    }

                    var nombreRealImagen = Path.GetFileName(model.Fotografia.FileName);
                    nombreImagen = $"assets/img/productos/{nombreRealImagen}";
                    var pathImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/clientes", nombreRealImagen);

                    using (var stream = new FileStream(pathImagen, FileMode.Create))
                    {
                        model.Fotografia.CopyTo(stream);
                    }
                }

                model.FotoActual = $"{nombreImagen}";

                clienteService.GestionarCliente(model.ToEntity());
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        [HttpPost]
        public IActionResult ObtenerSesion()
        {
            var json = HttpContext.Session.GetString("Cliente");
            var cliente = JsonConvert.DeserializeObject<App_Web.Models.VM.ClienteVM>(json);

            var clientelogueado = clienteService.Detalle(cliente.IdCliente);

            return Json(clientelogueado);
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

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Cliente");
        }

    }
}
