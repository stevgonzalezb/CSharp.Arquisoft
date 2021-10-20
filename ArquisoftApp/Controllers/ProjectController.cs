using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {
            return View("~/Views/Maintenance/ProjectList.cshtml");
        }

        public ActionResult Instance(int Id)
        {
            return View("~/Views/Maintenance/ProjectMaintenance.cshtml");
        }
    }
}