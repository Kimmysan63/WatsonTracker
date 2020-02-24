using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Helper;
using WatsonTracker.Models;
using WatsonTracker.ViewModels;

namespace WatsonTracker.Controllers
{


    public class TicketsController : Controller
    {
        private HistoryHelper historyHelper = new HistoryHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();

        // GET: Tickets
        [Authorize(Roles = "Admin, ProjectManger, Developer, Submitter")]
        public ActionResult AllTicketIndex()
        {
            var tickets = db.Tickets.ToList();
            return View(tickets);
        }
        public ActionResult Index()
        {
            TicketIndexVM model = new TicketIndexVM();
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

            return View(model);
        }



        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            TicketDetailsVM ticketDetailsVM = new TicketDetailsVM();
            ticketDetailsVM.Histories = db.TicketHistories.Where(th => th.TicketId == id).ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectID = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "ID", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "ID", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "ID", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ProjectID,TicketTypeId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(p => p.Name == "New").ID;
                ticket.TicketPriorityId = db.TicketPriorities.FirstOrDefault(p => p.Name == "High").ID;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.ProjectID = new SelectList(db.Projects, "Id", "Name", ticket.ProjectID);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Create for Dashboard 
        [Authorize(Roles = "Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTicDB([Bind(Include = "Id,Title,Description,ProjectID,TicketTypeId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(p => p.Name == "New").ID;
                ticket.TicketPriorityId = db.TicketPriorities.FirstOrDefault(p => p.Name == "High").ID;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.ProjectID = new SelectList(db.Projects, "Id", "Name", ticket.ProjectID);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }



        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedToUserId = new SelectList(rolesHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.ProjectID = new SelectList(db.Projects, "Id", "Name", ticket.ProjectID);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "ID", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "ID", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "ID", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,ProjectID,TicketTypeId,TicketPriorityId,TicketStatusId,AssignedToUserId,OwnerUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //called Memento's object - picture of old before changes
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                ticket.Updated = DateTimeOffset.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                //determine if ticket notification needs to be generated.  This class/method is static.  No need to instantiate.
                NotificationManager.ManageTicketNotifications(oldTicket, newTicket);

                var userId = User.Identity.GetUserId();
                historyHelper.GenHistory(oldTicket, newTicket, userId);
                return RedirectToAction("Index");

            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.ProjectID = new SelectList(db.Projects, "Id", "Name", ticket.ProjectID);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "ID", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "ID", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "ID", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        //GET: Assign Ticket
        public ActionResult AssignTicket(int? id)
        {
            UserRolesHelper helper = new UserRolesHelper();
            var ticket = db.Tickets.Find(id);
            var users = helper.UsersInRole("Developer").ToList();
            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FirstName", ticket.AssignedToUserId);
            return View(ticket);
        }

        //POST: Assign Ticket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignTicket (Ticket model)
        {
            var ticket = db.Tickets.Find(model.Id);
            ticket.AssignedToUserId = model.AssignedToUserId;

            db.SaveChanges();

            var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);

            try
            {
                EmailService ems = new EmailService();
                IdentityMessage msg = new IdentityMessage();
                ApplicationUser user = db.Users.Find(model.AssignedToUserId);

                msg.Body = "You have been assigned a new Ticket." + Environment.NewLine +
                           "Please click the following link to view the details " +
                           "<a href=\"" + callbackUrl + "\">NEW TICKET</a>";

                msg.Destination = user.Email;
                msg.Subject = "New Ticket Assignment";

                await ems.SendMailAsync(msg);
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }

            return RedirectToAction("Index");
        }






        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
