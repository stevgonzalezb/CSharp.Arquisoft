using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArquisoftApp.Common
{
    public class AppEnums
    {
        public enum Permissions
        {
            #region Users
            USER_READ = 1,
            USER_EDIT = 2,
            USER_DELETE = 3,
            USER_ADD = 4,
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