using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using ArquisoftApp.Models;

namespace ArquisoftApp.Controllers
{
    public class AppController : Controller
    {

        public static string Encrypt(string str)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                SHA256 sha256 = SHA256Managed.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                stream = sha256.ComputeHash(encoding.GetBytes(str));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            }
            catch(Exception)
            {
            }

            return sb.ToString();
        }

        public static bool IsAuthorized(Common.AppEnums.Permissions value )
        {
            RoleOperations hasOperation = null;

            try
            {
                var currentUser = GetSessionUser();

                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    hasOperation = (from u in db.RoleOperations.Where(x => x.IdRole == currentUser.IdRole && x.IdOperation == (int)value)
                           select u).FirstOrDefault();
                }
            }
            catch(Exception)
            {
                throw;
            }

            return hasOperation == null ? false : true;
        }

        public static bool isAdmin()
        {
            Users currentUser;

            try
            {
                currentUser = GetSessionUser();
            }
            catch(Exception)
            {
                throw;
            }

            return currentUser.IdRole == (int)Common.AppEnums.Permissions.ADMIN_ROLE;
        }

        public static Users GetSessionUser()
        {
            return (Models.Users)System.Web.HttpContext.Current.Session["user"];
        }

        public ActionResult LogOut()
        {
            System.Web.HttpContext.Current.Session["User"] = null;
            return RedirectToAction("Login", "Login");
        }
    }
}