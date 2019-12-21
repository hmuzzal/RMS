using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Softcode.Rms.Web.UI.Models;
using Softcode.Rms.Web.UI.Data;

namespace Softcode.Rms.Web.UI.Controllers
{
    [Authorize]
    public class SubModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SubModules.Include(s => s.Module);
            return View(await applicationDbContext.ToListAsync());
        }

     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subModule = await _context.SubModules
                .Include(s => s.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subModule == null)
            {
                return NotFound();
            }

            return View(subModule);
        }

        
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Url,ModuleId")] SubModule subModule)
        {
            if (ModelState.IsValid)
            {
                if (SubModuleNameExists(subModule.Name))
                {
                    ModelState.AddModelError(string.Empty, "Sub-Module already exists.");
                    return View(subModule);

                }

                _context.Add(subModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", subModule.ModuleId);
            return View(subModule);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subModule = await _context.SubModules.FindAsync(id);
            if (subModule == null)
            {
                return NotFound();
            }

            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", subModule.ModuleId);
            return View(subModule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Url,ModuleId")] SubModule subModule)
        {
            if (id != subModule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubModuleExists(subModule.Id, subModule.Name))
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

            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Name", subModule.ModuleId);
            return View(subModule);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var subModule = await _context.SubModules.FindAsync(id);
            _context.SubModules.Remove(subModule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubModuleExists(int id, string name)
        {
            return _context.SubModules.Any(e => e.Id == id || e.Name == name);
        }

        private bool SubModuleNameExists(string name)
        {
            return _context.SubModules.Any(e => e.Name == name);
        }
    }
}
