using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Helper;
using WatsonTracker.Models;
using WatsonTracker.ViewModels;

namespace WatsonTracker.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        DashboardVM vm = new DashboardVM();
        UserRolesHelper helper = new UserRolesHelper();

        public ActionResult IDashboard()
        {
            return View();
        }

        // GET: Dashboard 
        [Authorize]
        public ActionResult Index()
        {
            vm.ActiveTickets = db.Tickets.Where(t => t.TicketStatus.Name != "Complete").ToList();
            vm.TicNotAssigned = db.Tickets.Where(t => t.AssignedToUser == null).ToList();
            vm.NumProjects = db.Projects.ToList();
            vm.PriorityUrgent = db.Tickets.Where(t => t.TicketPriority.Name == "Urgent").ToList();
            vm.PriorityHigh = db.Tickets.Where(t => t.TicketPriority.Name == "High").ToList();
            vm.PriorityMedium = db.Tickets.Where(t => t.TicketPriority.Name == "Medium").ToList();
            vm.PriorityLow = db.Tickets.Where(t => t.TicketPriority.Name == "Low").ToList();
            vm.UsersAssigned = db.Users.Where(u => u.Roles.Count != 0).ToList();
            vm.Tickets = db.Tickets.ToList();

                       
            return View(vm);
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