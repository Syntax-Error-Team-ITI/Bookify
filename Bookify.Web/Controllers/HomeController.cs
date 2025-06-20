using System.Diagnostics;
using Bookify.Web.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BooksRepository bookRepo;
        private readonly SubscribersRepository subscribersRepo;
        private readonly BookCopyRepository bookCopyRepo;
        private readonly CategoriesRepository categoriesRepository;

        public HomeController(ILogger<HomeController> logger,
            BooksRepository bookRepo,
            SubscribersRepository subscribersRepo,
            CategoriesRepository categoriesRepository
            , BookCopyRepository _copyRepo)
        {
            _logger = logger;
            this.bookRepo = bookRepo;
            this.subscribersRepo = subscribersRepo;
            this.categoriesRepository = categoriesRepository;
            bookCopyRepo = _copyRepo;
        }

        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Index", "UserHome");
            }
            var model = new HomeViewModel
            {
                AllBooks = bookRepo.GetAll().Where(b => b.IsDeleted == false),
                AllSubscribers = subscribersRepo.GetAll().Where(b => b.IsDeleted == false),
                AllBooksCount = bookCopyRepo.GetAll().Where(b => b.IsDeleted == false).Count(),
                AllCopies = bookCopyRepo.GetAllWithBooksAndAuthors().Where(b => b.IsDeleted == false),
                AllAvailableBooksCount = bookCopyRepo.GetAllWithBooksAndAuthors().Where(b => b.IsDeleted == false && b.IsAvailableForRental == true).Count(),
               AllRentalsCount = bookCopyRepo.GetAllWithBooksAndAuthors().Where(b => !b.IsDeleted && b.IsAvailableForRental == false).Count(),

                AllCategories = categoriesRepository.GetAll().Where(c => c.Books.Count() > 0)
            };
            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
