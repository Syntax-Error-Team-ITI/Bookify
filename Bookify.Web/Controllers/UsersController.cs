using Bookify.Web.Core.ViewModels.Category;
using Bookify.Web.Core.ViewModels.User;
using Bookify.Web.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class UsersController : Controller
    {
        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper,
            RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            this.mapper = mapper;
            RoleManager = roleManager;
        }

        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly IMapper mapper;

        public async Task<IActionResult> Index()
        {
            var users = await UserManager.Users.ToListAsync();
            var viewModel = mapper.Map<IEnumerable<UserViewModel>>(users);
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var viewModel = new UserAddViewModel
            {
                Roles = await RoleManager.Roles
                                .Select(r => new SelectListItem
                                {
                                    Text = r.Name,
                                    Value = r.Name
                                })
                                .ToListAsync()
            };

            return View("_Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if(model.Roles == null)
                {
                    model.Roles = await RoleManager.Roles
                                .Select(r => new SelectListItem
                                {
                                    Text = r.Name,
                                    Value = r.Name
                                })
                                .ToListAsync();
                }
                return View("_Form", model);
            }

            ApplicationUser user = new()
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await UserManager.AddToRolesAsync(user, model.SelectedRoles);

                var users = await UserManager.Users.ToListAsync();
                var viewModel = mapper.Map<IEnumerable<UserViewModel>>(users);
                return View("Index",viewModel);
            }

            if (model.Roles == null)
            {
                model.Roles = await RoleManager.Roles
                            .Select(r => new SelectListItem
                            {
                                Text = r.Name,
                                Value = r.Name
                            })
                            .ToListAsync();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return View("_Form", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            UserAddViewModel model = mapper.Map<UserAddViewModel>(user);
            model.Roles = await RoleManager.Roles
                            .Select(r => new SelectListItem
                            {
                                Text = r.Name,
                                Value = r.Name
                            })
                            .ToListAsync();
            return View("_Form", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await RoleManager.Roles
                            .Select(r => new SelectListItem
                            {
                                Text = r.Name,
                                Value = r.Name
                            })
                            .ToListAsync();
                return View("_Form", model);
            }

            var existingUser = await UserManager.FindByIdAsync(model.Id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Map the updated values onto the existing user
            mapper.Map(model, existingUser);
            existingUser.LastUpdatedOn = DateTime.Now;

            var result = await UserManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                // Handle the error case
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                model.Roles = await RoleManager.Roles
                            .Select(r => new SelectListItem
                            {
                                Text = r.Name,
                                Value = r.Name
                            })
                            .ToListAsync();
                return View("_Form", model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.LastUpdatedOn = DateTime.Now;
            user.IsDeleted = true;
            await UserManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Retrieve(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.LastUpdatedOn = DateTime.Now;
            user.IsDeleted = false;
            await UserManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}
