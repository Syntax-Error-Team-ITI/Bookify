using Bookify.Web.Core.ViewModels.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Reception)]
    public class RenatlsController : Controller
    {
        public IActionResult Create(String sKey)
        {
            var ViewModel = new RentalFormViewModel
            {
                subscriberKey = sKey
            };
            return View(ViewModel);
        }
    }
}
