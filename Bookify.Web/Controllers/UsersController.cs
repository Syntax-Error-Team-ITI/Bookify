using Bookify.Web.Core.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class UsersController : Controller
    {
        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            UserManager = userManager;
            this.mapper = mapper;
        }

        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IMapper mapper;

        public async Task<IActionResult> Index()
        {
            var users = await UserManager.Users.ToListAsync();
            var viewModel = mapper.Map<IEnumerable<UserViewModel>>(users);
            return View(viewModel);
        }

        public IActionResult Add()
        {
            return Content("Add");
        }
        public IActionResult Edit()
        {
            return Content("Edit");
        }
        public IActionResult Delete()
        {
            return Content("Delete");
        }
        public IActionResult Retrieve()
        {
            return Content("Delete");
        }
    }
}
