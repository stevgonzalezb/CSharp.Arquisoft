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
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static bool IsAuthorized(Common.AppEnums.Permissions value )
        {

            try
            {
                //var oUser = (Users)HttpContext.Current.Session["user"];
            }
            catch(Exception)
            {

            }


            return false;
        }


        public static Users GetSessionUser()
        {
            return new Users();
        }
    }
}