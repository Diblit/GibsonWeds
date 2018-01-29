using GibsonWeds.DAL;
using GibsonWeds.DAL.Classes.Admin;
using GibsonWeds.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GibsonWeds.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        //GET: Admin/AdminDash
        public ActionResult AdminDash()
        {
            var data = bl_AdminDash.AdminDashList();
            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data });

            return View();
        }
        //GET: Admin/GuestList
        public ActionResult GuestList()
        {
            var paging = new PagingInfo
            {
                skip = 0,
                take = 10
            };

            var data = bl_GuestList.GuestList(ref paging);
            var guestCouple = bl_GuestCouples.ListGuestCouples(ref paging);
            //GibsonWedsEntities test = new GibsonWedsEntities();
            //var q = (from row in test.db_User
            //         select row).ToList();

            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data, Count = paging.result_count, CouplesList = guestCouple });

            return View();            
        }

        public ActionResult GuestCouples()
        {
            var paging = new PagingInfo
            {
                skip = 0,
                take = 10
            };

            var data = bl_GuestCouples.ListGuestCouples(ref paging);            

            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data, Count = paging.result_count });

            return View();
        }
        //GET: Admin/ActivityList
        public ActionResult ActivityList()
        {
            var paging = new PagingInfo
            {
                skip = 0,
                take = 10
            };

            var data = bl_ActivityList.ActivityList(ref paging);

            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data, Count = paging.result_count });

            return View();
        }

        public ActionResult ActivityCategory()
        {
            var paging = new PagingInfo
            {
                skip = 0,
                take = 10
            };

            var data = bl_ActivityCategory.ActivityCategoryList(ref paging);

            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data, Count = paging.result_count });

            return View();
        }

        public ActionResult Registry()
        {
            var paging = new PagingInfo
            {
                skip = 0,
                take = 10
            };

            var data = bl_Registry.RegistryList(ref paging);

            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data, Count = paging.result_count });

            return View();
        }
        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
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

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
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

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
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
