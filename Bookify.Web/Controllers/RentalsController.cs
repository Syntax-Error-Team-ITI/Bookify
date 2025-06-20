using Bookify.Web.Core.Enums;
using Bookify.Web.Core.ViewModels.BookCopies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Reception)]
    public class RentalsController : Controller
    {
        private readonly IDataProtector _dataProtector;
        private readonly IMapper _mapper;
        private readonly RentalRepository _rentalRepo;
        private readonly RentalCopiesRepository _rentalCopyRepo;

        public RentalsController(
                    RentalRepository rentalRepo,
                    RentalCopiesRepository rentalCopyRepo,
                    IDataProtectionProvider dataProtector,
                    IMapper mapper)
        {
            _rentalRepo = rentalRepo;
            _rentalCopyRepo = rentalCopyRepo;
            _dataProtector = dataProtector.CreateProtector("MySecureKey");
            _mapper = mapper;
        }



        public IActionResult Details(int id)
        {
            var rental = _rentalRepo.GetWithCopiesAndBook(id);

            if (rental is null)
                return NotFound();

            var viewModel = _mapper.Map<RentalViewModel>(rental);

            return View(viewModel);
        }

        public IActionResult Create(int id)
        {

            var subscriber = _rentalRepo.GetSubscriberWithRentalsAndSubscriptions(id);
            
            if (subscriber is null)
                return NotFound();

            var (errorMessage, maxAllowedCopies) = ValidateSubscriber(subscriber);

            if (!string.IsNullOrEmpty(errorMessage))
                return View("NotAllowedRental", errorMessage);

            var viewModel = new RentalFormViewModel
            {
                SubscriberId = id,
                MaxAllowedCopies = maxAllowedCopies
            };

            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RentalFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var subscriberId = model.SubscriberId;

            var subscriber = _rentalRepo.GetSubscriberWithRentalsAndSubscriptions(model.SubscriberId);

            if (subscriber is null)
                return NotFound();

            var (errorMessage, maxAllowedCopies) = ValidateSubscriber(subscriber);

            if (!string.IsNullOrEmpty(errorMessage))
                return View("NotAllowedRental", errorMessage);

            var (rentalsError, copies) = ValidateCopies(model.SelectedCopies, subscriberId);

            if (!string.IsNullOrEmpty(rentalsError))
                return View("NotAllowedRental", rentalsError);

            Rental rental = new()
            {
                RentalCopies = copies,
                CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            };

            subscriber.Rentals.Add(rental);
            _rentalRepo.Save();

            return RedirectToAction(nameof(Details), new { id = rental.Id });
        }

        public IActionResult Edit(int id)
        {
            var rental = _rentalRepo.GetWithCopiesAndBook(id);

            if (rental is null || rental.CreatedOn.Date != DateTime.Today)
                return NotFound();

            var subscriber = _rentalRepo.GetSubscriberWithRentalsAndSubscriptions(rental.SubscriberId);

            if (subscriber is null)
                return NotFound();

            var (errorMessage, maxAllowedCopies) = ValidateSubscriber(subscriber, rental.Id, editMode: true);

            if (!string.IsNullOrEmpty(errorMessage))
                return View("NotAllowedRental", errorMessage);

            var currentCopies = rental.RentalCopies
                               .Select(rc => rc.BookCopy) 
                               .ToList();
            var viewModel = new RentalFormViewModel
            {
                Id = rental.Id,
                SubscriberId = subscriber.Id,
                MaxAllowedCopies = maxAllowedCopies,
                CurrentCopies = _mapper.Map<IEnumerable<BookCopyViewModel>>(currentCopies)
            };

            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RentalFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var rental = _rentalRepo.GetByIdWithCopies(model.Id);


            if (rental is null || rental.CreatedOn.Date != DateTime.Today)
                return NotFound();

            var subscriber = _rentalRepo.GetSubscriberWithRentalsAndSubscriptions(model.SubscriberId);

            if (subscriber is null)
            {
                ModelState.AddModelError("", "Subscriber not found.");
                return View("Form", model);
            }

            var (errorMessage, maxAllowedCopies) = ValidateSubscriber(subscriber, model.Id, editMode: true);

            if (!string.IsNullOrEmpty(errorMessage))
                return View("NotAllowedRental", errorMessage);

            var (rentalsError, copies) = ValidateCopies(model.SelectedCopies, subscriber.Id, rental.Id);

            if (!string.IsNullOrEmpty(rentalsError))
                return View("NotAllowedRental", rentalsError);

            rental.RentalCopies = copies;
            rental.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            rental.LastUpdatedOn = DateTime.Now;

            _rentalRepo.Save();

            return RedirectToAction(nameof(Details), new { id = rental.Id });
        }

        public IActionResult Return(int id)
        {
            var rental = _rentalRepo.GetWithCopiesAndBook(id);

            if (rental is null || rental.CreatedOn.Date == DateTime.Today)
                return NotFound();

            var subscriber = _rentalRepo.GetSubscriberWithSubscriptionsOnly(rental.SubscriberId);

            var viewModel = new RentalReturnFormViewModel
            {
                Id = id,
                Copies = _mapper.Map<IList<RentalCopyViewModel>>(rental.RentalCopies.Where(c => !c.ReturnDate.HasValue).ToList()),
                SelectedCopies = rental.RentalCopies.Where(c => !c.ReturnDate.HasValue).Select(c => new ReturnCopyViewModel { Id = c.BookCopyId, IsReturned = c.ExtendedOn.HasValue ? false : null }).ToList(),
                AllowExtend = !subscriber!.IsBlackListed
                    && subscriber!.Subscriptions.Last().EndDate >= rental.StartDate.AddDays((int)RentalConfigurations.MaxRentalDuration)
                    && rental.StartDate.AddDays((int)RentalConfigurations.RentalDuration) >= DateTime.Today
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Return(RentalReturnFormViewModel model)
        {
            var rental = _rentalRepo.GetWithCopiesAndBook(model.Id);

            if (rental is null)
                return NotFound();

            var rentalCopies = _rentalRepo.GetRentalCopiesWithoutReturn(model.Id);
            var mappedCopies = _mapper.Map<IList<RentalCopyViewModel>>(rentalCopies);
            model.Copies = mappedCopies;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var subscriber = _rentalRepo.GetSubscriberWithSubscriptionsOnly(rental.SubscriberId);

            if (subscriber is null)
                return NotFound();

            if (model.SelectedCopies.Any(c => c.IsReturned.HasValue && !c.IsReturned.Value))
            {
                string error = string.Empty;

                if (subscriber.IsBlackListed)
                    error = Errors.RentalNotAllowedForBlacklisted;
                else if (subscriber.Subscriptions.Last().EndDate < rental.StartDate.AddDays((int)RentalConfigurations.MaxRentalDuration))
                    error = Errors.RentalNotAllowedForInactive;
                else if (rental.StartDate.AddDays((int)RentalConfigurations.RentalDuration) < DateTime.Today)
                    error = Errors.ExtendNotAllowed;

                if (!string.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("", error);
                    return View(model);
                }
            }

            var isUpdated = false;

            foreach (var copy in model.SelectedCopies)
            {
                if (!copy.IsReturned.HasValue) continue;

                var currentCopy = rental.RentalCopies.SingleOrDefault(c => c.BookCopyId == copy.Id);
                if (currentCopy is null) continue;

                if (copy.IsReturned == true && !currentCopy.ReturnDate.HasValue)
                {
                    currentCopy.ReturnDate = DateTime.Now;
                    isUpdated = true;
                }
                else if (copy.IsReturned == false && !currentCopy.ExtendedOn.HasValue)
                {
                    currentCopy.ExtendedOn = DateTime.Now;
                    currentCopy.EndDate = currentCopy.RentalDate.AddDays((int)RentalConfigurations.MaxRentalDuration);
                    isUpdated = true;
                }
            }

            if (isUpdated)
            {
                rental.LastUpdatedOn = DateTime.Now;
                rental.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                rental.PenaltyPaid = model.PenaltyPaid;

                _rentalRepo.Save();
            }

            return RedirectToAction(nameof(Details), new { id = rental.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetCopyDetails(SearchFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var copy = _rentalRepo.GetBookCopyWithBookBySerial(model.Value);

            if (copy is null)
                return NotFound(Errors.InvalidSerialNumber);

            if (!copy.IsAvailableForRental || !copy.Book!.IsAvailableForRental)
                return BadRequest(Errors.NotAvilableRental);

            //check that copy is not in rental
            var copyIsInRental = _rentalRepo.IsCopyInRental(copy.Id);

            if (copyIsInRental)
                return BadRequest(Errors.CopyIsInRental);

            var viewModel = _mapper.Map<BookCopyViewModel>(copy);

            return PartialView("_CopyDetails", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsDeleted(int id)
        {
            var rental = _rentalRepo.GetById(id);

            if (rental is null || rental.CreatedOn.Date != DateTime.Today)
                return NotFound();

            rental.IsDeleted = true;
            rental.LastUpdatedOn = DateTime.Now;
            rental.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            _rentalRepo.Save();

            //var copiesCount = _context.RentalCopies.Count(r => r.RentalId == id);
            var copiesCount = _rentalCopyRepo.CountByRentalId(id);
            return Ok(copiesCount);
        }
 
        private (string errorMessage, int? maxAllowedCopies) ValidateSubscriber(Subscriber subscriber, int? rentalId = null, bool editMode = false)
        {
            if (subscriber.IsBlackListed)
                return (errorMessage: Errors.BlackListedSubscriber, maxAllowedCopies: null);

            if (subscriber.Subscriptions.Last().EndDate < DateTime.Today.AddDays((int)RentalConfigurations.RentalDuration))
                return (errorMessage: Errors.InactiveSubscriber, maxAllowedCopies: null);

            
            var currentRentals = subscriber.Rentals
                .Where(r => r.CreatedOn.Date == DateTime.Today && (!editMode || r.Id != rentalId))
                .SelectMany(r => r.RentalCopies)
                .Count(c => !c.ReturnDate.HasValue);

            var maxAllowed = (int)RentalConfigurations.MaxAllowedCopies;
            var availableCopiesCount = maxAllowed - currentRentals;

           
            if (!editMode && availableCopiesCount <= 0)
                return (errorMessage: Errors.MaxCopiesReached, maxAllowedCopies: null);

            return (errorMessage: string.Empty, maxAllowedCopies: availableCopiesCount);
        }


        private (string errorMessage, ICollection<RentalCopy> copies) ValidateCopies(IEnumerable<int> selectedSerials, int subscriberId, int? rentalId = null)
        {
           
            List<RentalCopy> copies = new();

            foreach (var serial in selectedSerials)
            {
                var copy = _rentalRepo.GetBookCopyWithBookBySerial(serial.ToString());

                if (copy is null)
                    return (Errors.InvalidSerialNumber, copies);

                if (!copy.IsAvailableForRental || !copy.Book!.IsAvailableForRental)
                    return (Errors.NotAvilableRental, copies);

                if (_rentalCopyRepo.IsCopyInRental(copy.Id, rentalId))

                    return (Errors.CopyIsInRental, copies);

                var currentSubscriberBookIds = _rentalRepo.GetSubscriberCurrentBookIds(subscriberId, rentalId);

                if (currentSubscriberBookIds.Contains(copy.BookId))
                    return ($"This subscriber already has a copy for '{copy.Book.Title}' Book", copies);

                copies.Add(new RentalCopy { BookCopyId = copy.Id });
            }

            return (string.Empty, copies);
        }
    }
}