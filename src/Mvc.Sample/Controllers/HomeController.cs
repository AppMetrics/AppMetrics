using Microsoft.AspNetCore.Mvc;

namespace Mvc.Sample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TestParam(int id)
        {
            return View("About");
        }
    }
}