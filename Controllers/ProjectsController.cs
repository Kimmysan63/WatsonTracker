using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Helper;
using WatsonTracker.Models;

namespace WatsonTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper helper = new ProjectsHelper();


        // GET: Assign Users to Projects
        [Authorize(Roles = "Admin,ProjectManager")]

        public ActionResult AssignUserToProject()
        {
            AssignUserToProjectViewModel model = new AssignUserToProjectViewModel();

            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.ProjectId = new SelectList(db.Projects, "Id", "Name");
            model.ProjectUser = db.Users.ToList();

            return View(model);

        }
        //POST: Assign Users to Projects
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserToProject(string UserId, int ProjectId)
        {
            helper.AddUserToProject(UserId, ProjectId);
            db.SaveChanges();
            return RedirectToAction("AssignUserToProject", "Projects");
        }
        //******************************************************************************************

        // GET: Remove Users to Projects
        [Authorize(Roles = "Admin,ProjectManager")]

        public ActionResult RemoveUserFromProject()
        {
            RemoveUserFromProjectViewModel model = new RemoveUserFromProjectViewModel();

            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.ProjectId = new SelectList(db.Projects, "Id", "Name");
            model.ProjectUser = db.Users.ToList();

            return View(model);

        }
        //POST: Remove Users to Projects
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUserFromProject(string UserId, int ProjectId)
        {
            helper.RemoveUserFromProject(UserId, ProjectId);
            db.SaveChanges();
            return RedirectToAction("RemoveUserFromProject", "Projects");
        }


        // GET: Projects
        [Authorize(Roles ="Admin, ProjectManger, Developer, Submitter")]
        public ActionResult Index(List<Project> model)
        {
            if (User.IsInRole("Admin"))
            {
                model = db.Projects.ToList();

                return View(model);
            }
            else if (User.IsInRole("ProjectManager"))
            {
                var userId = User.Identity.GetUserId();

                var user = db.Users.Find(userId);

                model = user.Projects.Where(p => p.ProjectManagerId == userId).ToList();

                return View(model);
            }
            else if ((User.IsInRole("Developer")) || ( User.IsInRole("Submitter")))
            {

                var userId = User.Identity.GetUserId();

                var user = db.Users.Find(userId);

                model = user.Projects.ToList();

                return View(model);
            }

            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin,ProjectManager")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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

