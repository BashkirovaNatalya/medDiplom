using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using med.Data;
using med.Models;

namespace med.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrganizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Organizations
        public async Task<IActionResult> Index()
        {
              return _context.Organization != null ? 
                          View(await _context.Organization.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Organization'  is null.");
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Organization == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization
                .FirstOrDefaultAsync(m => m.IdOrganization == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // GET: Organizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOrganization,Name,Address,PhoneNumber,WebSite,OKPO,OKVED,INN,CorrAcc,BIK,KPP,OGRN")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Organization == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdOrganization,Name,Address,PhoneNumber,WebSite,OKPO,OKVED,INN,CorrAcc,BIK,KPP,OGRN")] Organization organization)
        {
            if (id != organization.IdOrganization)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.IdOrganization))
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
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Organization == null)
            {
                return NotFound();
            }

            var organization = await _context.Organization
                .FirstOrDefaultAsync(m => m.IdOrganization == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Organization == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Organization'  is null.");
            }
            var organization = await _context.Organization.FindAsync(id);
            if (organization != null)
            {
                _context.Organization.Remove(organization);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
          return (_context.Organization?.Any(e => e.IdOrganization == id)).GetValueOrDefault();
        }

        public IActionResult ExportToExcel(string searchString)
        {
            var orgs = from s in _context.Organization select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                orgs = orgs.Where(x => x.Name.Contains(searchString) ||
                                                 x.Address.Contains(searchString) ||
                                                 x.PhoneNumber.Contains(searchString) ||
                                                 x.WebSite.Contains(searchString)
                );
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Клиентские организации");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Название";
                worksheet.Cell(currentRow, 2).Value = "Адрес";
                worksheet.Cell(currentRow, 3).Value = "Номер телефона";
                worksheet.Cell(currentRow, 4).Value = "Веб-сайт";
                worksheet.Cell(currentRow, 5).Value = "ОКПО";
                worksheet.Cell(currentRow, 6).Value = "ОКВЭД";
                worksheet.Cell(currentRow, 7).Value = "Корр.счет";
                worksheet.Cell(currentRow, 8).Value = "БИК"; 
                worksheet.Cell(currentRow, 9).Value = "КПП";
                worksheet.Cell(currentRow, 10).Value = "ОГРН";
                foreach (var item in orgs)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Name;
                    worksheet.Cell(currentRow, 2).Value = item.Address;
                    worksheet.Cell(currentRow, 3).Value = item.PhoneNumber;
                    worksheet.Cell(currentRow, 4).Value = item.WebSite;
                    worksheet.Cell(currentRow, 5).Value = item.OKPO;
                    worksheet.Cell(currentRow, 6).Value = "\'" + item.OKVED;

                    worksheet.Cell(currentRow, 7).SetDataType(XLDataType.Text);
                    worksheet.Cell(currentRow, 7).Value = "\'" + item.CorrAcc;
                    worksheet.Cell(currentRow, 8).Value = item.BIK;
                    worksheet.Cell(currentRow, 9).Value = item.KPP;
                    worksheet.Cell(currentRow, 10).Value = "\'" + item.OGRN;
                }

                worksheet.Columns(1, 100).AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "orgs.xlsx");
                }
            }
        }
    }
    
}
