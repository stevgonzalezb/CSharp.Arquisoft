﻿using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public ActionResult Report(int Id)
        {
            //SetSessionData();
            return View("~/Views/Reports/_BudgetReport.cshtml");
        }

        public JsonResult SaveBudget(Budgets oBudget)
        {
            String response = "";

            try
            {
                if (oBudget.Id == 0)
                {
                    if (AppController.IsAuthorized(Common.AppEnums.Permissions.PROJECT_ADD))
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            oBudget.CreationDate = DateTime.Now;
                            db.Budgets.Add(oBudget);
                            db.SaveChanges();
                        }
                        AppController.AuditAction(new Audit { Module = "Presupuestos", Action = "Crear", Date = DateTime.Now });
                        response = oBudget.Id.ToString();

                    }
                    else
                        Redirect("~/Views/Error/NotAuthorized.cshtml");
                }
            }
            catch(Exception e)
            {
                response = "Ocurrió un error en el proceso de almacenado.";
            }

            return Json(new { result = response }, JsonRequestBehavior.AllowGet);
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

            var data = "[]";
            var ArquisoftConnection = AppController.GetConnectionString();
            var queryString = string.Format(@"SELECT A.Id [Budget], A.Fee, A.Total Subtotal, CONVERT(DECIMAL(18,1), ((A.Total*(CONVERT(decimal(10,2),A.Fee)/100))+A.Total)) Total, Lines.*
                                                FROM Budgets A 
	                                                LEFT JOIN BudgetLines Lines ON A.Id = Lines.BudgetId
                                                WHERE A.Id = {0} 
                                                FOR JSON AUTO", budgetId);

            using (SqlConnection conn = new SqlConnection(ArquisoftConnection))
            {
                using (SqlCommand command = new SqlCommand(queryString, conn))
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            data = reader.GetValue(0).ToString();
                        }
                    }
                }
            }

            return Json(new { data }, JsonRequestBehavior.AllowGet);



            //List<BudgetLines> budgetLines = new List<BudgetLines>();

            //using (ArquisoftEntities db = new ArquisoftEntities())
            //{

            //    budgetLines = (from p in db.BudgetLines.Where(b => b.BudgetId == budgetId)
            //               select p).ToList();
            //}
            //return Json(new { data = budgetLines }, JsonRequestBehavior.AllowGet);
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


        [HttpPost]
        public JsonResult SaveBudgetLine(BudgetLines oBudgetLine, Budgets oBudget)
        {

            String response = "";
            try
            {

                if (oBudgetLine.Id == 0)
                {
                    if (AppController.IsAuthorized(Common.AppEnums.Permissions.PROJECT_ADD))
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            db.BudgetLines.Add(oBudgetLine);

                            Budgets tempBudget = (from c in db.Budgets.Where(x => x.Id == oBudget.Id)
                                                  select c).FirstOrDefault();

                            tempBudget.Fee = oBudget.Fee;
                            tempBudget.Total = oBudget.Total;

                            db.SaveChanges();
                        }
                        AppController.AuditAction(new Audit { Module = "Linea Presupuesto", Action = "Crear", Date = DateTime.Now });
                        response = oBudgetLine.Id.ToString();

                    }
                    else
                        Redirect("~/Views/Error/NotAuthorized.cshtml");
                }
                else
                {
                    if (AppController.IsAuthorized(Common.AppEnums.Permissions.PROJECT_EDIT))
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            BudgetLines tempBudgetLine = (from c in db.BudgetLines.Where(x => x.Id == oBudgetLine.Id && x.BudgetId == oBudgetLine.BudgetId)
                                                          select c).FirstOrDefault();

                            Budgets tempBudget = (from c in db.Budgets.Where(x => x.Id == oBudget.Id)
                                                  select c).FirstOrDefault();

                            tempBudget.Fee = oBudget.Fee;
                            tempBudget.Total = (tempBudget.Total - (tempBudgetLine.Price* tempBudgetLine.Quantity)) + (oBudgetLine.Price * oBudgetLine.Quantity);

                            tempBudgetLine.Description = oBudgetLine.Description;
                            tempBudgetLine.Quantity = oBudgetLine.Quantity;
                            tempBudgetLine.Price = oBudgetLine.Price;
                            tempBudgetLine.UOM = oBudgetLine.UOM;
                            tempBudgetLine.BudgetId = oBudgetLine.BudgetId;

                            db.SaveChanges();
                        }

                        AppController.AuditAction(new Audit { Module = "Linea Presupuesto", Action = "Editar", Date = DateTime.Now });
                        response = "OK";
                    }
                }
            }
            catch (Exception e)
            {
                response = "Ocurrió un error en el proceso de almacenado.";
            }
            return Json(new { result = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBudgetLine(int budgetLineId, Budgets oBudget)
        {

            bool response = true;
            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    BudgetLines oBudgetLine = new BudgetLines();
                    oBudgetLine = (from c in db.BudgetLines.Where(x => x.Id == budgetLineId)
                               select c).FirstOrDefault();

                    Budgets tempBudget = (from c in db.Budgets.Where(x => x.Id == oBudget.Id)
                                          select c).FirstOrDefault();

                    tempBudget.Fee = oBudget.Fee;
                    tempBudget.Total = tempBudget.Total - (oBudgetLine.Price*oBudgetLine.Quantity);

                    db.BudgetLines.Remove(oBudgetLine);
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