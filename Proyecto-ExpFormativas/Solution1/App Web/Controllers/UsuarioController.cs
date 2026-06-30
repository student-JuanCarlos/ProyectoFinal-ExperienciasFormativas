using App_Web.Models.Extension;
using App_Web.Models.VM;
using Business_Logic.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace App_Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioService usuarioService;
        private readonly CargoService cargoService;
        private readonly RolService rolService;
        private readonly MesaService mesaService;
        private readonly DescuentoService descuentoService;

        public UsuarioController(UsuarioService usuario, CargoService cargo, RolService rol, MesaService mesa, DescuentoService descuento)
        {
            usuarioService = usuario;
            cargoService = cargo;
            rolService = rol;
            mesaService = mesa;
            descuentoService = descuento;
        }

        [HttpGet]
        public IActionResult Index(string Busqueda = null, bool? Estado = null)
        {

            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.Rol = rolService.Listado(null).Select(r => r.ToViewModel()).ToList();
            ViewBag.Cargo = cargoService.ListadoCargo(null).Select(c => c.ToViewModel()).ToList();

            var listado = usuarioService.ListadoUsuario(Busqueda, Estado).Select(u => u.ToViewModel()).ToList(); ;

            return View(listado);
        }

        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GestionarUsuario(UsuarioVM usuario)
        {
            usuarioService.GestionarUsuario(usuario.ToEntity());

            return RedirectToAction("Index", "Usuario");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Contraseña)
        {
            var usuario = usuarioService.Login(Email, Contraseña);

            if (usuario == null)
            {
                ViewBag.Error = "Email o Contraseña incorrecta";
                return View("Login");
            }
            if(usuario.Estado == false)
            {
                ViewBag.Error = "Su cuenta ha sido desactivada";
                return View("Login");
            }

            mesaService.ActualizarEstadoMesasHoy();
            descuentoService.ActualizarEstadoDescuentosHoy();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.rol.NombreRol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(usuario));
            HttpContext.Session.SetString("Rol", usuario.rol.NombreRol);

            return RedirectToAction("DashBoard", "Usuario");
        }

        [HttpGet]
        public JsonResult Detalle(int id)
        {
            var usuario = usuarioService.Detalle(id).ToViewModel();

            return Json(usuario);
        }

        [HttpPost]
        public IActionResult CambiarEstadoUsuario(int id)
        {
            usuarioService.CambiarEstado(id);

            return RedirectToAction("Index", "Usuario");
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Usuario");
        }

    }
}
