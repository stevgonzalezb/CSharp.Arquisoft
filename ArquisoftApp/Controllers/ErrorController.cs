using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotAuthorized()
        {
            return View("~/Views/Error/NotAuthorized.cshtml");
        }
    }
}