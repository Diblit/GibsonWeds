using GibsonWeds.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace GibsonWeds.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);            
            RouteTable.Routes.MapMvcAttributeRoutes();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.EnsureInitialized();
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // look if any security information exists for this request
            if (HttpContext.Current.User != null)
            {
                // see if this user is authenticated, any authenticated cookie (ticket) exists for this user
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    // see if the authentication is done using FormsAuthentication
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get the roles stored for this request from the ticket
                        // get the identity of the user
                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;

                        //Get the form authentication ticket of the user
                        FormsAuthenticationTicket ticket = identity.Ticket;

                        // Set Sliding Expiration
                        FormsAuthentication.RenewTicketIfOld(ticket);

                        //Get the roles stored as UserData into ticket
                        //string[] roles = ticket.UserData.Split(',');
                        string[] roles = ticket.UserData.Split('|').Last().Split(',');

                        //Create general principal and assign it to current request
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(identity, roles);
                    }
                }
            }
        }
        void Application_EndRequest(object sender, EventArgs e)
        {
            DataAccess.CleanUp();
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }
    }
}
