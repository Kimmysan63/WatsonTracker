using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Helper;
using WatsonTracker.Models;
using WatsonTracker.ViewModels;

namespace WatsonTracker.Controllers

{ public class DashboardController : Controller
{
    ApplicationDbContext db = new ApplicationDbContext();
    DashboardVM vm = new DashboardVM();
    UserRolesHelper helper = new UserRolesHelper();


    // GET: Dashboard
    public ActionResult Index()
    {
        vm.ActiveTickets = db.Tickets.Where(t => t.TicketStatus.Name != "Complete").ToList();
        vm.TicNotAssigned = db.Tickets.Where(t => t.AssignedToUserId == null).ToList();
        vm.NumProjects = db.Projects.Where(p => p.Id != null).ToList();
        vm.PriorityUrgent = db.Tickets.Where(t => t.TicketPriority.Name == "Urgent").ToList();
        vm.PriorityHigh = db.Tickets.Where(t => t.TicketPriority.Name == "High").ToList();
        vm.PriorityMedium = db.Tickets.Where(t => t.TicketPriority.Name == "Medium").ToList();
        vm.PriorityLow = db.Tickets.Where(t => t.TicketPriority.Name == "Low").ToList();
        vm.UsersAssigned = db.Users.Where(u => u.Roles.Count != 0).ToList();
           



        return View();
    }
} } 

