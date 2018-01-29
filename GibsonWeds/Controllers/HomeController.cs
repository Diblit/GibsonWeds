using GibsonWeds.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GibsonWeds.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Story()
        {
            ViewBag.Message = "";

            return View();
        }
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.returnUrl = ReturnUrl;

            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Guest/RSVP");
            }

            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            UserTicket.clearUserSession();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            return Redirect("~/Home/Login");
        }
    }
}