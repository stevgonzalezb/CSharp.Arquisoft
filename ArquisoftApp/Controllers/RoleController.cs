using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        [Filters.VerifyRole(Permission = 0)]
        public ActionResult Index()
        {
            return View("~/Views/Maintenance/RoleMaintenance.cshtml");
        }

        [Filters.VerifyRole(Permission = 0)]
        public JsonResult List()
        {

            List<Roles> rolesList = new List<Roles>();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                rolesList = (from u in db.Roles.Where(x => x.Id != (int)Common.AppEnums.Permissions.ADMIN_ROLE)
                             select u).ToList();
            }
            return Json(new { data = rolesList }, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = 0)]
        public JsonResult Get(int roleId)
        {
            Roles role = new Roles();

            using (ArquisoftEntities db = new ArquisoftEntities())
            {

                role = (from r in db.Roles.Where(x => x.Id == roleId && x.Id != (int)Common.AppEnums.Permissions.ADMIN_ROLE)
                        select r).FirstOrDefault();
            }

            return Json(role, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerifyRole(Permission = 0)]
        [HttpPost]
        public JsonResult Save(Roles oRole)
        {
            String response = "OK";
            var validateRole = String.Empty;

            try
            {

                if (oRole.Id == 0)
                {
                    validateRole = RoleExists(oRole);

                    if (validateRole == string.Empty)
                    {
                        using (ArquisoftEntities db = new ArquisoftEntities())
                        {
                            db.Roles.Add(oRole);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        response = validateRole;
                    }
                }
                else
                {
                    using (ArquisoftEntities db = new ArquisoftEntities())
                    {
                        Roles tempUser = (from r in db.Roles
                                          where r.Id == oRole.Id
                                          select r).FirstOrDefault();

                        tempUser.Name = oRole.Name;
                        tempUser.Description = oRole.Description;

                        db.SaveChanges();
                    }
                }
            }
            catch
            {
                response = "Ocurrió un error en el proceso de almacenado.";

            }

            return Json(new { result = response }, JsonRequestBehavior.AllowGet);

        }

        private String RoleExists(Roles oRole)
        {

            String returnString = string.Empty;
            Roles isValidName;

            try
            {
                using (ArquisoftEntities db = new ArquisoftEntities())
                {
                    isValidName = (from p in db.Roles
                                   where p.Name.ToLower() == oRole.Name.ToLower()
                                   select p).FirstOrDefault();

                }

                if (isValidName != null)
                {
                    returnString = "El nombre del rol ya existe en el sistema.";
                }
            }
            catch
            {

            }

            return returnString;
        }

        [Filters.VerifyRole(Permission = 0)]
        public JsonResult GetPermissionsByRole(int roleId)
        {

            var data = "[]";
            var ArquisoftConnection = AppController.GetConnectionString();
            var queryString = @"SELECT  A.Id, A.Name, Operations.Name 'Permission', CASE WHEN RoleOp.IdOperation IS NULL THEN 0 ELSE 1 END 'HasPrivilege'
                                FROM Modules A 
                                    INNER JOIN ModuleOperations Operations ON A.Id = Operations.IdModule
                                    LEFT JOIN RoleOperations RoleOp on RoleOp.IdOperation = Operations.Id
                                WHERE RoleOp.IdRole = " + roleId+ " OR RoleOp.IdRole IS NULL FOR JSON AUTO";

            using (SqlConnection conn = new SqlConnection(ArquisoftConnection))
            {
                using (SqlCommand command = new SqlCommand(queryString, conn))
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            data = reader.GetValue(0).ToString();
                        }
                    }
                }
            }

            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

    }
}