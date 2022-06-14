using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using med.Data;
using med.Data.Migrations;
using med.Models;
using med.ViewModels.Equipment;
using Microsoft.AspNetCore.Http.Features;

namespace med.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EquipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Equipment
                .Include(e => e.Cabinet.Filial.Organization)
                .Include(e => e.Client)
                .Include(e => e.Client.ApplicationUser)
                .Include(e => e.EquipmentType);

            if (User.IsInRole("Ответственный"))
            {
                applicationDbContext = applicationDbContext.Where(e => e.Client.ApplicationUser.UserName == User.Identity.Name)
                    .Include(e => e.Cabinet.Filial.Organization)
                    .Include(e => e.Client)
                    .Include(e => e.Client.ApplicationUser)
                    .Include(e => e.EquipmentType);
            }
            return View(await applicationDbContext.OrderBy(x => x.EquipmentType).ToListAsync());
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipment == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .Include(e => e.Cabinet)
                .Include(e => e.Client)
                .Include(e => e.EquipmentType)
                .FirstOrDefaultAsync(m => m.IdEquipment == id);

            var appList = _context.Application.Where(x => x.EquipmentId == equipment.IdEquipment)
                .Include(x => x.Employee)
                .Include(x => x.ApplicationStatus).ToList();

            var model = new EquipmentViewModel
            {
                IdEquipment = equipment.IdEquipment,
                Name = equipment.Name,
                Manufacturer = equipment.Manufacturer,
                SerialNumber = equipment.SerialNumber,
                ManufacturingYear = equipment.ManufacturingYear,
                CertificateNubber = equipment.CertificateNubber,
                CommissioningYear = equipment.CommissioningYear,
                DecommissioningYear = equipment.DecommissioningYear,
                PeriodTO = equipment.PeriodTO,
                EquipmentType = equipment.EquipmentType,
                Cabinet = equipment.Cabinet,
                Client = equipment.Client,
                ApplicationList = appList
            };


            if (equipment == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {

            ViewData["CabinetId"] = new SelectList(_context.Cabinet, "IdCabinet", "Number");
            ViewData["ClientId"] = new SelectList(
            _context.Client.Where(x => x.ClientPosition.Name == "Ответственный").ToList()
                .Prepend(new Client()),
            "IdClient", "Fio");
            ViewData["EquipmentTypeId"] = new SelectList(_context.EquipmentType, "IdEquipmentType", "Name");
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEquipmentViewModel equipment)
        {
            if (ModelState.IsValid)
            {
                var model = new Equipment
                {
                    Name = equipment.Name,
                    Manufacturer = equipment.Manufacturer,
                    SerialNumber = equipment.SerialNumber,
                    ManufacturingYear = equipment.ManufacturingYear,
                    CertificateNubber = equipment.CertificateNubber,
                    CommissioningYear = equipment.CommissioningYear,
                    DecommissioningYear = equipment.DecommissioningYear,
                    PeriodTO = equipment.PeriodTO,
                    EquipmentTypeId = equipment.EquipmentTypeId,
                    CabinetId = equipment.CabinetId
                };
                if (equipment.ClientId > 0) 
                    model.ClientId = equipment.ClientId;
                //TODO: client id!
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CabinetId"] = new SelectList(_context.Cabinet, "IdCabinet", "Number");
            ViewData["ClientId"] = new SelectList(
                _context.Client.Where(x => x.ClientPosition.Name == "Ответственный").ToList().Prepend(new Client()),
                "IdClient", "Fio");
            ViewData["EquipmentTypeId"] = new SelectList(_context.EquipmentType, "IdEquipmentType", "Name");
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Equipment == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            ViewData["CabinetId"] = new SelectList(_context.Cabinet, "IdCabinet", "Number", equipment.CabinetId);
            ViewData["ClientId"] = new SelectList(
                _context.Client.Where(x => x.ClientPosition.Name == "Ответственный")
                    .ToList().Prepend(new Client()),
                "IdClient", "Fio");
            ViewData["EquipmentTypeId"] = new SelectList(_context.EquipmentType, "IdEquipmentType", "Name", equipment.EquipmentTypeId);
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
            [Bind("IdEquipment,Name,Manufacturer,SerialNumber,ManufacturingYear,CertificateNubber,CommissioningYear,DecommissioningYear,PeriodTO,EquipmentTypeId,CabinetId,ClientId")] 
            EditEquipmentViewModel equipment)
        {
            if (id != equipment.IdEquipment)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var model = new Equipment
                    {
                        IdEquipment = equipment.IdEquipment,
                        Name = equipment.Name,
                        Manufacturer = equipment.Manufacturer,
                        SerialNumber = equipment.SerialNumber,
                        ManufacturingYear = equipment.ManufacturingYear,
                        CertificateNubber = equipment.CertificateNubber,
                        CommissioningYear = equipment.CommissioningYear,
                        DecommissioningYear = equipment.DecommissioningYear,
                        PeriodTO = equipment.PeriodTO,
                        EquipmentTypeId = equipment.EquipmentTypeId,
                        CabinetId = equipment.CabinetId
                    };
                    if (equipment.ClientId > 0)
                        model.ClientId = equipment.ClientId;


                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.IdEquipment))
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
            ViewData["CabinetId"] = new SelectList(_context.Cabinet, "IdCabinet", "Number", equipment.CabinetId);
            ViewData["ClientId"] = new SelectList(
                _context.Client.Where(x => x.ClientPosition.Name == "Ответственный")
                    .ToList().Prepend(new Client()),
                "IdClient", "Fio");
            ViewData["EquipmentTypeId"] = new SelectList(_context.EquipmentType, "IdEquipmentType", "Name", equipment.EquipmentTypeId);
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Equipment == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .Include(e => e.Cabinet)
                .Include(e => e.Client)
                .Include(e => e.EquipmentType)
                .FirstOrDefaultAsync(m => m.IdEquipment == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Equipment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Equipment'  is null.");
            }
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
          return (_context.Equipment?.Any(e => e.IdEquipment == id)).GetValueOrDefault();
        }

        public IActionResult ExportToExcel(string searchString)
        {
            var equipment =  _context.Equipment
                .Include(x => x.Client)
                .Include(x => x.EquipmentType)
                .Include(x => x.Cabinet)
                .Include(x=> x.Cabinet.Filial.Organization);

            if (!string.IsNullOrEmpty(searchString))
            {
                equipment = equipment.Where(x => x.Name.Contains(searchString) ||
                                                 x.Manufacturer.Contains(searchString) ||
                                                 x.Client.Fio.Contains(searchString)
                )
                    .Include(x => x.Client)
                    .Include(x => x.EquipmentType)
                    .Include(x => x.Cabinet)
                    .Include(x => x.Cabinet.Filial.Organization);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Сотрудники сервиса");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Название";
                worksheet.Cell(currentRow, 2).Value = "Производитель";
                worksheet.Cell(currentRow, 3).Value = "Серийный номер";
                worksheet.Cell(currentRow, 4).Value = "Год выпуска";
                worksheet.Cell(currentRow, 5).Value = "Номер сертификата";
                worksheet.Cell(currentRow, 6).Value = "Год ввода в эксплуатацию";
                worksheet.Cell(currentRow, 7).Value = "Год списания";
                worksheet.Cell(currentRow, 8).Value = "Период ТО";
                worksheet.Cell(currentRow, 9).Value = "Тип";
                worksheet.Cell(currentRow, 10).Value = "Кабинет";
                worksheet.Cell(currentRow, 11).Value = "Филиал";
                worksheet.Cell(currentRow, 12).Value = "Организация";
                worksheet.Cell(currentRow, 13).Value = "Ответственный";
                foreach (var item in equipment)
                {
                    var eqTypeName = _context.EquipmentType.Find(item.EquipmentTypeId)!.Name;
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Name;
                    worksheet.Cell(currentRow, 2).Value = item.Manufacturer;
                    worksheet.Cell(currentRow, 3).Value = item.SerialNumber;
                    worksheet.Cell(currentRow, 4).Value = item.ManufacturingYear;
                    worksheet.Cell(currentRow, 5).Value = item.CertificateNubber;
                    worksheet.Cell(currentRow, 6).Value = item.CommissioningYear;
                    worksheet.Cell(currentRow, 7).Value = item.DecommissioningYear;
                    worksheet.Cell(currentRow, 8).Value = item.PeriodTO;
                    worksheet.Cell(currentRow, 9).Value = eqTypeName;
                    worksheet.Cell(currentRow, 10).Value = item.Cabinet.Number;
                    worksheet.Cell(currentRow, 11).Value = item.Cabinet.Filial.Name;
                    worksheet.Cell(currentRow, 12).Value = item.Cabinet.Filial.Organization.Name;
                    worksheet.Cell(currentRow, 13).Value = item.Client.Fio;
                }

                worksheet.Columns(1, 100).AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "equipment.xlsx");
                }
            }

        }
    }
}
