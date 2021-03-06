﻿using Microsoft.AspNet.Identity;
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
using WatsonTracker.ViewModels;

namespace WatsonTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper helper = new ProjectsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();



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
        //POST: Assign Users to Projects from Dashboard
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserToProjectDB(string UserId, int ProjectId)
        {
            helper.AddUserToProject(UserId, ProjectId);
            db.SaveChanges();
            return RedirectToAction("UserDashboard", "Dashboard");
        }

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

        // GET: Assign ProjectManager to Projects
        [Authorize(Roles = "Admin,ProjectManager")]

        public ActionResult AssignProjectManagerToProject()
        {
            AssignProjectManagerToProject model = new AssignProjectManagerToProject();
            List<ProjectWithPMName> listOfProj = new List<ProjectWithPMName>();

            model.ProjectManagerId = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "FirstName");
            model.ProjectId = new SelectList(db.Projects, "Id", "Name");
            List<Project> normProjects = db.Projects.ToList();
            foreach(var proj in normProjects)
            {
                ProjectWithPMName projectWith = new ProjectWithPMName();

                projectWith.Name = proj.Name;
                var PMId = proj.ProjectManagerId;
                if (PMId == null)
                {
                    projectWith.PMName = "";
                } else
                {
                    projectWith.PMName = db.Users.FirstOrDefault(u => u.Id == PMId).FirstName;
                }

                listOfProj.Add(projectWith);
            }

            model.Projects = listOfProj;
            return View(model);

        }
        //POST: Assign ProjectManager to Projects
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignProjectManagerToProject(string ProjectManagerId, int ProjectId)
        {
            helper.AddProjectManagerToProject(ProjectManagerId, ProjectId);
            db.SaveChanges();
            return RedirectToAction("AssignProjectManagerToProject", "Projects");
        }

        //POST: Assign ProjectManager to Projects from Dashboard
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignProjectManagerToProjectDB(string ProjectManagerId, int ProjectId)
        {
            helper.AddProjectManagerToProject(ProjectManagerId, ProjectId);
            db.SaveChanges();
            return RedirectToAction("AssignProjectManagerToProject", "Projects");
        }


        // GET: Projects
        [Authorize(Roles ="Admin, ProjectManger, Developer, Submitter")]
        public ActionResult Index(List<Project> model)
        {
            if (User.IsInRole("Admin"))
            {
                model = db.Projects.Where(p => p.IsDeleted == false).ToList();

                return View(model);
            }
            else if (User.IsInRole("ProjectManager"))
            {
                var userId = User.Identity.GetUserId();

                model = db.Projects.Where(p => p.ProjectManagerId == userId).ToList();

                return View(model);
            }
            else if (User.IsInRole("Developer") ||  User.IsInRole("Submitter"))
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
            UserRolesHelper rolesHelper = new UserRolesHelper();
            ViewBag.ProjectManagerId = new SelectList(rolesHelper.UsersInRole("ProjectManager"), "Id", "FirstName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ProjectManagerId")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.PMName = db.Users.FirstOrDefault(u=>u.Id == project.ProjectManagerId).FirstName;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult CreateDB()
        {
            UserRolesHelper rolesHelper = new UserRolesHelper();
            ViewBag.ProjectManagerId = new SelectList(rolesHelper.UsersInRole("ProjectManager"), "Id", "FirstName");
            return View();
        }

        // POST: Projects/Create in User Dashboard model
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDB([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                //project.PMName = db.Users.FirstOrDefault(u => u.Id == project.ProjectManagerId).FirstName;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("UserDashboard", "Dashboard");
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
            //remove db.Projects.Remove(project);   ***changed to soft delete below
            project.IsDeleted = true;
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

