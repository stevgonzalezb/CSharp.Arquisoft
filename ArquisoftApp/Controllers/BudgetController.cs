using ArquisoftApp.Models;
using ArquisoftApp.Models.ReportsModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
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
            BudgetReport oReport = BuildReportModel(Id);

            return View("~/Views/Reports/_BudgetReport.cshtml", oReport);
        }

        public JsonResult SendReportToEmail(int BudgedId)
        {

            Settings settings = null;
            var email = "";
            var response = "OK";

            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    settings = (from p in db.Settings select p).FirstOrDefault();

                    email = (from b in db.Budgets
                             join p in db.Projects on b.ProjectId equals p.Id
                             join c in db.Clients on p.IdClient equals c.IdClient
                             where b.Id == BudgedId
                             select c.Email).FirstOrDefault();
                }

                BudgetReport oReport = BuildReportModel(1);
                string HTMLStringWithModel = ArquisoftApp.Common.RazorViewToStringHelper.RenderViewToString(this, "~/Views/Reports/_BudgetReport.cshtml", oReport);

                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress(settings.SMTP_Email, "Arquisoft");// replace with valid value
                Msg.Subject = "Presupuesto #" + BudgedId.ToString();
                Msg.To.Add(email); //replace with correct values

                Msg.Body = HTMLStringWithModel; //here is the razor view body

                Msg.IsBodyHtml = true;
                Msg.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = settings.SMTP_Server;
                smtp.Port = Int32.Parse(settings.SMTP_Port);
                smtp.Credentials = new System.Net.NetworkCredential(settings.SMTP_Email, settings.SMTP_Password);// replace with valid value
                smtp.EnableSsl = (bool)settings.SMTP_SSL;
                smtp.Timeout = 50000;

                smtp.Send(Msg);
            }
            catch (Exception e)
            {
                response = e.Message;
            }

            return Json(new { data = attList }, JsonRequestBehavior.AllowGet);
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
                else
                {
                    if (AppController.IsAuthorized(Common.AppEnums.Permissions.PROJECT_EDIT))
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {

                            Budgets tempBudget = (from c in db.Budgets.Where(x => x.Id == oBudget.Id)
                                                  select c).FirstOrDefault();

                            tempBudget.Fee = oBudget.Fee;
                            db.SaveChanges();
                        }
                        AppController.AuditAction(new Audit { Module = "Presupuestos", Action = "Editar Honorario", Date = DateTime.Now });
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
            //var queryString = string.Format(@"SELECT A.Id [Budget], A.Fee, A.Total Subtotal, CONVERT(DECIMAL(18,1), ((A.Total*(CONVERT(decimal(10,2),A.Fee)/100))+A.Total)) Total, Lines.*
            //                                    FROM Budgets A 
	           //                                     LEFT JOIN BudgetLines Lines ON A.Id = Lines.BudgetId
            //                                    WHERE A.Id = {0} 
            //                                    FOR JSON AUTO", budgetId);

            var queryString = string.Format(@"DECLARE @result NVARCHAR(max);
                                            SET @result = (SELECT A.Id [Budget], A.Fee, A.Total Subtotal, CONVERT(DECIMAL(18,1), ((A.Total*(CONVERT(decimal(10,2),A.Fee)/100))+A.Total)) Total, Lines.*
                                                           FROM Budgets A 
	                                                       LEFT JOIN BudgetLines Lines ON A.Id = Lines.BudgetId
                                                           WHERE A.Id = {0} 
                                                           FOR JSON AUTO)
                                            SELECT @result;", budgetId);

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

        private BudgetReport BuildReportModel(int Id)
        {
            Settings CompanyData = null;
            BudgetReport oReport = new BudgetReport();
            oReport.CurrentUser = AppController.GetSessionUser();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                oReport.Budget = (from p in db.Budgets.Where(b => b.Id == Id && b.IdState == (int)Common.AppEnums.States.ACTIVE)
                                  select p).FirstOrDefault();

                oReport.Project = (from p in db.Projects.Where(b => b.Id == oReport.Budget.ProjectId && b.IdState == (int)Common.AppEnums.States.ACTIVE)
                                   select p).FirstOrDefault();

                oReport.BudgetLines = (from p in db.BudgetLines.Where(b => b.BudgetId == Id)
                                       select p).ToList();

                oReport.Customer = (from p in db.Clients.Where(b => b.IdClient == oReport.Project.IdClient && b.idState == (int)Common.AppEnums.States.ACTIVE)
                                    select p).FirstOrDefault();

                CompanyData = (from p in db.Settings
                               select p).FirstOrDefault();
            }

            oReport.CompanyName = CompanyData.CompanyName;
            oReport.CompanyEmail = CompanyData.CompanyEmail;
            oReport.CompanyAddress = CompanyData.CompanyAddress;
            oReport.CompanyId = CompanyData.CompanyId;
            oReport.CompanyPhone = CompanyData.CompanyPhone;

            return oReport;
        }
    }
}