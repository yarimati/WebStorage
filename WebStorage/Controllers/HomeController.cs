using Microsoft.AspNetCore.Mvc;

namespace WebStorage.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
