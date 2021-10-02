using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using ArquisoftApp.Models;
using System.IO;
using System.Web.Configuration;
using System.Configuration;

namespace ArquisoftApp.Controllers
{
    public class AppController : Controller
    {

        public static string Encrypt(string str)
        {
            string encryptStr = string.Empty;

            try
            {
                string EncryptionKey = "3nf0rce3ncr1pt";
                byte[] clearBytes = Encoding.Unicode.GetBytes(str);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        encryptStr = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch(Exception)
            {
            }

            return encryptStr;
        }

        public static string Decrypt(string cipherStr)
        {
            string decryptStr = string.Empty;

            try
            {
                string EncryptionKey = "3nf0rce3ncr1pt";
                cipherStr = cipherStr.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherStr);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        decryptStr = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
            }

            return decryptStr;
        }

        public static bool IsAuthorized(Common.AppEnums.Permissions value )
        {
            RoleOperations hasOperation = null;

            try
            {
                var currentUser = GetSessionUser();

                if (currentUser.IdRole == (int)Common.AppEnums.Permissions.ADMIN_ROLE)
                    hasOperation = new RoleOperations();
                else
                {
                    using (ArquisoftEntities db = new ArquisoftEntities())
                    {
                        hasOperation = (from u in db.RoleOperations.Where(x => x.IdRole == currentUser.IdRole && x.IdOperation == (int)value)
                                        select u).FirstOrDefault();
                    }
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

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ArquisoftSQL"].ConnectionString;
        }
    }
}