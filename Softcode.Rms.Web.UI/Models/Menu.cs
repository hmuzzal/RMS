using System.Collections.Generic;
using System.Linq;
using Softcode.Rms.Web.UI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Softcode.Rms.Web.UI.Models
{
    public class Menu : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Menu(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var menus = _context.Modules.Include(m => m.SubModules)
                .OrderBy(m => m.SerialNo).ToList();
            return View(menus);
        }
    }
}