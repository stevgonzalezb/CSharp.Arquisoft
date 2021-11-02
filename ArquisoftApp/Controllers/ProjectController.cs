using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public JsonResult ListProjects()
        {
            var data = "[]";
            var ArquisoftConnection = AppController.GetConnectionString();
            var queryString = @"  SELECT A.Id, A.[Name], A.SiteArea, A.ConstructionArea, A.Levels, A.ConstructionSystem, A.MasterBuilder, B.[Name] +' '+ B.Last_Name ClientName
                                    FROM Projects A INNER JOIN Clients B ON A.IdClient = B.IdClient
                                    FOR JSON PATH";

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

        public JsonResult GetGeneralData(int projectId)
        {
            var data = "[]";
            var ArquisoftConnection = AppController.GetConnectionString();
            var queryString = @"  SELECT A.Id, A.[Name], A.SiteArea, A.ConstructionArea, A.Levels, A.ConstructionSystem, A.MasterBuilder, B.[Name] +' '+ B.Last_Name ClientName
                                    FROM Projects A INNER JOIN Clients B ON A.IdClient = B.IdClient
                                    WHERE A.Id = "+ projectId+" FOR JSON PATH";

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

        public JsonResult ListProjects2()
        {
            List<Projects> projectList = new List<Projects>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                projectList = (from p in db.Projects.Where(p => p.IdState != (int)Common.AppEnums.States.DELETE)
                               //join c in db.Clients on p.IdClient equals c.IdClient
                               //where p.IdState != (int)Common.AppEnums.States.DELETE
                               select p).ToList();
            }
            return Json(new { data = projectList }, JsonRequestBehavior.AllowGet);
        }
    }
}