using ArquisoftApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ArquisoftApp.Filters
{
    public class VerifyRole : ActionFilterAttribute
    {
        public ArquisoftApp.Common.AppEnums.Permissions Permission { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.Controller is UserController == true || filterContext.Controller is RoleController == true)
            {
                if (!Controllers.AppController.isAdmin())
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Error",
                        action = "NotAuthorized"
                    }));
                }
            }

            else if(!Controllers.AppController.IsAuthorized(this.Permission))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Error",
                    action = "NotAuthorized"
                }));
            }
        }
    }
}