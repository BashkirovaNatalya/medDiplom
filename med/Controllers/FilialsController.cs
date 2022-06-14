using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using med.Data;
using med.Models;
using med.ViewModels.Filials;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace med.Controllers
{
    public class FilialsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public FilialsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Filials
        public async Task<IActionResult> Index()
        {
            int u;
            var currUser = await _userManager.FindByNameAsync(User.Identity.Name);
            IQueryable<Filial> applicationDbContext;
            List<FilialViewModel> model = new List<FilialViewModel>();
            var orgs = _context.Organization;

            if (User.IsInRole("Администратор (контрагент)"))
            {

                u = _context.Client!.FirstOrDefault( x => x.ApplicationUserId == currUser.Id)!.OrganizationId;
                
                
                    //_context.Client!.FindByEmail(User.Identity.Name).OrganizationId;
                applicationDbContext = _context.Filial.Where(x => x.OrganizationId == u);
            }
            else
            {
                applicationDbContext = _context.Filial!;

            }

            foreach (var e in applicationDbContext)
            {
                var f = new FilialViewModel
                {
                    IdFilial = e.IdFilial,
                    Address = e.Address,
                    Name = e.Name,
                    OrganizationName = orgs.FirstOrDefault(x => x.IdOrganization == e.OrganizationId)!.Name
                };
                model.Add(f);
            }
        
            return View(model);
        }

        // GET: Filials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var orgs = _context.Organization;

            if (id == null || _context.Filial == null)
            {
                return NotFound();
            }

            var filial = await _context.Filial
                .Include(f => f.Organization)
                .FirstOrDefaultAsync(m => m.IdFilial == id);
            if (filial == null)
            {
                return NotFound();
            }

            var model = new FilialViewModel
            {
                Name = filial.Name,
                Address = filial.Address,
                OrganizationName = orgs.FirstOrDefault(x => x.IdOrganization == filial.OrganizationId)!.Name
            };

            return View(model);
        }

        // GET: Filials/Create
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name");
            return View();
        }

        // POST: Filials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFilialViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                var filial = new Filial
                {
                    Address = model.Address,
                    Name = model.Name,
                    OrganizationId = model.OrganizationId
                };
                _context.Add(filial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name", model.OrganizationId);
            return View(model);
        }

        // GET: Filials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Filial == null)
            {
                return NotFound();
            }

            var filial = await _context.Filial.FindAsync(id);
            if (filial == null)
            {
                return NotFound();
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name", filial.OrganizationId);
            return View(filial);
        }

        // POST: Filials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  EditFilialViewModel filial)
        {
            if (id != filial.IdFilial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var model = new Filial
                    {
                        IdFilial = filial.IdFilial,
                        Address = filial.Address,
                        Name = filial.Name,
                        OrganizationId = filial.OrganizationId
                    };


                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilialExists(filial.IdFilial))
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
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name", filial.OrganizationId);
            return View(filial);
        }

        // GET: Filials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Filial == null)
            {
                return NotFound();
            }

            var filial = await _context.Filial
                .Include(f => f.Organization)
                .FirstOrDefaultAsync(m => m.IdFilial == id);
            if (filial == null)
            {
                return NotFound();
            }

            return View(filial);
        }

        // POST: Filials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Filial == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Filial'  is null.");
            }
            var filial = await _context.Filial.FindAsync(id);
            if (filial != null)
            {
                _context.Filial.Remove(filial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilialExists(int id)
        {
          return (_context.Filial?.Any(e => e.IdFilial == id)).GetValueOrDefault();
        }
    }
}
