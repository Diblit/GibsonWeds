using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace GibsonWeds.Utility
{
    public static class UserAccessValues
    {
        /* ------------------------------------------------- */
        // Auth MVC
        /* ------------------------------------------------- */

        public static long loggedInUserID(this Controller c)
        {
            return HttpContext.Current.userTicket().userID;
        }

        public static string loggedInUserName(this Controller c)
        {
            return HttpContext.Current.userTicket().UserName;
        }

        /* ------------------------------------------------- */
        // Auth Api
        /* ------------------------------------------------- */
        public static long loggedInUserID(this ApiController c)
        {
            return HttpContext.Current.userTicket().userID;
        }

        public static string loggedInUserName(this ApiController c)
        {
            return HttpContext.Current.userTicket().UserName;
        }
    }
}