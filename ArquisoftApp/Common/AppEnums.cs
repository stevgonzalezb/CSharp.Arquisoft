using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArquisoftApp.Common
{
    public class AppEnums
    {

        public enum Modules
        {
            USERS = 1,
            ROLES = 2,
            CLIENTS = 3,
            MATERIALS = 4,
            PROJECTS = 5,
            VENDOR_MATERIALS = 6
        }

        public enum Permissions
        {
            #region Admin
            ADMIN_ROLE = 1,
            #endregion

            #region Clients
            CLIENT_READ = 5,
            CLIENT_EDIT = 6,
            CLIENT_DELETE = 7,
            CLIENT_ADD = 8,
            #endregion
        }

        public enum States
        {
            #region States
            ACTIVE = 1,
            DISABLE = 2,
            DELETE = 3,
            #endregion
        }
    }
}