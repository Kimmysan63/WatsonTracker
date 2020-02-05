using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Models;

namespace WatsonTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        [Authorize(Roles = "Admin, ProjectManger, Developer, Submitter")]
        //public ActionResult Index()
        //{
        //    var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
        //    return View(tickets.ToList());
        //}
        public ActionResult Index(List<Ticket> model)
        {
            if (User.IsInRole("Admin"))
            {
                model = db.Tickets.ToList();

                return View(model);
            }
            else if (User.IsInRole("ProjectManager"))
            {
                var userId = User.Identity.GetUserId();
                var projects = db.Projects.Where(p => p.ProjectManagerId == userId).ToList();

                model = projects.SelectMany(p => p.Tickets).ToList();

                return View(model);
            }
            else if (User.IsInRole("Developer"))
            {

                var userId = User.Identity.GetUserId();

                model = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();

                return View(model);
            }
            else if (User.IsInRole("Submitter"))
            {

                var userId = User.Identity.GetUserId();

                model = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();

                return View(model);
            }


            return View(db.Tickets.ToList());
        }



        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
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

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
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
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
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
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
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
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectID,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectID = new SelectList(db.Projects, "Id", "Name", ticket.ProjectID);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "ID", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "ID", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "ID", "Name", ticket.TicketTypeId);
            return View(ticket);
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
