using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserMaintenance()
        {
            return View("~/Views/Maintenance/UserMaintenance.cshtml");
        }
    }
}