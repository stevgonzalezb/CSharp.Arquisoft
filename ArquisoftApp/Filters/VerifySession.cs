using ArquisoftApp.Controllers;
using ArquisoftApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquisoftApp.Filters
{
    public class VerifySession : ActionFilterAttribute
    {
        private Users oUser;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);

                oUser = (Users)HttpContext.Current.Session["user"];

                if(oUser == null)
                {
                    if(filterContext.Controller is LoginController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Login");
                    }
                }

            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Login");
            }
        }
    }
}