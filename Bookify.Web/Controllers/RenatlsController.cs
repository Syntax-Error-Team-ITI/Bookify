using Bookify.Web.Core.ViewModels.Rentals;
using System.Security.Claims;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Reception)]
    public class RenatlsController : Controller
    {
        public RentalRepository RentalRepo { get; }
        public RentalCopiesRepository RentalCopiesRepo { get; }

        public RenatlsController(RentalRepository rentalRepo, RentalCopiesRepository rentalCopiesRepo)
        {
            RentalRepo = rentalRepo;
            RentalCopiesRepo = rentalCopiesRepo;
        }


        public IActionResult Create(String sKey)
        {
            var ViewModel = new RentalFormViewModel
            {
                subscriberKey = sKey
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsDeleted(int id)
        {
            Rental rental = RentalRepo.GetById(id);
            if (rental is null || rental.CreatedOn.Date != DateTime.Today)
                return NotFound();
            rental.IsDeleted = true;
            rental.LastUpdatedOn = DateTime.UtcNow;
            rental.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            RentalRepo.Save();
            var rentalCopiesCount = RentalCopiesRepo.Count(id);

            return Ok(rentalCopiesCount);
        }
    }
}
