using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class MaterialController : Controller
    {
        // GET: Material
        public ActionResult Index()
        {
            SetSessionData();
            return View("~/Views/Maintenance/MaterialMaintenance.cshtml");
        }

        public ActionResult Vendors()
        {
            SetSessionData();
            return View("~/Views/Maintenance/VendorMaterialMaintenance.cshtml");
        }

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.VENDOR_MAT_READ)]
        public JsonResult ListVendorMaterials()
        {

            List<VendorMaterials> materialList = new List<VendorMaterials>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                materialList = (from c in db.VendorMaterials
                              select c).ToList();
            }
            return Json(new { data = materialList }, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.VENDOR_MAT_READ)]
        public JsonResult GetVendorMaterial(int materialId)
        {
            VendorMaterials oMaterial = new VendorMaterials();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                oMaterial = (from c in db.VendorMaterials.Where(x => x.Id == materialId)
                            select c).FirstOrDefault();
            }


            return Json(oMaterial, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RunScript()
        {
            Run_Scrapper();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.MATERIAL_READ)]
        public JsonResult ListMaterials()
        {
            List<Materials> materialList = new List<Materials>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                materialList = (from c in db.Materials.Where(x => x.IdState != (int)Common.AppEnums.States.DELETE)
                                select c).ToList();
            }
            return Json(new { data = materialList }, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.MATERIAL_READ)]

        public JsonResult GetMaterial(int materialId)
        {
            Materials oMaterial = new Materials();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                oMaterial = (from c in db.Materials.Where(x => x.Id == materialId)
                            select c).FirstOrDefault();
            }

            return Json(oMaterial, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(Materials oMaterial)
        {
            String response = "OK";
            var validate = String.Empty;
            try
            {

                if (oMaterial.Id == 0)
                {
                    if (AppController.IsAuthorized(Common.AppEnums.Permissions.MATERIAL_ADD))
                    {
                        //validateClient = ClientExists(oClient);

                        if (validate == string.Empty)
                        {
                            using (ArquisoftEntities db = new ArquisoftEntities())
                            {
                                db.Materials.Add(oMaterial);
                                db.SaveChanges();
                            }
                            AppController.AuditAction(new Audit { Module = "Material", Action = "Crear", Date = DateTime.Now });
                        }
                        else
                        {
                            response = validate;
                        }
                    }
                    else
                        Redirect("~/Views/Error/NotAuthorized.cshtml");
                }
                else
                {
                    if (AppController.IsAuthorized(Common.AppEnums.Permissions.MATERIAL_EDIT))
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            Materials tempMaterial = (from c in db.Materials.Where(x => x.Id == oMaterial.Id)
                                                   select c).FirstOrDefault();

                            tempMaterial.Description = oMaterial.Description;
                            tempMaterial.Price = oMaterial.Price;
                            tempMaterial.IdState = oMaterial.IdState;

                            db.SaveChanges();
                        }
                        AppController.AuditAction(new Audit { Module = "Material", Action = "Editar", Date = DateTime.Now });
                    }
                }
            }
            catch
            {
                response = "Ocurrió un error en el proceso de almacenado.";
            }
            return Json(new { result = response }, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.MATERIAL_DELETE)]

        public JsonResult DeleteMaterial(int materialId)
        {
            bool response = true;
            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    Materials oMaterial = new Materials();
                    oMaterial = (from c in db.Materials.Where(x => x.Id == materialId)
                               select c).FirstOrDefault();

                    oMaterial.IdState = (int)Common.AppEnums.States.DELETE;

                    db.SaveChanges();
                }
                AppController.AuditAction(new Audit { Module = "Material", Action = "Eliminar", Date = DateTime.Now });
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

        private void Run_Scrapper()
        {
            //System.Diagnostics.Debugger.Launch();
            //System.Diagnostics.Debugger.Break();

            var path = AppDomain.CurrentDomain.BaseDirectory + @"Scripts\\src\\scrappers\\app.py";

            string strCmdText;
            strCmdText ="python"+ AppDomain.CurrentDomain.BaseDirectory + @"Scripts\\src\\scrappers\\app.py";
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);

            //ProcessStartInfo start = new ProcessStartInfo();
            //start.FileName = "python";
            //start.Arguments = string.Format("{0}", "~/Scripts/src/scrappers/app.py");
            //start.UseShellExecute = false;
            //start.RedirectStandardOutput = true;
            //using (Process process = Process.Start(start))
            //{
            //    using (StreamReader reader = process.StandardOutput)
            //    {
            //        string result = reader.ReadToEnd();
            //        Console.Write(result);
            //    }
            //}
        }
    }
}