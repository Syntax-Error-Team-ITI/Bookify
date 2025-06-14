using AutoMapper;
using Bookify.Web.Core.Models;
using Bookify.Web.Core.ViewModels.BookCopies;
using Bookify.Web.Core.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    public class BookCopiesController : Controller
    {
        private readonly BookCopyRepository repo;
        private readonly BooksRepository booksRepository;
        private readonly IMapper mapper;

        public BookCopiesController(BookCopyRepository repo , BooksRepository booksRepository  , IMapper mapper)
        {
            this.repo = repo;
            this.booksRepository = booksRepository;
            this.mapper = mapper;
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var copy = repo.GetById(id);
            if (copy == null)
                return NotFound();
            copy.LastUpdatedOn = DateTime.Now;
            copy.IsDeleted = true;
            repo.Save();
            return RedirectToAction("Details" , "Books" , new { id = copy.BookId });
        }
        public IActionResult Retrieve(int id)
        {
            var copy = repo.GetById(id);
            if (copy == null)
                return NotFound();
            copy.LastUpdatedOn = DateTime.Now;
            copy.IsDeleted = false;
            repo.Save();
            return RedirectToAction("Details", "Books" , new {id = copy.BookId});
        }
        public IActionResult Create(int bookId)
        {
            var book = booksRepository.GetById(bookId);
            if (book == null)
                return NotFound();
            var viewModel = new BookCopyFormViewModel 
            { 
                BookId = bookId,
                ShowRentalInput = book.IsAvailableForRental
            };
            return View("Form" , viewModel);
        }
        [HttpPost , ValidateAntiForgeryToken]
        public IActionResult Create(BookCopyFormViewModel vmodel)
        {
            if (!ModelState.IsValid) 
                return BadRequest();
            var book = booksRepository.GetById(vmodel.BookId);
            if (book == null)
                return NotFound();
            BookCopy copy = new BookCopy
            {
                EditionNumber = vmodel.EditionNumber,
                IsAvailableForRental = book.IsAvailableForRental && vmodel.IsAvailableForRental,
            };
            book.Copies.Add(copy);
            repo.Save();
            return RedirectToAction("Details", "Books", new { id = copy.BookId });
        }
        public IActionResult Update(int id)
        {
            var copy = repo.GetByIdWithBook(id);
            if (copy == null)
                return NotFound();
            var viewModel = mapper.Map<BookCopyFormViewModel>(copy);
            viewModel.ShowRentalInput = copy.Book!.IsAvailableForRental;
            return View("Form", viewModel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(BookCopyFormViewModel vmodel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var copy = repo.GetByIdWithBook(vmodel.Id);
            if (copy == null)
                return NotFound();
            copy.EditionNumber = vmodel.EditionNumber;
            copy.IsAvailableForRental = copy.Book!.IsAvailableForRental && vmodel.IsAvailableForRental;
            copy.LastUpdatedOn = DateTime.Now;
            repo.Save();
            return RedirectToAction("Details", "Books", new { id = copy.BookId });
        }
    }
}
