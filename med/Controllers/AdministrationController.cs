using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using med.Data;
using med.Models;
using med.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

using OfficeOpenXml;

namespace med.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult ListUsers(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var users = _userManager.Users;
            var positions = _dbContext.EmployeePosition;
            var model = new List<EmployeeViewModel>();

            var employees = from s in _dbContext.Employee select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(x => x.Fio.Contains(searchString) || 
                                     x.EmployeePosition.Name.Contains(searchString) ||
                                     x.ApplicationUser.UserName.Contains(searchString) ||
                                     x.PhoneNumber.Contains(searchString) ||
                                     x.Email.Contains(searchString)
                                     );
            }

            foreach (var employee in employees)
            {
                var user = users.First(x => x.Id == employee.ApplicationUserId);
                var m = new EmployeeViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = employee.Email,
                    Fio = employee.Fio,
                    EmployeePositionName =
                        positions.FirstOrDefault(x => x.IdEmployeePosition == employee.EmployeePositionId)!.Name,
                    PhoneNumber = employee.PhoneNumber
                };
                model.Add(m);
            
            }
            
            ViewData["EmployeePosition"] = _dbContext.EmployeePosition;
            return View(model);
        }
        
        [HttpGet]
        public IActionResult ListClients(int id)
        {
            var users = _userManager.Users;
            var clients = _dbContext.Client!.Where(x => x.OrganizationId == id);

            var model = new List<ClientViewModel>();

            foreach (var client in clients)
            {
                var user = users.First(x => x.Id == client.ApplicationUserId);
                var m = new ClientViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = client.Email,
                    Fio = client.Fio,
                    ClientPositionName = _dbContext.ClientPosition!
                            .FirstOrDefault(x => x.IdClientPosition == client.ClientPositionId)!.Name,
                    PhoneNumber = client.PhoneNumber,
                    OrganizationName = _dbContext.Organization!
                            .FirstOrDefault(x => x.IdOrganization == client.OrganizationId)!.Name
                    };
                model.Add(m);

            }

            ViewData["EmployeePosition"] = _dbContext.EmployeePosition;
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError("", errors.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role not found";
                return NotFound();
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add( user.UserName);
                }
            }

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return NotFound();
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return NotFound();
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRole = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }
                model.Add(userRole);
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return NotFound();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected 
                    && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected
                         && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);

                }
                else {continue;}

                if (result.Succeeded)
                {
                    if (i < model.Count - 1)
                        continue;
                    else return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });

        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {

            ViewData["EmployeePositionId"] = new SelectList(
                _dbContext.EmployeePosition, "IdEmployeePosition", "Name");
            var user = await _userManager.FindByIdAsync(id);
            var profile = _dbContext.Employee.FirstOrDefault(e => e.ApplicationUserId == id);

            if (user == null || profile == null)
                return new NotFoundResult();

            var userRoles = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles.Where(x => x.Name == "Инженер" || x.Name == "Администратор" || x.Name == "Менеджер"))
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                userRoles.Add(userRolesViewModel);
            }


            var model = new EditUserViewModel
            {
                Id = user.Id,
                Roles = userRoles,
                UserName = user.UserName,
                Email = profile.Email,
                Fio = profile.Fio,
                EmployeePositionId = profile.EmployeePositionId,
                PhoneNumber = profile.PhoneNumber
            };
            if (profile.Address != null) model.Address = profile.Address;

            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var profile = _dbContext.Employee.FirstOrDefault(e => e.ApplicationUserId == model.Id);
            if (user == null || profile == null)
                return new NotFoundResult();
            
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Roles.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            {
                user.Email = model.Email;
                user.UserName = model.UserName;

                result = await _userManager.UpdateAsync(user);



                if (result.Succeeded)
                {
                    profile.Email = model.Email;
                    profile.Fio = model.Fio;
                    profile.EmployeePositionId = model.EmployeePositionId;
                    profile.PhoneNumber = model.PhoneNumber;
                    if (model.Address != null) profile.Address = model.Address;
                    _dbContext.Update(profile);
                    await _dbContext.SaveChangesAsync();


                    return RedirectToAction("ListUsers", "Administration");
                }
                else
                {


                    foreach (var VARIABLE in result.Errors)
                    {
                        ModelState.AddModelError("", VARIABLE.Description);
                    }

                    return View(model);
                }

            }
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            int org_id = 1;
            if (user == null)
                return new NotFoundResult();
            if (_dbContext.Employee!.Where(x => x.ApplicationUserId == id)!.Any())
            {
                var employee = _dbContext.Employee!.Where(x => x.ApplicationUserId == id)!.FirstOrDefault();
                _dbContext.Employee!.Remove(employee);
            }
            else if (_dbContext.Client!.Where(x => x.ApplicationUserId == id)!.Any())
            {
                var client = _dbContext.Client!.Where(x => x.ApplicationUserId == id)!.FirstOrDefault();
                org_id = client.OrganizationId;
                _dbContext.Client!.Remove(client);
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                if (User.IsInRole("Администатор"))
                    return RedirectToAction("ListUsers", "Administration");
                
                return RedirectToAction("ListClients", "Administration", new {id = org_id});

            }
            else
            {


                foreach (var VARIABLE in result.Errors)
                {
                    ModelState.AddModelError("", VARIABLE.Description);
                }

                if (User.IsInRole("Администатор"))
                    return RedirectToAction("ListUsers", "Administration");

                return RedirectToAction("ListClients", "Administration", new { id = org_id });
            }

        }


        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return new NotFoundResult();
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("ListRoles", "Administration");
            else
            {
                foreach (var VARIABLE in result.Errors)
                {
                    ModelState.AddModelError("", VARIABLE.Description);
                }

                return View("ListRoles");
            }

        }
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId, string userType)
        {
            ViewBag.userId = userId;
            ViewBag.userType = userType;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return new NotFoundResult();
            

            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return new NotFoundResult();


            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            var e = _dbContext.Employee.Where(x => x.ApplicationUserId == userId);

            if (_dbContext.Employee.Where(x => x.ApplicationUserId == userId).Any())
                return RedirectToAction("EditUser", new { Id = userId });
            else if (_dbContext.Client.Where(x => x.ApplicationUserId == userId).Any())
                return RedirectToAction("EditClient", new { Id = userId });
            else return new NotFoundResult();

        }


        public async Task<IActionResult> EditClient(string id)
        {

            ViewData["ClientPositionId"] = new SelectList(
                _dbContext.ClientPosition, "IdClientPosition", "Name");
            ViewData["OrganizationId"] = new SelectList(
                _dbContext.Organization, "IdOrganization", "Name");

            var user = await _userManager.FindByIdAsync(id);
            var profile = _dbContext.Client!.Where(e => e.ApplicationUserId == id)!.FirstOrDefault();


            if (user == null || profile == null)
                return new NotFoundResult();

            var userRoles = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles.Where(x => x.Name == "Администратор (контрагент)" || x.Name == "Ответственный"))
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                userRoles.Add(userRolesViewModel);
            }

            var model = new EditClientViewModel
            {
                Id = user.Id,
                Roles = userRoles,
                UserName = user.UserName,
                Email = profile.Email,
                Fio = profile.Fio,
                ClientPositionId = profile.ClientPositionId,
                OrganizationId = profile.OrganizationId,
                PhoneNumber = profile.PhoneNumber

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditClient(EditClientViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var profile = _dbContext.Client.FirstOrDefault(e => e.ApplicationUserId == model.Id);
            if (user == null || profile == null)
                return new NotFoundResult();
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = await _userManager.UpdateAsync(user);



                if (result.Succeeded)
                {
                    profile.Email = model.Email;
                    profile.Fio = model.Fio;
                    profile.ClientPositionId = model.ClientPositionId;
                    profile.PhoneNumber = model.PhoneNumber;
                    profile.OrganizationId = model.OrganizationId;
                    _dbContext.Update(profile);
                    await _dbContext.SaveChangesAsync();


                    return RedirectToAction("ListClients", "Administration", new {id = profile.OrganizationId});
                }
                else
                {


                    foreach (var VARIABLE in result.Errors)
                    {
                        ModelState.AddModelError("", VARIABLE.Description);
                    }

                    return View(model);
                }

            }
        }

        public async Task<IActionResult> ExportToExcel(string searchString)
        {
            var employees = from s in _dbContext.Employee select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(x => x.Fio.Contains(searchString) ||
                                                 x.EmployeePosition.Name.Contains(searchString) ||
                                                 x.ApplicationUser.UserName.Contains(searchString) ||
                                                 x.PhoneNumber.Contains(searchString) ||
                                                 x.Email.Contains(searchString)
                );
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Сотрудники сервиса");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "ФИО";
                worksheet.Cell(currentRow, 2).Value = "Пользователь";
                worksheet.Cell(currentRow, 3).Value = "E-mail";
                worksheet.Cell(currentRow, 4).Value = "Номер телефона";
                worksheet.Cell(currentRow, 5).Value = "Должность";
                foreach (var item in employees)
                {
                    var user = await _userManager.FindByIdAsync(item.ApplicationUserId);
                    var position = await _dbContext.EmployeePosition.FindAsync(item.EmployeePositionId);
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Fio;
                    worksheet.Cell(currentRow, 2).Value = user.UserName;
                    worksheet.Cell(currentRow, 3).Value = item.Email;
                    worksheet.Cell(currentRow, 4).Value = item.PhoneNumber;
                    worksheet.Cell(currentRow, 5).Value = position.Name;
                }

                worksheet.Columns(1, 100).AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "users.xlsx");
                }
            }


        }
    }

}
