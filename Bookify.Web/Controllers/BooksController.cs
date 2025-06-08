using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;

        public BooksController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View("Form", RenderBookFormViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormVM model)
        {
            if (!ModelState.IsValid)
                return View("Form", RenderBookFormViewModel(model));
            var book = _mapper.Map<Book>(model);

            if (model.Image is not null)
            {

                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image),Errors.NotAllowedExtension);
                    return View("Form", RenderBookFormViewModel(model));
                }
                if(model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image),Errors.MaxSize);
                    return View("Form", RenderBookFormViewModel(model));
                }
                var imageName = $"{Guid.NewGuid().ToString()}{extension}";
                book.ImageUrl = imageName;
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books",imageName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }

            foreach (var category in model.SelectedCategories)
                book.Categories.Add(new() { CategoryId = category });

            _context.Add(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Edit(int id)
        {
            var book = _context.Books.Include(b => b.Categories).SingleOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();
            var model = _mapper.Map<BookFormVM>(book);
            var viewModel = RenderBookFormViewModel(model);
            viewModel.SelectedCategories = book.Categories.Select(c => c.CategoryId).ToList();
            return View("Form", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookFormVM model)
        {
            if (!ModelState.IsValid)
                return View("Form", RenderBookFormViewModel(model));

            var book = _context.Books.Include(b => b.Categories).SingleOrDefault(b => b.Id == model.Id);
            if (book == null)
                return NotFound();

            if (model.Image is not null)
            {
                if(!string.IsNullOrEmpty(book.ImageUrl))
                {
                    var oldImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books", book.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath)){
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.NotAllowedExtension);
                    return View("Form", RenderBookFormViewModel(model));
                }
                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaxSize);
                    return View("Form", RenderBookFormViewModel(model));
                }
                var imageName = $"{Guid.NewGuid().ToString()}{extension}";
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books", imageName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                model.ImageUrl = imageName;
            }
            if (model.ImageUrl is null)
                model.ImageUrl = book.ImageUrl;

            book = _mapper.Map(model,book);
            book.LastUpdatedOn = DateTime.Now;

            foreach (var category in model.SelectedCategories)
                book.Categories.Add(new() { CategoryId = category });

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        private BookFormVM RenderBookFormViewModel(BookFormVM? model = null)
        {
            BookFormVM viewModel = model is null ? new BookFormVM() : model;
            var authors = _context.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            var categories = _context.Categories.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();

            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            return viewModel;
        }
    }
}
