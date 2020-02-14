using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Models;

namespace WatsonTracker.Controllers
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


        private ApplicationDbContext db = new ApplicationDbContext();
        [ChildActionOnly]
        public PartialViewResult _SideNav()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            return PartialView(user);
        }
        public PartialViewResult _RightTopNav()
        {
            var userId = User.Identity.GetUserId();
            var notification = db.TicketNotifications.Where(t => t.RecipientId == userId).ToList();

            return PartialView(notification);
        }
    }
}