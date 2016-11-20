using Microsoft.AspNetCore.Mvc;

namespace Mvc.Sample.Controllers
{
    [Route("[controller]")]
    public class AttributeController : Controller
    {
        [Route("[action]")]
        public IActionResult Index()
        {
            ViewData["Message"] = Request.Path.Value;
            return View();
        }

        [Route("[action]/{id}")]
        public IActionResult Index(int id)
        {
            ViewData["Message"] = Request.Path.Value;
            return View();
        }
    }
}