using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ArquisoftApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View("~/Views/Login.cshtml");
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                using (Models.ArquisoftEntities db = new Models.ArquisoftEntities())
                {
                    string passwordUser = password;
                    string passwordEncryt = AppController.Encrypt(passwordUser);

                    Models.Users oUser = (from d in db.Users
                                 where d.Username == username.Trim() && d.Password == passwordEncryt.Trim()
                                 select d).FirstOrDefault();
                    
                    if (oUser == null)
                    {
                        //Page page = System.Web.HttpContext.Current.Handler as Page;
                        //page.ClientScript.RegisterStartupScript(GetType(), "script", "alert('Record Successfuly saved');", true);
                        //page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showContent();", true);
                        ViewBag.Error = "Usuario o contraseña invalida";
                        return View("~/Views/Login.cshtml");
                    }
                    
                    Session["User"] = oUser;
                    
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }
    }
}