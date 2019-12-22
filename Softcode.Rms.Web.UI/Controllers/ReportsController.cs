using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Softcode.Rms.Web.UI.Models;
using Microsoft.AspNetCore.Hosting;
using Softcode.Rms.Web.UI.Data;

namespace Softcode.Rms.Web.UI.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string filePath;

        public ReportsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reports.Include(r => r.Module).Include(r => r.SubModule);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Module)
                .Include(r => r.SubModule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }



        public async Task<IActionResult> ReportDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var report = await _context.Reports.FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }



        public JsonResult GetSubModule(int id)
        {
            return Json(new SelectList(_context.SubModules.Where(s => s.ModuleId == id), "Id", "Name"));
        }



        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Image,IsoNo,Description,ModuleId,SubModuleId")] Models.ReportViewModel model)
        {
            if (model.Image == null)
            {
                ModelState.AddModelError(string.Empty, "Please Select a image to create report.");
            }
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Image != null)
                {
                    string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    filePath = Path.Combine(uplaodsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Image.CopyTo(stream);
                    }

                }

                Models.Report report = new Models.Report
                {
                    Title = model.Title,
                    ImageUrl = uniqueFileName,
                    ReportUrl = "/Reports/ReportDetails/",
                    IsoNo = model.IsoNo,
                    Description = model.Description,
                    ModuleId = model.ModuleId,
                    SubModuleId = model.SubModuleId
                };


                _context.Add(report);


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", model.ModuleId);
            //ViewData["SubModuleId"] = new SelectList(_context.SubModules, "Id", "Name", model.SubModuleId);
            return View(model);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", report.ModuleId);
            ViewData["SubModuleId"] = new SelectList(_context.SubModules.Where(s => s.ModuleId == report.ModuleId), "Id", "Name", report.SubModuleId);
            TempData["PreviousImage"] = report.ImageUrl;
            ReportViewModel viewReport = new ReportViewModel
            {
                Title = report.Title,
                ImageUrl = report.ImageUrl,
                ReportUrl = report.ReportUrl,
                IsoNo = report.IsoNo,
                Description = report.Description,
                ModuleId = report.ModuleId,
                SubModuleId = report.SubModuleId
            };

            return View(viewReport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ImageUrl,ReportUrl,Image,IsoNo,Description,ModuleId,SubModuleId")] ReportViewModel viewReport)
        {
            if (id != viewReport.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                string previousImageUrl = TempData["PreviousImage"].ToString();


                string uniqueFileName = null;

                if (viewReport.Image != null)
                {

                    string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + viewReport.Image.FileName;
                    filePath = Path.Combine(uplaodsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewReport.Image.CopyTo(stream);
                    }

                    viewReport.ImageUrl = uniqueFileName;
                }


                Models.Report report = new Models.Report
                {
                    Id = viewReport.Id,
                    Title = viewReport.Title,
                    ImageUrl = viewReport.ImageUrl,
                    ReportUrl = viewReport.ReportUrl,
                    IsoNo = viewReport.IsoNo,
                    Description = viewReport.Description,
                    ModuleId = viewReport.ModuleId,
                    SubModuleId = viewReport.SubModuleId
                };

                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();

                    string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    if (viewReport.ImageUrl != previousImageUrl)
                    {
                        if (System.IO.File.Exists(Path.Combine(uplaodsFolder, previousImageUrl)))
                        {

                            System.IO.File.Delete(Path.Combine(uplaodsFolder, previousImageUrl));
                        }

                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", viewReport.ModuleId);
            ViewData["SubModuleId"] = new SelectList(_context.SubModules.Where(s => s.ModuleId == viewReport.ModuleId), "Id", "Name", viewReport.SubModuleId);
            return View(viewReport);
        }

        public async Task<IActionResult> Delete(int? id)
        {

            await _context.Reports.FindAsync(id);
            var report = await _context.Reports.FindAsync(id);
            string previousImageUrl = report.ImageUrl;
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");


            if (System.IO.File.Exists(Path.Combine(uplaodsFolder, previousImageUrl)))
            {

                System.IO.File.Delete(Path.Combine(uplaodsFolder, previousImageUrl));
            }


            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
