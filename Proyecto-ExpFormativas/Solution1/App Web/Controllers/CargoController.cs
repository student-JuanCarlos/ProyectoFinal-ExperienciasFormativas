using Microsoft.AspNetCore.Mvc;

namespace App_Web.Controllers
{
    public class CargoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
