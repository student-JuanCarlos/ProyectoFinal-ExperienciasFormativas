using Business_Logic.Service;
using Microsoft.AspNetCore.Mvc;

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

        
    }
}
