using GibsonWeds.DAL.Classes.Login;
using GibsonWeds.Models;
using GibsonWeds.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace GibsonWeds.Api
{
    public class LoginApiController : ApiController
    {
        [HttpPost]
        [Route("api/login")]
        public LoginResult userLogin(LoginInfo info)
        {
            var currentContext = HttpContext.Current;

            if (currentContext.Session != null)
                currentContext.Session.Clear();

            if (!ModelState.IsValid || info == null)
                throw new FormatException();

            // No Email
            if (string.IsNullOrEmpty(info.Email))
                return new LoginResult { isSuccess = false, errorText = "Invalid Credentials!" };

            // No Password
            var IsAuthenticated = (currentContext.User != null) && currentContext.User.Identity.IsAuthenticated;
            if (string.IsNullOrEmpty(info.Password) && !IsAuthenticated)
                return new LoginResult { isSuccess = false, errorText = "Invalid Credentials!" };

            //get user details
            var user = bl_Login.Get(info.Email, info.Password);

            //validate user
            if (user == null)
                return new LoginResult { isSuccess = false, errorText = "Login Failed - Incorrect credentials entered." };

            if (!user.isActive)
                return new LoginResult { isSuccess = false, errorText = "Your account is not activated - please contact support." };            

            var role = user.isGuest ? "User" : "Admin";

            var baseLink = user.isGuest ? "/Guest/RSVP" : "/Admin/AdminDash";

            var postback = Config.url_relative_path_base + baseLink;

            // login with account id and user id
            var postbackUrl = createTicket(user.userID, user.Name + " " + user.LastName, role, postback, info.returnUrl);

            return new LoginResult { isSuccess = true, returnUrl = postbackUrl, userType = role };

        }
        //create ticket by add user detail to userSession
        internal string createTicket(long userID, string FullName, string roles, string postbackUrl, string returnUrl)
        {
            var context = HttpContext.Current;

            UserTicket s = new UserTicket
            {
                userID = userID,
                UserName = FullName,
                roles = roles.Split(',')
            };
            string userData = s.toString();

            //Create Form Authentication ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                (1, userID.ToString(), DateTime.UtcNow, DateTime.UtcNow.AddMinutes(150), false, userData, FormsAuthentication.FormsCookiePath);

            //For security reasons we may hash the cookies
            string hashCookies = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
            
            // add the cookie to user browser
            context.Response.Cookies.Add(cookie);

            // get the requested page
            // string returnUrl = context.Request.QueryString["ReturnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
                postbackUrl = returnUrl;

            //navigate to page
            if (!string.IsNullOrEmpty(postbackUrl))
                return postbackUrl;

            return postbackUrl;
        }
    }
}