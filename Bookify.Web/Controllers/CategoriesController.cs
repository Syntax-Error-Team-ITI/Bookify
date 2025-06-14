using AutoMapper;
using Bookify.Web.Core.ViewModels.Category;
using Bookify.Web.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bookify.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IMapper mapper;
        private readonly CategoriesRepository categoriesRepository;
        public CategoriesController(IMapper _mapper, CategoriesRepository _categoriesRepository)
        {
            mapper = _mapper;
            categoriesRepository = _categoriesRepository;
        }
        public IActionResult Index()
        {
            return View(categoriesRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Form");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var category = mapper.Map<Category>(model);
            categoriesRepository.Add(category);
            categoriesRepository.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = categoriesRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            var model = mapper.Map<CategoryFormViewModel>(category);
            return View("Form", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            var category = categoriesRepository.GetById(model.Id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = model.Name;
            category.LastUpdatedOn = DateTime.Now;
            categoriesRepository.Update(category);
            categoriesRepository.Save();
            //var categoryVM = mapper.Map<CategoryFormViewModel>(category);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = categoriesRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            category.LastUpdatedOn = DateTime.Now;
            category.IsDeleted = true;
           categoriesRepository.Update(category);
            categoriesRepository.Save();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Retrieve(int id)
        {
            var category = categoriesRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            category.LastUpdatedOn = DateTime.Now;
            category.IsDeleted = false;
           categoriesRepository.Update(category);
            categoriesRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult IsAllowed(CategoryFormViewModel model)
        {
            
            var category = categoriesRepository.GetAll().SingleOrDefault(c => c.Name == model.Name);
            var isAllowed = category == null|| category.Id.Equals(model.Id);

            return Json(isAllowed);
        }
    }
}
