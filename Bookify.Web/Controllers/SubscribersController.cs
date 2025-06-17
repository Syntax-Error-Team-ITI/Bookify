using AutoMapper;
using Bookify.Web.Core.Models;
using Bookify.Web.Core.ViewModels;
using Bookify.Web.Core.ViewModels.Subscription;
using Bookify.Web.Repository;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Security.Claims;

namespace Bookify.Web.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SubscribersRepository _subscriberRepo;
        private readonly AreaRepository _areaRepo;
        private readonly GovernorateRepository _governorateRepo;
        private readonly IMapper _mapper;

        public SubscribersController(
            SubscribersRepository subscriberRepo,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            AreaRepository areaRepo,
            GovernorateRepository governorateRepo)
        {
            _areaRepo = areaRepo;
            _governorateRepo = governorateRepo;
            _subscriberRepo = subscriberRepo;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetSubscribers(int page = 1, int recordsNum = 10, string search = "")
        {
            var subscribers = _subscriberRepo.Pagination(page, recordsNum, search);
            var recordsTotal = _subscriberRepo.RecordCount(search);

            ViewBag.TotalPages = (int)Math.Ceiling((double)recordsTotal / recordsNum);
            ViewBag.CurrentPage = page;

            var vm = _mapper.Map<List<SubscriberVM>>(subscribers);

            return PartialView("_SubscribersTable", vm);
        }

      /*
        public IActionResult Details (string id)
        {
            var subscriberId = int.Parse(_dataProtector.Unprotect(id));
            var subscriber = _context.Subscripers
                .include (s => s.governorate)
                .include (s => s.Area)
                .include (s => s.Subscribtions)
                .include (s => s.Rentals)
                .ThenInclude(r => r.RentalCopies)
                .SingleOrDefault (s => s.Id == subscriberId);
            if (subscriber is null) 
                return NotFound();
            var ViewModel = _mapper.Map<SubscriberVM>(subscriber);
            ViewModel.Key = id;
            return View(BookViewModel);
        }

        */
        public IActionResult Details(int id)
        {
            var subscriber = _subscriberRepo.GetByIdWithAreaAndGovernorate(id);

            if (subscriber is null)
                return NotFound();

            var subscriberVM = _mapper.Map<SubscriberVM>(subscriber);

            subscriberVM.FullName = $"{subscriber.FirstName} {subscriber.LastName}";
            subscriberVM.Area = subscriber.Area?.Name;
            subscriberVM.Governorate = subscriber.Area?.Governorate?.Name;
            foreach (var subscription in subscriberVM.Subscriptions)
            {
                subscription.IsBlackListed = subscriber.IsBlackListed;
            }
            return View(subscriberVM);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new SubscriberFormVM
            {
                Governorates = _mapper.Map<IEnumerable<SelectListItem>>(_governorateRepo.GetAll()),
                Areas = new List<SelectListItem>()
            };

            return View("Form", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubscriberFormVM model)
        {
            
            if (!ModelState.IsValid)
            {
                model.Governorates = _mapper.Map<IEnumerable<SelectListItem>>(_governorateRepo.GetAll());
                model.Areas = _mapper.Map<IEnumerable<SelectListItem>>(_areaRepo.GetNotDeleted().Where(a => a.GovernorateId == model.GovernorateId));
                return View("Form", model);
            }

            var subscriber = _mapper.Map<Subscriber>(model);

            if (model.Image is not null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "subscribers");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                subscriber.ImageUrl = $"/images/subscribers/{uniqueFileName}";
                subscriber.ImageThumbnailUrl = subscriber.ImageUrl;
            }
            Subscription subscription = new()
            {
                CreatedById = subscriber.CreatedById,
                CreatedOn = subscriber.CreatedOn,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1)
            };

            subscriber.Subscriptions.Add(subscription);

            _subscriberRepo.Add(subscriber);
            _subscriberRepo.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subscriber = _subscriberRepo.GetById(id);
            if (subscriber == null)
                return NotFound();

            var model = _mapper.Map<SubscriberFormVM>(subscriber);

            model.Governorates = _mapper.Map<IEnumerable<SelectListItem>>(_governorateRepo.GetAll());
            model.Areas = _mapper.Map<IEnumerable<SelectListItem>>(
                _areaRepo.GetNotDeleted().Where(a => a.GovernorateId == subscriber.Area.GovernorateId));

            return View("Form", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SubscriberFormVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Governorates = _mapper.Map<IEnumerable<SelectListItem>>(_governorateRepo.GetAll());
                model.Areas = _mapper.Map<IEnumerable<SelectListItem>>(
                    _areaRepo.GetNotDeleted().Where(a => a.GovernorateId == model.GovernorateId));
                return View("Form", model);
            }

            var subscriber = _subscriberRepo.GetById(id);
            if (subscriber == null)
                return NotFound();

            subscriber.FirstName = model.FirstName;
            subscriber.LastName = model.LastName;
            subscriber.DateOfBirth = model.DateOfBirth;
            subscriber.NationalId = model.NationalId;
            subscriber.MobileNumber = model.MobileNumber;
            subscriber.HasWhatsApp = model.HasWhatsApp;
            subscriber.Email = model.Email;
            subscriber.AreaId = model.AreaId;
            subscriber.Address = model.Address;

            if (model.Image is not null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "subscribers");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(subscriber.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, subscriber.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                subscriber.ImageUrl = $"/images/subscribers/{uniqueFileName}";
                subscriber.ImageThumbnailUrl = subscriber.ImageUrl;
            }

            _subscriberRepo.Update(subscriber);
            _subscriberRepo.Save();

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenewSubscription(int id)
        {
            var subscriber = _subscriberRepo.GetByIdWithAreaAndGovernorate(id);

            if (subscriber is null)
                return NotFound();

            if (subscriber.IsBlackListed)
                return BadRequest();

            var startDate = subscriber.Subscriptions.Any() && subscriber.Subscriptions.Last().EndDate >= DateTime.Today
                            ? subscriber.Subscriptions.Last().EndDate.AddDays(1)
                            : DateTime.Today;

            Subscription newSubscription = new()
            {
                CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                CreatedOn = DateTime.Now,
                StartDate = startDate,
                EndDate = startDate.AddYears(1)
            };

            subscriber.Subscriptions.Add(newSubscription);
            _subscriberRepo.Save();

           
            return RedirectToAction(nameof(Details), new { id });

        }





        [HttpGet]
        public IActionResult GetAreasByGovernorate(int governorateId)
        {
            var areas = _areaRepo.GetAreasByGovernorateId(governorateId);
            var areaList = _mapper.Map<IEnumerable<SelectListItem>>(areas);
            return Json(areaList);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult AllowEmail(string email, int id)
        {
            var exists = _subscriberRepo.IsEmailExists(email, id);
            return Json(!exists);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult AllowNationalId(string nationalId, int id)
        {
            var exists = _subscriberRepo.IsNationalIdExists(nationalId, id);
            return Json(!exists);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult AllowMobileNumber(string mobileNumber, int id)
        {
            var exists = _subscriberRepo.IsMobileNumberExists(mobileNumber, id);
            return Json(!exists);
        }

        public IActionResult Block(int Id)
        {
            var subscriber = _subscriberRepo.GetById(Id);
            if (subscriber == null)
                return NotFound();

            subscriber.LastUpdatedOn = DateTime.Now;
            subscriber.IsBlackListed = true;

            _subscriberRepo.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Unblock(int Id)
        {
            var subscriber = _subscriberRepo.GetById(Id);
            if (subscriber == null)
                return NotFound();

            subscriber.LastUpdatedOn = DateTime.Now;
            subscriber.IsBlackListed = false;

            _subscriberRepo.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
