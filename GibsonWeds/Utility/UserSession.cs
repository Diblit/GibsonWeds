using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GibsonWeds.Utility
{
    public class UserSession
    {
        public string Username;
    }
    public static class UserSessionExt
    {
        public static UserSession UserSession(this HttpContext context)
        {
            if (HttpContext.Current.User != null)
            {
                var ctxUS = (UserSession)HttpContext.Current.Session["UserSessionCtxKey"];

                //var us = HttpContext.Current.userTicket();

                if (ctxUS == null)
                {
                    HttpContext.Current.Session["UserSessionCtxKey"] = ctxUS;
                }

                return ctxUS;
            }
            return null;
        }
    }
}