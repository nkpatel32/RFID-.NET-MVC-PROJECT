using Microsoft.AspNetCore.Mvc;

namespace RFID_.NET_MVC_PROJECT.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult ClientLogin()
        {
            return View("~/Views/ClientPanal/ClientLogin.cshtml");
        }

        public IActionResult ClientRegister()
        {
            return View("~/Views/ClientPanal/ClientRegister.cshtml");
        }

    }
}
