using GibsonWeds.DAL.Classes.Admin;
using GibsonWeds.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GibsonWeds.Controllers
{
    [Authorize(Roles = "User")]
    public class GuestController : Controller
    {
        // GET: Guest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WeddingInfo()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home/Login");
            }
            var userID = this.loggedInUserID();
            var isAttending = bl_GuestList.HasGuestRsvpD(userID);
            if (!isAttending)
            {
                return Redirect("~/Home/Login");
            }

            return View();
        }
        public ActionResult RSVP()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home/Login");
            }
            var userID = this.loggedInUserID();
            var hasRSVPd = bl_GuestList.HasGuestRsvpD(userID);

            var data = bl_GuestList.GetGuest(userID);
            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data });
            ViewBag.hasRsvp = hasRSVPd;
            return View();
        }
        public ActionResult Contact()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home/Login");
            }

            return View();
        }
        public ActionResult ResetPassword()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home/Login");
            }

            return View();
        }
        public ActionResult Registry()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home/Login");
            }
            var userID = this.loggedInUserID();

            var data = bl_Registry.GetRegistry(userID);
            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data });

            return View();
        }
        public ActionResult Activity()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home/Login");
            }
            var userID = this.loggedInUserID();

            var data = bl_ActivityCategory.GetActivities();
            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data });

            return View();
        }


        // GET: Guest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Guest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Guest/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Guest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Guest/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Guest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Guest/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
