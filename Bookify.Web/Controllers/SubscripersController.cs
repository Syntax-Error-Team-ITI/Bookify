using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    public class SubscripersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
