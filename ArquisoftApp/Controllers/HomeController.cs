using ArquisoftApp.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SetSessionData();
            return View();
        }

        public JsonResult GetDashboardData()
        {
            var data = "[]";
            var ArquisoftConnection = AppController.GetConnectionString();
            var queryString = string.Format(@"  SELECT [Status] 'Value', COUNT(1) [Count]
                                                FROM Projects 
                                                WHERE [Status] IS NOT NULL AND IdState = {0}
                                                GROUP BY [Status]
                                                UNION ALL
                                                SELECT 'Clientes', COUNT(1) 
                                                FROM Clients
                                                WHERE idState = {0}
                                                FOR JSON PATH", (int)Common.AppEnums.States.ACTIVE);

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
        }

        private void SetSessionData()
        {
            var oUser = (Models.Users)System.Web.HttpContext.Current.Session["user"];
            ViewBag.UserId = oUser.Id;
            ViewBag.UserName = oUser.Name + " " + oUser.Last_Name;
        }
    }
}