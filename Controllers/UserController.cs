using Microsoft.AspNetCore.Mvc;

namespace RFID_.NET_MVC_PROJECT.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserLogin()
        {
            return View("~/Views/UserPanal/UserLogin.cshtml");
        }

        public IActionResult UserRegister()
        {
            return View("~/Views/UserPanal/UserRegister.cshtml");
        }
    }
}
