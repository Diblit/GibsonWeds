using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GibsonWeds.Utility
{
    public class UserTicket
    {
        #region Authentication Ticket (Cookie) Info
        public long userID;
        public string UserName;
        public bool isAdmin; //admin = true, agent = false
        public bool isUser;
        public string[] roles;
        public long companyID;
        public UserTicket() { }

        public UserTicket(string userData)
        {
            string[] uD = userData.Split('|');
            userID = Convert.ToInt64(uD[0]);
            UserName = uD[1];
            roles = uD[2].Split(',');

        }

        public bool IsInRole(string role)
        {
            return Array.BinarySearch(roles, role) >= 0 ? true : false;
        }

        public string toString()
        {
            string ss = roles.ToString();
            string r = "";
            for (int i = 0; i < roles.Length; i++)
            {
                r += roles[i];
                if (i != roles.Length - 1)
                    r += ",";
            }
            return string.Format("{0}|{1}|{2}", userID, UserName, r);
        }

        public static void clearUserSession()
        {
            for (int i = 0; i < HttpContext.Current.Session.Keys.Count; i++)
            {
                var key = HttpContext.Current.Session.Keys[i];
                if (key.StartsWith("UserSession_Extra_"))
                {
                    HttpContext.Current.Session.Remove(key);
                }
            }
        }
        #endregion
    }

    public static class UserTicketExt
    {
        public static UserTicket userTicket(this HttpContext context)
        {
            if (HttpContext.Current.User != null)
            {
                var ctxUT = (UserTicket)context.Items["UserTicketCtxKey"];
                if (ctxUT == null)
                {
                    FormsIdentity identity = (FormsIdentity)context.User.Identity;
                    ctxUT = new UserTicket(identity.Ticket.UserData);
                    HttpContext.Current.Items["UserTicketCtxKey"] = ctxUT;
                }

                return ctxUT;
            }
            return null;
        }
    }
}