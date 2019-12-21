using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Softcode.Rms.Web.UI.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {

        internal List<string> CurrentErrors
        {
            get
            {
                return ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
        }

        public JsonResult ErrorResult()
        {
            return Json(new { Success = false, Errors = CurrentErrors });
        }

        public JsonResult ErrorResult(string message)
        {
            return Json(new { Success = false, Errors = message });
        }

        public JsonResult Reload()
        {
            return Json(new { Success = true, Reload = true });
        }
        public JsonResult Reload(int saveStatus)
        {
            return saveStatus > 0 ? Json(new { Success = true, Reload = true }) : ErrorResult("Fail to save data");
        }

        public ActionResult Dialog()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }

            return View();
        }

        public ActionResult Dialog(object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        public ActionResult Dialog(string viewName, object model)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, model);
            }

            return View(viewName, model);
        }
    }
}

public static class ExtenssionMethod
{

    public static bool IsAjaxRequest(this HttpRequest request)
    {
        if (request == null)
            throw new ArgumentNullException("request");

        if (request.Headers != null)
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        return false;
    }
}