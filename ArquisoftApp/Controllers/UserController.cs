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
        public ActionResult Index()
        {
            SetSessionData();
            return View("~/Views/Maintenance/UserMaintenance.cshtml");
        }

        public JsonResult List()
        {

            List<Users> usersList = new List<Users>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                usersList = (from u in db.Users.Where(x => x.Enable == true)
                               select u).ToList();
            }
            return Json(new { data = usersList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int userId)
        {
            Users user = new Users();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                user = (from p in db.Users.Where(x => x.Id == userId && x.Enable == true)
                            select p).FirstOrDefault();
            }

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(Users oUser)
        {

            String response = "OK";
            var validateUser = String.Empty;
            //Creo el objeto oData para poder almacenar todos los datos que trae el objeto oUser desde el HTML
            //Asigno todos los datos pero el password lo envio a la clase Encryption para que se 
            //encripte con SHA256, una vez devuelto guardo el objeto oData que es una copia de oUser
            // solo que con la contraseña encriptada
            var oData = new Users
            {
                Id = oUser.Id,
                Name = oUser.Name,
                Last_Name = oUser.Last_Name,
                Email = oUser.Email,
                Password = AppController.Encrypt(oUser.Password),
                Username = oUser.Username,
                Enable = oUser.Enable,
            };

            try
            {

                if (oData.Id == 0)
                {
                    validateUser = UserExists(oData);
                                        
                    if (validateUser == string.Empty) 
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            db.Users.Add(oData);
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
                        Users tempPersona = (from p in db.Users
                                             where p.Id == oData.Id
                                             select p).FirstOrDefault();

                        tempPersona.Name = oData.Name;
                        tempPersona.Last_Name = oData.Last_Name;
                        tempPersona.Email = oData.Email;
                        tempPersona.Password = oData.Password;
                        tempPersona.Username = oData.Username;
                        tempPersona.Enable = oData.Enable;

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

        public JsonResult Delete(int userId)
        {
            bool response = true;
            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    Users oUser = new Users();
                    oUser = (from p in db.Users.Where(x => x.Id == userId && x.Enable == true)
                                select p).FirstOrDefault();

                    oUser.Enable = false;
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