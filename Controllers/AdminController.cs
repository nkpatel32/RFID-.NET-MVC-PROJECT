using Microsoft.AspNetCore.Mvc;

namespace RFID_.NET_MVC_PROJECT.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminLogin()
        {
            return View("~/Views/AdminPanal/AdminLogin.cshtml");
        }


    }
}
