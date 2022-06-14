using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using med.Data;
using med.Models;
using med.ViewModels.Job;
using Microsoft.Build.Framework;

namespace med.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public JobsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Job.Include(j => j.Application).Include(j => j.JobType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.Application)
                .Include(j => j.JobType)
                .FirstOrDefaultAsync(m => m.IdJob == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Create
        public IActionResult Create(string appId)
        {
            ViewData["ApplicationId"] = appId;
            ViewData["JobTypeId"] = new SelectList(_context.JobType, "IdJobType", "Name");

            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string appId ,CreateJobViewModel job)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                


                var model = new Job
                {
                    Description = job.Description,
                    Cost = job.Cost,
                    StartDatePlan = job.StartDatePlan,
                    FinishDatePlan = job.FinishDatePlan,
                    JobTypeId = job.JobTypeId,
                    ApplicationId = int.Parse(appId),
                };
                var app = await _context.Application.FindAsync(int.Parse(appId));
                app.ApplicationStatusId = 3;

                var result = _context.Add(model);
                await _context.SaveChangesAsync();

                if (job.Photos != null)
                {
                    string uploads = _environment.WebRootPath;
                    foreach (var photo in job.Photos)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                        string filePath = Path.Combine(uploads, uniqueFileName);

                        await photo.CopyToAsync(new FileStream(filePath, FileMode.Create));

                        var img = new JobImage
                        {
                            FilePath = filePath,
                            JobId = result.Entity.IdJob
                        };
                        model.Images.Add(img);
                        _context.Update(model);
                        _context.Add(img);
                        await _context.SaveChangesAsync();


                    }

                }

                return RedirectToAction("Details", "Applications", new { id = app.IdApplication });
            }
            ViewData["ApplicationId"] = appId;
            ViewData["JobTypeId"] = new SelectList(_context.JobType, "IdJobType", "Name", job.JobTypeId);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["ApplicationId"] = new SelectList(_context.Application, "IdApplication", "IdApplication", job.ApplicationId);
            ViewData["JobTypeId"] = new SelectList(_context.JobType, "IdJobType", "IdJobType", job.JobTypeId);

            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJob,Description,Cost,StartDatePlan,StartDateFact,FinishDatePlan,FinishDateFact,HoursPlan,HoursFact,JobTypeId,ApplicationId,StageId")] Job job)
        {
            if (id != job.IdJob)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.IdJob))
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
            ViewData["ApplicationId"] = new SelectList(_context.Application, "IdApplication", "Description", job.ApplicationId);
            ViewData["JobTypeId"] = new SelectList(_context.JobType, "IdJobType", "Name", job.JobTypeId);

            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.Application)
                .Include(j => j.JobType)

                .FirstOrDefaultAsync(m => m.IdJob == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Job == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Job'  is null.");
            }
            var job = await _context.Job.FindAsync(id);
            if (job != null)
            {
                var images = _context.JobImage.Where(x => x.JobId == job.IdJob);
                if (images.Any())
                    foreach (var img in images)
                    {
                        _context.JobImage.Remove(img);
                    }
                _context.Job.Remove(job);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
          return (_context.Job?.Any(e => e.IdJob == id)).GetValueOrDefault();
        }
    }
}
