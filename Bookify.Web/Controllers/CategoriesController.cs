using AutoMapper;
using Bookify.Web.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IMapper mapper;
        private readonly CategoriesRepository categoriesRepository;
        public CategoriesController(IMapper _mapper,CategoriesRepository _categoriesRepository)
        {
            mapper = _mapper;
            categoriesRepository = _categoriesRepository;
        }
        public IActionResult Index()
        {
            return View(categoriesRepository.GetAll());
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            return View();
        }
    }
}
