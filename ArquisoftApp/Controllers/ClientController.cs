using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.CLIENT_READ)]
        public ActionResult Index()
        {
            SetSessionData();
            return View("~/Views/Maintenance/ClientMaintenance.cshtml");
        }

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.CLIENT_READ)]
        public JsonResult List()
        {

            List<Clients> ClientList = new List<Clients>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                ClientList = (from c in db.Clients
                              select c).ToList();
            }
            return Json(new { data = ClientList }, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.CLIENT_READ)]
        public JsonResult Get(int clientId)
        {
            Clients oClients = new Clients();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                oClients = (from c in db.Clients.Where(x => x.IdClient == clientId && x.idState == 1)
                            select c).FirstOrDefault();
            }

            return Json(oClients, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(Clients oClient)
        {
            String response = "OK";
            var validateClient = String.Empty;
            try
            {
                
                if (oClient.IdClient == 0)
                {
                    if (AppController.IsAuthorized(Common.AppEnums.Permissions.CLIENT_ADD))
                    {
                        validateClient = ClientExists(oClient);

                        if (validateClient == string.Empty)
                        {
                            using (ArquisoftEntities db = new ArquisoftEntities())
                            {
                                db.Clients.Add(oClient);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            response = validateClient;
                        }
                    }
                    else
                        Redirect("~/Views/Error/NotAuthorized.cshtml");
                }
                else
                {
                    if (AppController.IsAuthorized(Common.AppEnums.Permissions.CLIENT_EDIT))
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            Clients tempCliente = (from c in db.Clients.Where(x => x.IdClient == oClient.IdClient)
                                                   select c).FirstOrDefault();

                            tempCliente.Name = oClient.Name;
                            tempCliente.Last_Name = oClient.Last_Name;
                            tempCliente.Direction = oClient.Direction;
                            tempCliente.Phone = oClient.Phone;
                            tempCliente.Email = oClient.Email;
                            tempCliente.idState = 1;

                            db.SaveChanges();
                        }
                    }
                }
            }
            catch
            {
                response = "Ocurrió un error en el proceso de almacenado.";
            }
            return Json(new { result = response }, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = Common.AppEnums.Permissions.CLIENT_DELETE)]
        public JsonResult Delete(int clientId)
        {
            bool response = true;
            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    Clients oClient = new Clients();
                    oClient = (from c in db.Clients.Where(x => x.IdClient == clientId)
                               select c).FirstOrDefault();

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

        private String ClientExists(Clients oClient)
        {

            String returnString = string.Empty;
            Clients isValidClient, isValidEmail;

            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    isValidClient = (from c in db.Clients
                                     where c.Name.ToLower() == oClient.Name.ToLower()
                                     select c).FirstOrDefault();

                    isValidEmail = (from c in db.Clients
                                    where c.Email.ToLower() == oClient.Email.ToLower()
                                    select c).FirstOrDefault();

                }

                if (isValidClient != null)
                {
                    returnString = "El nombre de cliente ya existe en el sistema.";
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