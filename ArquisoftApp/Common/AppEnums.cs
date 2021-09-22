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
            CLIENTS = 1,
            MATERIALS = 2,
            PROJECTS = 3,
            VENDOR_MATERIALS = 4
        }

        public enum Permissions
        {
            #region Admin
            ADMIN_ROLE = 1,
            #endregion

            #region Clients
            CLIENT_READ = 1,
            CLIENT_EDIT = 2,
            CLIENT_DELETE = 3,
            CLIENT_ADD = 4,
            #endregion

            #region Materials
            MATERIAL_READ = 5,
            MATERIAL_EDIT = 6,
            MATERIAL_DELETE = 7,
            MATERIAL_ADD = 8,
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