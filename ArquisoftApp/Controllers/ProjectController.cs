using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.PROJECT_READ)]
        public ActionResult Index()
        {
            SetSessionData();
            return View("~/Views/Maintenance/ProjectList.cshtml");
        }

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.PROJECT_READ)]
        public ActionResult Instance(int Id)
        {
            SetSessionData();
            return View("~/Views/Maintenance/ProjectMaintenance.cshtml");
        }

        public JsonResult ListProjects()
        {

            var data = "[]";
            var ArquisoftConnection = AppController.GetConnectionString();
            var queryString = @" SELECT A.Id, A.[Name], A.SiteArea, A.ConstructionArea, A.Levels, A.ConstructionSystem, A.MasterBuilder, B.[Name] +' '+ B.Last_Name ClientName, A.Status
                                    FROM Projects A INNER JOIN Clients B ON A.IdClient = B.IdClient
                                    WHERE A.IdState = " + (int)Common.AppEnums.States.ACTIVE + " FOR JSON PATH";

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
            var queryString = @"  SELECT A.Id, A.[Name], A.SiteArea, A.ConstructionArea, A.Levels, A.ConstructionSystem, A.Comments, A.MasterBuilder, B.[Name] +' '+ B.Last_Name ClientName, A.IdClient, A.Status
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

        [HttpPost]
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
                            tempProject.Status = oProject.Status;

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

        public JsonResult Delete(int projectId)
        {
            bool response = true;
            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    Projects oProject = new Projects();
                    oProject = (from c in db.Projects.Where(x => x.Id == projectId)
                               select c).FirstOrDefault();

                    oProject.IdState = (int)Common.AppEnums.States.DELETE;

                    db.SaveChanges();
                }
                AppController.AuditAction(new Audit { Module = "Project", Action = "Eliminar", Date = DateTime.Now });
            }
            catch
            {
                response = false;
            }

            return Json(new { result = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadAttachment()
        {
            string response = "";
            try
            {
                if (Request.Files.Count > 0)
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    HttpPostedFileBase file = files[0];
                    string fname;
                    string projectId = Request.Form[0];

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    // Get the complete folder path and store the file inside it.  
                    fname = Path.Combine(Server.MapPath("~/ProjectAttachments/"), projectId + "_" + fname);
                    file.SaveAs(fname);

                    Attachments oAttach = new Attachments();
                    oAttach.FileName = projectId + "_" + file.FileName;
                    oAttach.FileSize = (file.ContentLength / 1024).ToString(); // saving in KB 
                    oAttach.ProjectId = Int32.Parse(Request.Form[0]);

                    using (ArquisoftEntities db = new ArquisoftEntities())
                    {
                        db.Attachments.Add(oAttach);
                        db.SaveChanges();
                    }
                    AppController.AuditAction(new Audit { Module = "Proyectos", Action = "Agregar adjunto", Date = DateTime.Now });

                    response = "Archivo subido correctamente";
                }
                else
                {
                    response = "No hay archivos seleccionados";
                }
                

            }
            catch(Exception e)
            {
                response = "Ocurrió un error guardando el adjunto";
            }
            return Json(new { result = response }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListAttachments(int projectId)
        {
            List<Attachments> attList = new List<Attachments>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                attList = (from p in db.Attachments.Where(a => a.ProjectId == projectId)
                               select p).ToList();
            }
            return Json(new { data = attList }, JsonRequestBehavior.AllowGet);
        }

        public void DownloadFile(int Id, int projectId)
        {
            try
            {
                Attachments att = new Attachments();

                using (ArquisoftEntities db = new ArquisoftEntities())
                {

                    att = (from p in db.Attachments.Where(a => a.ProjectId == projectId && a.Id == Id)
                               select p).FirstOrDefault();
                }

                if(att != null)
                {
                    string filePath = "~/ProjectAttachments/" + att.FileName;
                    Response.ContentType = "image/png";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                    Response.WriteFile(filePath);
                    Response.End();
                }


            }
            catch(Exception e)
            {

            }
        }

        public JsonResult DeleteFile(int Id, int projectId)
        {
            try
            {
                Attachments att = new Attachments();

                using (ArquisoftEntities db = new ArquisoftEntities())
                {

                    att = (from p in db.Attachments.Where(a => a.ProjectId == projectId && a.Id == Id)
                           select p).FirstOrDefault();
                }

                if (att != null)
                {
                    string filePath = Path.Combine(Server.MapPath("~/ProjectAttachments/"), att.FileName);
                    
                    if ((System.IO.File.Exists(filePath)))
                    {
                        //Delete file from directory
                        System.IO.File.Delete(filePath);

                        // Delet record from DB
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            db.Attachments.Attach(att);
                            db.Attachments.Remove(att);
                            db.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception e)
            {
                
            }
            return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
        }

        private void SetSessionData()
        {
            var oUser = (Models.Users)System.Web.HttpContext.Current.Session["user"];
            ViewBag.UserId = oUser.Id;
            ViewBag.UserName = oUser.Name + " " + oUser.Last_Name;
        }
    }
}