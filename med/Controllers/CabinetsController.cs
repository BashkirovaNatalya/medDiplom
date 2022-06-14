using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using med.Data;
using med.Models;
using med.ViewModels.Cabinets;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace med.Controllers
{
    public class CabinetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CabinetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cabinets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cabinet.Include(c => c.Filial);

            var model = new List<CabinetViewModel>();

            var filials = _context.Filial;

            foreach (var e in applicationDbContext)
            {
                var c = new CabinetViewModel
                {
                    IdCabinet = e.IdCabinet,
                    FilialName = filials.FirstOrDefault(x => x.IdFilial == e.FilialId).Name,
                    Number = e.Number,
                    Type = e.Type
                };
                model.Add(c);
            }

            
            return View(model);
        }

        // GET: Cabinets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var filials = _context.Filial;

            if (id == null || _context.Cabinet == null)
            {
                return NotFound();
            }

            var cabinet = _context.Cabinet
                .Include(c => c.Filial)
                .FirstOrDefault(m => m.IdCabinet == id);
            if (cabinet == null)
            {
                return NotFound();
            }

            var model = new CabinetViewModel
            {
                FilialName = filials.FirstOrDefault(x => x.IdFilial == cabinet.FilialId).Name,
                Type = cabinet.Type,
                Number = cabinet.Number,
                IdCabinet = cabinet.IdCabinet
            };

            return View(model);
        }

        // GET: Cabinets/Create
        public IActionResult Create()
        {
            ViewData["FilialId"] = new SelectList(_context.Filial, "IdFilial", "Name");
            return View();
        }

        // POST: Cabinets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCabinetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cabinet = new Cabinet
                {
                    FilialId = model.FilialId,
                    Type = model.Type,
                    Number = model.Number
                };

                _context.Add(cabinet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilialId"] = new SelectList(_context.Filial, "IdFilial", "Name", model.FilialId);
            return View(model);
        }

        // GET: Cabinets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cabinet == null)
            {
                return NotFound();
            }

            var cabinet = await _context.Cabinet.FindAsync(id);
            if (cabinet == null)
            {
                return NotFound();
            }
            ViewData["FilialId"] = new SelectList(_context.Filial, "IdFilial", "Name", cabinet.FilialId);



            return View(cabinet);
        }

        // POST: Cabinets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCabinetViewModel cabinet)
        {
            if (id != cabinet.IdCabinet)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var model = new Cabinet
                    {
                        FilialId = cabinet.FilialId,
                        IdCabinet = cabinet.IdCabinet,
                        Number = cabinet.Number,
                        Type = cabinet.Type
                    };

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CabinetExists(cabinet.IdCabinet))
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
            ViewData["FilialId"] = new SelectList(_context.Filial, "IdFilial", "Name", cabinet.FilialId);
            return View(cabinet);
        }

        // GET: Cabinets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cabinet == null)
            {
                return NotFound();
            }

            var cabinet = await _context.Cabinet
                .Include(c => c.Filial)
                .FirstOrDefaultAsync(m => m.IdCabinet == id);
            if (cabinet == null)
            {
                return NotFound();
            }

            return View(cabinet);
        }

        // POST: Cabinets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cabinet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cabinet'  is null.");
            }
            var cabinet = await _context.Cabinet.FindAsync(id);
            if (cabinet != null)
            {
                _context.Cabinet.Remove(cabinet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CabinetExists(int id)
        {
          return (_context.Cabinet?.Any(e => e.IdCabinet == id)).GetValueOrDefault();
        }
    }
}
