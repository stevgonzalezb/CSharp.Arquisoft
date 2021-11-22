using ArquisoftApp.Models;
using ArquisoftApp.Models.ReportsModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            SetSessionData();
            return View("~/Views/Reports/ReportsList.cshtml");
        }

        public ActionResult _ProjectsByCustomer(int cId)
        {
            Settings CompanyData = null;
            ProjectsByCustomer oReport = new ProjectsByCustomer();
            oReport.CurrentUser = AppController.GetSessionUser();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {
                oReport.Customer = (from p in db.Clients.Where(b => b.IdClient == cId && b.idState == (int)Common.AppEnums.States.ACTIVE)
                                    select p).FirstOrDefault();

                oReport.Projects = (from p in db.Projects.Where(b => b.IdClient == cId && b.IdState == (int)Common.AppEnums.States.ACTIVE)
                                    select p).ToList();

                CompanyData = (from p in db.Settings
                               select p).FirstOrDefault();
            }

            oReport.CompanyName = CompanyData.CompanyName;
            oReport.CompanyEmail = CompanyData.CompanyEmail;
            oReport.CompanyAddress = CompanyData.CompanyAddress;
            oReport.CompanyId = CompanyData.CompanyId;
            oReport.CompanyPhone = CompanyData.CompanyPhone;


            return View("~/Views/Reports/_ProjectsByCustomer.cshtml", oReport);
        }

        public ActionResult _UsedMaterials(string dates)
        {
            DateTimeFormatInfo crDtfi = new CultureInfo("es-CR", false).DateTimeFormat;
            var startDate = Convert.ToDateTime(dates.Split('-')[0], crDtfi).ToString("yyyy-MM-dd");
            var endDate = Convert.ToDateTime(dates.Split('-')[1], crDtfi).ToString("yyyy-MM-dd");

            var queryString = string.Format(@"SELECT [Description], COUNT(BudgetId) 'Cantidad'
                                                FROM BudgetLines A 
	                                                INNER JOIN Budgets B ON A.BudgetId = B.Id
                                                WHERE B.CreationDate >= '{0}' AND B.CreationDate <= '{1}'
                                                GROUP BY [Description]", startDate, endDate);

            Settings CompanyData = null;
            UsedMaterials oReport = new UsedMaterials();
            oReport.Materials = new List<string>();
            oReport.CurrentUser = AppController.GetSessionUser();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {
                CompanyData = (from p in db.Settings
                               select p).FirstOrDefault();
            }

            oReport.CompanyName = CompanyData.CompanyName;
            oReport.CompanyEmail = CompanyData.CompanyEmail;
            oReport.CompanyAddress = CompanyData.CompanyAddress;
            oReport.CompanyId = CompanyData.CompanyId;
            oReport.CompanyPhone = CompanyData.CompanyPhone;

            using (SqlConnection conn = new SqlConnection(AppController.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(queryString, conn))
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var val = string.Format(@"{0}|{1}", reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                            oReport.Materials.Add(val);
                        }
                    }
                }
            }

            return View("~/Views/Reports/_UsedMaterials.cshtml", oReport);
        }

        public ActionResult _BudgetsCreated(string dates)
        {
            DateTimeFormatInfo crDtfi = new CultureInfo("es-CR", false).DateTimeFormat;
            var startDate = Convert.ToDateTime(dates.Split('-')[0], crDtfi);
            var endDate = Convert.ToDateTime(dates.Split('-')[1], crDtfi);

            Settings CompanyData = null;
            BudgetsCreated oReport = new BudgetsCreated();
            oReport.CurrentUser = AppController.GetSessionUser();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                oReport.Budgets = (from p in db.Budgets.Where(b => b.IdState == (int)Common.AppEnums.States.ACTIVE && (b.CreationDate >= startDate && b.CreationDate <= endDate))
                                    select p).ToList();

                CompanyData = (from p in db.Settings
                               select p).FirstOrDefault();
            }

            oReport.CompanyName = CompanyData.CompanyName;
            oReport.CompanyEmail = CompanyData.CompanyEmail;
            oReport.CompanyAddress = CompanyData.CompanyAddress;
            oReport.CompanyId = CompanyData.CompanyId;
            oReport.CompanyPhone = CompanyData.CompanyPhone;

            return View("~/Views/Reports/_BudgetsCreated.cshtml", oReport);
        }

        private void SetSessionData()
        {
            var oUser = (Models.Users)System.Web.HttpContext.Current.Session["user"];
            ViewBag.UserId = oUser.Id;
            ViewBag.UserName = oUser.Name + " " + oUser.Last_Name;
        }
    }
}