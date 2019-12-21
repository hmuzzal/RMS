using Softcode.Rms.Web.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Softcode.Rms.Web.UI.Data;
using Microsoft.AspNetCore.Authorization;

namespace Softcode.Rms.Web.UI.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ModulesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

      
        public async Task<IActionResult> Index()
        {
            return View(await _context.Modules.OrderBy(m => m.SerialNo).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

     
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SerialNo")] Module @module)
        {
            if (ModelState.IsValid)
            {

                if (ModuleNameExists(@module.Name))
                {
                    ModelState.AddModelError(string.Empty, "Module already exists.");
                    return View(@module);


                }
                if (ModuleSerialExists(@module.SerialNo))
                {
                    ModelState.AddModelError(string.Empty, "Serial No. is already given to a module.");
                    return View(@module);

                }
                _context.Add(@module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(@module);
        }

 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }
            return View(@module);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SerialNo")] Module @module)
        {
            if (id != @module.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //if (ModuleNameExistsExceptCurrent(@module))
                //{
                //    ModelState.AddModelError(string.Empty, "Module already exists.");
                //    return View(@module);


                //}

                //if (ModuleSerialExistsExceptCurrent(@module))
                //{
                //    ModelState.AddModelError(string.Empty, "Serial No. is already given to a module.");
                //    return View(@module);

                //}

                try
                {
                    _context.Update(@module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.Id))
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
            return View(@module);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var @module = await _context.Modules.FindAsync(id);
            _context.Modules.Remove(@module);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public ActionResult GetMenuList()
        //{
        //    var menus = _context.Modules.Include(m => m.SubModules)
        //        .Include(m => m.Reports).ToList();
        //    List<Module> menuTree = GetMenuTree(menus, 1);
        //    return View("_menuFromDb", menuTree);
        //}
        //public JsonResult MenuList()
        //{
        //    var menus = _context.Modules.Include(m => m.SubModules)
        //        .Include(m => m.Reports).ToList();
        //    List<Module> menuTree = GetMenuTree(menus, 1);
        //    return Json(menuTree);
        //}

        //private List<Module> GetMenuTree(List<Module> moduleList, int? moduleId)
        //{
        //    return moduleList.Where(x => x.Id == moduleId).ToList();
        //}

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.Id == id);
        }

        private bool ModuleNameExists(string name)
        {
            return _context.Modules.Any(e => e.Name == name);
        }
        private bool ModuleNameExistsExceptCurrent(Module module)
        {
            var currentModule = _context.Modules.Where(m => m.Id == module.Id);
            return _context.Modules.ToList().Except(currentModule).Any(e => e.Name == module.Name);
        }
        private bool ModuleSerialExists(int serial)
        {
            return _context.Modules.Any(e => e.SerialNo == serial);
        }

        private bool ModuleSerialExistsExceptCurrent(Module module)
        {
            var currentModule = _context.Modules.Where(m => m.Id == module.Id);
            return _context.Modules.ToList().Except(currentModule).Any(e => e.SerialNo == module.SerialNo);
        }

        public PartialViewResult SideNavBer(int id)
        {

            var menus = _context.Reports.Where(x => x.SubModuleId == id).ToList();
            menus = menus.Select(x =>
            {
                x.ImageUrl = Path.Combine("images/", x.ImageUrl);
                return x;
            }).ToList();
            return PartialView("_SideMenuData", menus);
        }

    }
}
