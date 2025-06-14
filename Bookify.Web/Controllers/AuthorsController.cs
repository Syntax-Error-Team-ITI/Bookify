using Bookify.Web.Core.ViewModels.Author;

namespace Bookify.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IMapper mapper;
        private readonly AuthorRepository authorRepository;
        public AuthorsController(IMapper _mapper, AuthorRepository _authorRepository)
        {
            mapper = _mapper;
            authorRepository = _authorRepository;
        }
        public IActionResult Index()
        {
            return View(authorRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Form");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var author = mapper.Map<Author>(model);
            authorRepository.Add(author);
            authorRepository.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            var model = mapper.Map<AuthorFormViewModel>(author);
            return View("Form", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var author = authorRepository.GetById(model.Id);
            if (author == null)
            {
                return NotFound();
            }
            author.Name = model.Name;
            author.LastUpdatedOn = DateTime.Now;
            authorRepository.Update(author);
            authorRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var author = authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }


            author.LastUpdatedOn = DateTime.Now;
            author.IsDeleted = true;
            authorRepository.Update(author);
            authorRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Retrieve(int id)
        {
            var author = authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }

            author.LastUpdatedOn = DateTime.Now;
            author.IsDeleted = false;
            authorRepository.Update(author);
            authorRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult IsAllowed(AuthorFormViewModel model)
        {

            var author = authorRepository.GetAll().SingleOrDefault(c => c.Name == model.Name);
            var isAllowed = author == null || author.Id.Equals(model.Id);

            return Json(isAllowed);
        }
    }
}
