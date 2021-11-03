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
            SetSessionData();
            return View("~/Views/Maintenance/ProjectList.cshtml");
        }

        public ActionResult Instance(int Id)
        {
            SetSessionData();
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
            var queryString = @"  SELECT A.Id, A.[Name], A.SiteArea, A.ConstructionArea, A.Levels, A.ConstructionSystem, A.Comments, A.MasterBuilder, B.[Name] +' '+ B.Last_Name ClientName, A.IdClient
                                    FROM Projects A INNER JOIN Clients B ON A.IdClient = B.IdClient
                                    WHERE A.Id = " + projectId+" FOR JSON PATH";

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

        public JsonResult Save(Projects oProject)
        {
            String response = "";
            try
            {

                if (oProject.Id == 0)
                {
                    if (AppController.IsAuthorized(Common.AppEnums.Permissions.PROJECT_ADD))
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            db.Projects.Add(oProject);
                            db.SaveChanges();
                        }
                        AppController.AuditAction(new Audit { Module = "Proyectos", Action = "Crear", Date = DateTime.Now });
                        response = oProject.Id.ToString();

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
                            Projects tempProject = (from c in db.Projects.Where(x => x.Id == oProject.Id)
                                                   select c).FirstOrDefault();

                            tempProject.Name = oProject.Name;
                            tempProject.SiteArea = oProject.SiteArea;
                            tempProject.ConstructionArea = oProject.ConstructionArea;
                            tempProject.Levels = oProject.Levels;
                            tempProject.ConstructionSystem = oProject.ConstructionSystem;
                            tempProject.MasterBuilder = oProject.MasterBuilder;
                            tempProject.Comments = oProject.Comments;
                            tempProject.IdClient = oProject.IdClient;

                            db.SaveChanges();
                        }
                        AppController.AuditAction(new Audit { Module = "Proyectos", Action = "Editar", Date = DateTime.Now });
                        response = "OK";
                    }
                }
            }
            catch(Exception e)
            {
                response = "Ocurrió un error en el proceso de almacenado.";
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