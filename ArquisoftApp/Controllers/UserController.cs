using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserMaintenance()
        {
            SetSessionData();
            return View("~/Views/Maintenance/UserMaintenance.cshtml");
        }

        public JsonResult List()
        {

            List<Users> usersList = new List<Users>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                usersList = (from u in db.Users
                               select u).ToList();
            }
            return Json(new { data = usersList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int userId)
        {
            Users user = new Users();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                user = (from p in db.Users.Where(x => x.Id == userId)
                            select p).FirstOrDefault();
            }

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(Users oUser)
        {

            bool response = true;
            try
            {

                if (oUser.Id == 0)
                {
                    using (ArquisoftEntities db = new ArquisoftEntities())
                    {
                        db.Users.Add(oUser);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (ArquisoftEntities db = new ArquisoftEntities())
                    {
                        Users tempPersona = (from p in db.Users
                                                where p.Id == oUser.Id
                                                select p).FirstOrDefault();

                        tempPersona.Name = oUser.Name;
                        tempPersona.Last_Name = oUser.Last_Name;
                        tempPersona.Email = oUser.Email;

                        db.SaveChanges();
                    }

                }
            }
            catch
            {
                response = false;

            }

            return Json(new { result = response }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Delete(int userId)
        {
            bool response = true;
            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    Users oUser = new Users();
                    oUser = (from p in db.Users.Where(x => x.Id == userId)
                                select p).FirstOrDefault();

                    db.Users.Remove(oUser);

                    db.SaveChanges();
                }
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