using Microsoft.AspNetCore.Mvc;

namespace SezBot3000v2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hi there. This is telega bot Sez3000.");
        }
    }
}
