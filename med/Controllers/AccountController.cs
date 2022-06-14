using med.Data;
using med.Data.Migrations;
using med.Models;
using med.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace med.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public AccountController(UserManager<ApplicationUser> userManager, 
                                    SignInManager<ApplicationUser> signInManager,
                                    ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    EmailConfirmed = true,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (_signInManager.IsSignedIn(User) && (User.IsInRole("Администратор") || User.IsInRole("Администратор (контрагент)")))
                    {
                        return RedirectToAction("CreateProfile", "Account", new {id = user.Id});
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }

            }

            return View(model);
        }

        [HttpGet]
        public  IActionResult CreateProfile(string id)
        
        {
            ViewData["user_id"] = id;

            if (User.IsInRole("Администратор"))
            {
                ViewData["EmployeePositionId"] = new SelectList(
                    _dbContext.EmployeePosition, "IdEmployeePosition", "Name");
                return View("CreateProfile");
            }
            else if (User.IsInRole("Администратор (контрагент)"))
            {
                ViewData["ClientPositionId"] = new SelectList(
                    _dbContext.ClientPosition, "IdClientPosition", "Name");

                return View("CreateProfileClient");

            }
            else return new ForbidResult();

        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateEmployeeViewModel model, string userId)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var profile = new Employee
                {
                    ApplicationUserId = userId,
                    Email = model.Email,
                    Fio = model.Fio,
                    EmployeePositionId = model.EmployeePositionId,
                    PhoneNumber = model.PhoneNumber
                };
                _dbContext.Add(profile);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("ListUsers", "Administration");
            }
            ViewData["EmployeePositionId"] = new SelectList(
                _dbContext.EmployeePosition, "IdEmployeePosition", "Name", model.EmployeePositionId); 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfileClient(CreateClientViewModel model, string userId)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var profile = new Client
                {
                    ApplicationUserId = userId,
                    Email = model.Email,
                    Fio = model.Fio,
                    ClientPositionId = model.ClientPositionId,
                    PhoneNumber = model.PhoneNumber,
                    OrganizationId = 1
                };
                _dbContext.Add(profile);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("ListClients", "Administration", new {id = "1"});
            }
            ViewData["ClientPositionId"] = new SelectList(
                _dbContext.ClientPosition, "IdClientPosition", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
                    model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                
                ModelState.AddModelError("", "Invalid login attempt.");


            }

            return View(model);
        }

    }
}
