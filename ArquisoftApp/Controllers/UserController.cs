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
        [Filters.VerifyRole(Permission = 0)]
        public ActionResult Index()
        {
            SetSessionData();
            return View("~/Views/Maintenance/UserMaintenance.cshtml");
        }

        [Filters.VerifyRole(Permission = 0)]
        public JsonResult List()
        {

            List<Users> usersList = new List<Users>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                usersList = (from u in db.Users.Where(x => x.IdState != (int)Common.AppEnums.States.DELETE && x.IdRole != (int)Common.AppEnums.Permissions.ADMIN_ROLE)
                               select u).ToList();
            }
            return Json(new { data = usersList }, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = 0)]
        public JsonResult Get(int userId)
        {
            Users user = new Users();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                user = (from p in db.Users.Where(x => x.Id == userId && x.IdRole != (int)Common.AppEnums.Permissions.ADMIN_ROLE)
                            select p).FirstOrDefault();
            }

            user.Password = AppController.Decrypt(user.Password);

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = 0)]
        [HttpPost]
        public JsonResult Save(Users oUser)
        {

            String response = "OK";
            var validateUser = String.Empty;
            
            try
            {

                if (oUser.Id == 0)
                {
                    validateUser = UserExists(oUser);
                                        
                    if (validateUser == string.Empty) 
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            oUser.Password = AppController.Encrypt(oUser.Password);
                            db.Users.Add(oUser);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        response = validateUser;
                    }
                }
                else
                {
                    using (ArquisoftEntities db = new ArquisoftEntities())
                    {
                        Users tempUser = (from p in db.Users
                                             where p.Id == oUser.Id
                                             select p).FirstOrDefault();

                        tempUser.Name = oUser.Name;
                        tempUser.Last_Name = oUser.Last_Name;
                        tempUser.Email = oUser.Email;
                        tempUser.Password = AppController.Encrypt(oUser.Password);
                        tempUser.Username = oUser.Username;
                        tempUser.IdState = oUser.IdState;
                        tempUser.IdRole = oUser.IdRole;

                        db.SaveChanges();
                    }
                }
            }
            catch
            {
                response = "Ocurrió un error en el proceso de almacenado.";

            }

            return Json(new { result = response }, JsonRequestBehavior.AllowGet) ;

        }

        [Filters.VerifyRole(Permission = 0)]
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

                    oUser.IdState = (int)Common.AppEnums.States.DELETE;
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

        private String UserExists(Users oUser)
        {

            String returnString = string.Empty;
            Users isValidUsername, isValidEmail;

            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    isValidUsername = (from p in db.Users
                                         where p.Username.ToLower() == oUser.Username.ToLower()
                                         select p).FirstOrDefault();

                    isValidEmail = (from p in db.Users
                                             where p.Email.ToLower() == oUser.Email.ToLower()
                                             select p).FirstOrDefault();

                }

                if (isValidUsername != null)
                {
                    returnString = "El nombre de usuario ya existe en el sistema.";
                }

                if (isValidEmail != null)
                {
                    returnString = "El correo electrónico existe en el sistema.";
                }
            }
            catch
            {

            }

            return returnString;
        }
    }
}