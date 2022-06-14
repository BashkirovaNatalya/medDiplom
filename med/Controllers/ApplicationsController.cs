using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using med.Data;
using med.Models;
using med.ViewModels.Applications;

namespace med.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Applications

        public async Task<IActionResult> Mine()
        {
            var applicationDbContext = _context.Application
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Employee)
                .Include(a => a.Equipment)
                .Include(a => a.Equipment.Client)
                .Include(a => a.Equipment.Client.ApplicationUser);

            if (User.IsInRole("Инженер"))
                applicationDbContext = applicationDbContext
                    .Where(x => x.Employee.ApplicationUser.UserName == User.Identity.Name)
                    .Include(a => a.ApplicationStatus)
                    .Include(a => a.Employee)
                    .Include(a => a.Equipment)
                    .Include(a => a.Equipment.Client)
                    .Include(a => a.Equipment.Client.ApplicationUser);

            return View(await applicationDbContext.OrderBy(x => x.ApplicationStatusId).ToListAsync());
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Application
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Employee)
                .Include(a => a.Equipment)
                .Include(a => a.Equipment.Client)
                .Include(a => a.Equipment.Client.ApplicationUser);

            if (User.IsInRole("Ответственный"))
                applicationDbContext = applicationDbContext.Where(x =>
                    x.Equipment.Client.ApplicationUser.UserName == User.Identity.Name)
                    .Include(a => a.ApplicationStatus)
                    .Include(a => a.Employee)
                    .Include(a => a.Equipment)
                    .Include(a => a.Equipment.Client)
                    .Include(a => a.Equipment.Client.ApplicationUser);

            if (User.IsInRole("Инженер"))
                applicationDbContext = applicationDbContext.Where(x =>
                        x.Employee == null)
                    .Include(a => a.ApplicationStatus)
                    .Include(a => a.Employee)
                    .Include(a => a.Equipment)
                    .Include(a => a.Equipment.Client)
                    .Include(a => a.Equipment.Client.ApplicationUser);



            return View(await applicationDbContext.OrderBy(x => x.ApplicationStatusId).ToListAsync());
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Application == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Employee)
                .Include(a => a.Equipment)
                .Include(a => a.Equipment.Client)
                .FirstOrDefaultAsync(m => m.IdApplication == id);
            if (application == null)
            {
                return NotFound();
            }

            var model = new ApplicationViewModel
            {
                IdApplication = application.IdApplication,
                Description = application.Description,
                StartDatePlan = application.StartDatePlan,
                StartDateFact = application.StartDateFact,
                FinishDatePlan = application.FinishDatePlan,
                FinishDateFact = application.FinishDateFact,
                EquipmentId = application.EquipmentId,
                Equipment = application.Equipment,
                EmployeeId = application.EmployeeId,
                Employee = application.Employee,
                ApplicationStatusId = application.ApplicationStatusId,
                ApplicationStatus = application.ApplicationStatus,
                Jobs = _context.Job
                    .Include(x => x.JobType)
                    .Where(x => x.ApplicationId == application.IdApplication).ToList()
            };

            return View(model);
        }

        // GET: Applications/Create
        public IActionResult Create()
        {
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatus, "IdApplicationStatus", "Name");
            ViewData["EmployeeId"] = new SelectList(
                _context.Employee.Where(x => x.EmployeePosition.Name=="Инженер")
                    .ToList().Prepend(new Employee()),
                "IdEmployee", "Fio");
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "IdEquipment", "Name");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateApplicationViewModel application)
        {
            if (ModelState.IsValid)
            {
                var model = new Application
                {
                    Description = application.Description,
                    EquipmentId = application.EquipmentId,
                    ApplicationStatusId = _context.ApplicationStatus
                        .First(x => x.Name == "Оформлена")
                        .IdApplicationStatus
                    
                };
                
                if (application.StartDatePlan != null) model.StartDatePlan = application.StartDatePlan;
                if (application.FinishDatePlan != null) model.FinishDatePlan = application.FinishDatePlan;
                
                if (application.Description != "") model.Description = application.Description;
                
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatus, "IdApplicationStatus", "Name");
            ViewData["EmployeeId"] = new SelectList(
                _context.Employee.Where(x => x.EmployeePosition.Name == "Инженер")
                    .ToList().Prepend(new Employee()),
                "IdEmployee", "Fio");
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "IdEquipment", "Name");
            return View(application);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Application == null)
            {
                return NotFound();
            }

            var application = await _context.Application.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            //if (User.IsInRole("Ответственный"))
            //{
                var model = new EditApplicationViewModel
                {
                    IdApplication = application.IdApplication,
                    Description = application.Description,
                    StartDatePlan = application.StartDatePlan,
                    FinishDatePlan = application.FinishDatePlan,
                    EquipmentId = application.EquipmentId,
                    ApplicationStatusId = application.ApplicationStatusId,
                };
            

                ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatus, "IdApplicationStatus",
                    "Name", application.ApplicationStatusId);

                ViewData["EquipmentId"] =
                    new SelectList(_context.Equipment, "IdEquipment", "Name", application.EquipmentId);
                return View(model);
            //}
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> AssignApplication(int id)
        {
            var applicationDbContext = _context.Application.Include(a => a.ApplicationStatus).Include(a => a.Employee).Include(a => a.Equipment);
            var model = await applicationDbContext.FirstAsync(x => x.IdApplication == id);
            ViewData["EmployeeId"] = new SelectList(
                _context.Employee.Where(x => x.EmployeePosition.Name == "Инженер")
                    .ToList().Prepend(new Employee()),
                "IdEmployee", "Fio");
            return View(model);
        }

        public async Task<IActionResult> AssignApplication(int id, AssignApplicationViewModel assignApplication)
        {
            if (id != assignApplication.IdApplication)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var model = await _context.Application.FindAsync(id);

                    model.IdApplication = assignApplication.IdApplication;
                    if (assignApplication.EmployeeId != 0) model.EmployeeId = assignApplication.EmployeeId;
                    else model.EmployeeId = null;

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(assignApplication.IdApplication))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View();




        }


        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
            EditApplicationViewModel application)
        {
            if (id != application.IdApplication)
            {
                return NotFound();
            }

            var applicationOld = await _context.Application.FindAsync(id);
            if (applicationOld == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var model = applicationOld;

                    model.IdApplication = application.IdApplication;
                    model.Description = application.Description;
                    model.StartDatePlan = application.StartDatePlan;
                    model.FinishDatePlan = application.FinishDatePlan;
                    model.EquipmentId = application.EquipmentId;
                    model.ApplicationStatusId = application.ApplicationStatusId;

                    if (applicationOld.EmployeeId > 0) model.EmployeeId = applicationOld.EmployeeId;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.IdApplication))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatus, "IdApplicationStatus", "Name", application.ApplicationStatusId);

            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "IdEquipment", "Name", application.EquipmentId);
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Application == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Employee)
                .Include(a => a.Equipment)
                .FirstOrDefaultAsync(m => m.IdApplication == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Application == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Application'  is null.");
            }
            var application = await _context.Application.FindAsync(id);
            if (application != null)
            {
                _context.Application.Remove(application);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
          return (_context.Application?.Any(e => e.IdApplication == id)).GetValueOrDefault();
        }


    }
}
