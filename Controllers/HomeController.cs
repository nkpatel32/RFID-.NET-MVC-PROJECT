using Microsoft.AspNetCore.Mvc;

namespace RFIDSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
