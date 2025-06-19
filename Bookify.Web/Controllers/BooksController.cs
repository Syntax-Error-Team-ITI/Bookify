using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Dynamic.Core;
namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BooksRepository _bookRepo;
        private readonly CategoriesRepository _categoryRepo;
        private readonly AuthorRepository _AuthorRepo;
        private readonly IMapper _mapper;
        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;


        public BooksController(BooksRepository bookRepo, IMapper mapper, IWebHostEnvironment webHostEnvironment, CategoriesRepository categoryRepo, AuthorRepository authorRepo)
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _categoryRepo = categoryRepo;
            _AuthorRepo = authorRepo;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var book = _bookRepo.GetByIdWithAllRelations(id);
            if (book is null)
                return NotFound();
            BookViewModel bookvm = _mapper.Map<BookViewModel>(book);
            bookvm.Categories = book.Categories.Select(b => b.Category!.Name).ToList();
            return View(bookvm);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetBooks()
        {
            try
            {
                var skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                // Get search value from DataTables
                var searchValue = Request.Form["search[value]"];

                // sort 
                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                // Get data from repository
                var books = _bookRepo.GetBooksWith();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    var search = searchValue.ToString().ToLower();
                    books = books.Where(b => b.Title.ToLower().Contains(search) || b.Author.Name.ToLower().Contains(search));
                }
                books = books.OrderBy($"{sortColumn} {sortColumnDirection}");
                var data = books.Skip(skip).Take(pageSize).ToList();
                var mappedData = _mapper.Map<IEnumerable<BookListVM>>(data);
                var recordsTotal = books.Count();

                var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = mappedData };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            try
            {
                var book = _bookRepo.GetById(id);

                if (book is null)
                    return NotFound();

                book.IsDeleted = !book.IsDeleted;
                book.LastUpdatedOn = DateTime.Now;

                _bookRepo.Save();

                return Ok(new { success = true, message = "Status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
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
                    ModelState.AddModelError(nameof(model.Image), Errors.NotAllowedExtension);
                    return View("Form", RenderBookFormViewModel(model));
                }
                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaxSize);
                    return View("Form", RenderBookFormViewModel(model));
                }
                var imageName = $"{Guid.NewGuid().ToString()}{extension}";
                book.ImageUrl = $"/images/books/{imageName}";
                book.ImageThumbnailUrl = $"/images/books/thumb/{imageName}";
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books", imageName);
                var thumbPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books/thumb", imageName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                using (var image = Image.Load(model.Image.OpenReadStream()))
                {
                    var ratio = (float)image.Width / 200;
                    var height = image.Height / ratio;
                    image.Mutate(i => i.Resize(width: 200, height: (int)height));
                    image.Save(thumbPath);
                }
            }

            foreach (var category in model.SelectedCategories)
                book.Categories.Add(new() { CategoryId = category });

            _bookRepo.Add(book);
            _bookRepo.Save();
            return RedirectToAction(nameof(Details), new { id = book.Id });

        }

        public IActionResult Edit(int id)
        {
            var book = _bookRepo.GetByIdWithAllRelations(id);
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

            var book = _bookRepo.GetByIdWithAllRelations(model.Id);
            if (book == null)
                return NotFound();

            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    var oldImagePath = $"{_webHostEnvironment.WebRootPath}{book.ImageUrl}";
                    var oldThumbPath = $"{_webHostEnvironment.WebRootPath}{book.ImageThumbnailUrl}";

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    if (System.IO.File.Exists(oldThumbPath))
                    {
                        System.IO.File.Delete(oldThumbPath);
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
                model.ImageUrl = $"/images/books/{imageName}";
                model.ImageThumbnailUrl = $"/images/books/thumb/{imageName}";
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books", imageName);
                var thumbPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books/thumb", imageName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                using (var image = Image.Load(model.Image.OpenReadStream()))
                {
                    var ratio = (float)image.Width / 200;
                    var height = image.Height / ratio;
                    image.Mutate(i => i.Resize(width: 200, height: (int)height));
                    image.Save(thumbPath);
                }
            }
            else if (model.ImageUrl is null)
            {
                model.ImageUrl = book.ImageUrl;
                model.ImageThumbnailUrl = book.ImageThumbnailUrl;
            }

            book = _mapper.Map(model, book);
            book.LastUpdatedOn = DateTime.Now;

            foreach (var category in model.SelectedCategories)
                book.Categories.Add(new BookCategory() { CategoryId = category });

            _bookRepo.Save();
            return RedirectToAction(nameof(Details), new { id = book.Id });
        }

        public IActionResult AllowItem(BookFormVM model)
        {
            return Json(!_bookRepo.CheckDuplicatedTitleAuthor(model));
        }
        private BookFormVM RenderBookFormViewModel(BookFormVM? model = null)
        {
            BookFormVM viewModel = model is null ? new BookFormVM() : model;
            var authors = _AuthorRepo.GetNotDeleted().OrderBy(a => a.Name);
            var categories = _categoryRepo.GetNotDeleted().OrderBy(a => a.Name);

            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            return viewModel;
        }
    }
}
