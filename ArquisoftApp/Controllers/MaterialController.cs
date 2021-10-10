using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
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

        private void SetSessionData()
        {
            var oUser = (Models.Users)System.Web.HttpContext.Current.Session["user"];
            ViewBag.UserId = oUser.Id;
            ViewBag.UserName = oUser.Name + " " + oUser.Last_Name;
        }
    }
}