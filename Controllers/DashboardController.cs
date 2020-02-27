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
    public class DashboardController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        DashboardVM vm = new DashboardVM();
        UserRolesHelper helper = new UserRolesHelper();


        // GET: Dashboard 
        public ActionResult UserDashboard()
        {
            List<Project> projectsForUser = GetProjects();

            vm.ActiveTickets = db.Tickets.Where(t => t.TicketStatus.Name != "Complete").ToList();
            vm.TicNotAssigned = db.Tickets.Where(t => t.AssignedToUser == null).ToList();
            vm.NumProjects = projectsForUser;
            vm.PriorityUrgent = db.Tickets.Where(t => t.TicketPriority.Name == "Urgent").ToList();
            vm.PriorityHigh = db.Tickets.Where(t => t.TicketPriority.Name == "High").ToList();
            vm.PriorityMedium = db.Tickets.Where(t => t.TicketPriority.Name == "Medium").ToList();
            vm.PriorityLow = db.Tickets.Where(t => t.TicketPriority.Name == "Low").ToList();
            vm.UsersAssigned = db.Users.Where(u => u.Roles.Count != 0).ToList();
            vm.Tickets = db.Tickets.ToList();
            vm.UserId = new SelectList(db.Users, "Id", "FirstName");
            vm.RoleName = new SelectList(db.Roles, "Name", "Name");
            vm.ProjectId = new SelectList(db.Projects, "Id", "Name");
            vm.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            vm.MyTickets = GetTickets();

            ViewBag.ProjectManagerId = new SelectList(helper.UsersInRole("ProjectManager"), "Id", "FullName");

            return View(vm);
        }

        public List<Project> GetProjects()
        {
            List<Project> model = new List<Project>();
            var userId = User.Identity.GetUserId();
            if (User.IsInRole("Admin"))
            {
                model = db.Projects.Where(p => p.IsDeleted == false).ToList();

                return model;
            }
            else if (User.IsInRole("ProjectManager"))
            {

                model = db.Projects.Where(p => p.ProjectManagerId == userId).ToList();

                return model;
            }
            else if (User.IsInRole("Developer") || User.IsInRole("Submitter"))
            {

                var user = db.Users.Find(userId);

                model = user.Projects.ToList();

                return model;
            }

            return model;
        }

        public List<Ticket> GetTickets()
        {
            DashboardVM model = new DashboardVM();
            var tickets = db.Tickets;
            var myTickets = new List<Ticket>();
            var devTickets = new List<Ticket>();

            if (User.IsInRole("Admin"))
            {
                myTickets = tickets.ToList();
            }
            else if (User.IsInRole("ProjectManager"))
            {
                var userId = User.Identity.GetUserId();
                var projects = db.Projects.Where(p => p.ProjectManagerId == userId).ToList();

                myTickets = projects.SelectMany(p => p.Tickets).ToList();
            }
            else if (User.IsInRole("Developer"))
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var projects = user.Projects.ToList();
                myTickets = projects.SelectMany(t => t.Tickets).ToList();
                devTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
            }
            else if (User.IsInRole("Submitter"))
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                myTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
            }
            model.MyTickets = myTickets;
            model.AssignedTickets = devTickets;

            return myTickets;

        }
    }
}

