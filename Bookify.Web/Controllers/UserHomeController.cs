using Bookify.Web.Core.ViewModels.BookCopies;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace Bookify.Web.Controllers
{
    public class UserHomeController : Controller
    {
        private IMapper _mapper;
        private BooksRepository _booksRepo;
        private RentalCopiesRepository _rentalCopiesRepository;
        public UserHomeController(IMapper mapper , BooksRepository booksRepository , RentalCopiesRepository rentalCopiesRepository)
        {
            this._mapper = mapper;
            this._booksRepo = booksRepository;
            this._rentalCopiesRepository = rentalCopiesRepository;
        }
        public IActionResult Index(string? search = null)
        {
            var books = _booksRepo.GetBooksWithIncludes(search);

            var bookVMs = _mapper.Map<List<BookWithAvailabilityVM>>(books);

            foreach (var book in bookVMs)
            {
                book.AvailableCopies = book.Copies.Count(copy =>
                    copy.IsAvailableForRental &&
                    !_rentalCopiesRepository.IsCopyInRental(copy.Id));
            }

            return View(bookVMs);
        }


    }
}
