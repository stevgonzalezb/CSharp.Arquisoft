using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class SettingController : Controller
    {
        // GET: Setting
        [Filters.VerifyRole(Permission = 0)]
        public ActionResult Index()
        {
            SetSessionData();
            return View("~/Views/Maintenance/SettingMaintenance.cshtml");
        }

        [Filters.VerifyRole(Permission = 0)]
        public JsonResult GetSettings()
        {
            Settings settings = null;

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                settings = (from p in db.Settings
                        select p).FirstOrDefault();
            }


            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = 0)]
        public JsonResult SaveSettings(Settings oSetting)
        {
            String response = "OK";

            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    Settings tempSettings = (from p in db.Settings
                                      select p).FirstOrDefault();

                    tempSettings.CompanyName = oSetting.CompanyName ?? tempSettings.CompanyName;
                    //tempSettings.CompanyId = oSetting.CompanyId ?? tempSettings.CompanyId;
                    tempSettings.CompanyEmail = oSetting.CompanyEmail ?? tempSettings.CompanyEmail;
                    tempSettings.CompanyPhone = oSetting.CompanyPhone ?? tempSettings.CompanyPhone;
                    tempSettings.CompanyAddress = oSetting.CompanyAddress ?? tempSettings.CompanyAddress;

                    db.SaveChanges();
                }
                AppController.AuditAction(new Audit { Module = "Configuraciones", Action = "Actualiza Datos", Date = DateTime.Now });
            }
            catch(Exception e)
            {
                response = "Ocurrió un error en el proceso de almacenado de las configuraciones.";
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