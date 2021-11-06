using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class BudgetController : Controller
    {
        // GET: Budget
        public ActionResult Instance(int Id)
        {
            SetSessionData();
            return View("~/Views/Maintenance/BudgetMaintenance.cshtml");
        }

        public JsonResult ListBudgets(int projectId)
        {
            List<Budgets> attList = new List<Budgets>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                attList = (from p in db.Budgets.Where(b => b.ProjectId == projectId && b.IdState == (int)Common.AppEnums.States.ACTIVE)
                           select p).ToList();
            }
            return Json(new { data = attList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBudgetLines(int budgetId)
        {
            List<BudgetLines> budgetLines = new List<BudgetLines>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                budgetLines = (from p in db.BudgetLines.Where(b => b.BudgetId == budgetId)
                           select p).ToList();
            }
            return Json(new { data = budgetLines }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int budgetId)
        {
            bool response = true;
            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    Budgets oBudget = new Budgets();
                    oBudget = (from c in db.Budgets.Where(x => x.Id == budgetId)
                                select c).FirstOrDefault();

                    oBudget.IdState = (int)Common.AppEnums.States.DELETE;

                    db.SaveChanges();
                }
                AppController.AuditAction(new Audit { Module = "Presupuesto", Action = "Eliminar", Date = DateTime.Now });
            }
            catch
            {
                response = false;
            }

            return Json(new { result = response }, JsonRequestBehavior.AllowGet);
        }

        private void SetSessionData()
        {
            var oUser = (Models.Users)System.Web.HttpContext.Current.Session["user"];
            ViewBag.UserId = oUser.Id;
            ViewBag.UserName = oUser.Name + " " + oUser.Last_Name;
        }
    }
}