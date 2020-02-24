using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Helper;
using WatsonTracker.Models;

namespace WatsonTracker.Controllers

{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper helper = new UserRolesHelper();

        // GET: Roles
        public ActionResult AssignUserRole()
        {
            AssignNewUserRoleViewModel model = new AssignNewUserRoleViewModel();

            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.RoleName = new SelectList(db.Roles, "Name", "Name");
            model.UserRole = db.Users.ToList();

            return View(model);
        }

        // POST: Roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserRole(string UserId, string RoleName)
        {
            if (helper.AddUserToRole(UserId, RoleName))
            {
                return RedirectToAction("AssignUserRole", "Roles");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        // POST: Roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserRoleDB(string UserId, string RoleName)
        {
            if (helper.AddUserToRole(UserId, RoleName))
            {
                return RedirectToAction("UserDashboard", "Dashboard");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }



        // GET: Remove User Roles
        public ActionResult RemoveUserRole()
        {
            RemoveUserRoleViewModel model = new RemoveUserRoleViewModel();

            model.UserId = new SelectList(db.Users, "Id", "FirstName");
            model.RoleName = new SelectList(db.Roles, "Name", "Name");
            model.UserRole = db.Users.ToList();

            return View(model);
        }

        // POST: Remove User Roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUserRole(string UserId, string RoleName)
        {
            if (helper.RemoveUserFromRole(UserId, RoleName))
            {
                return RedirectToAction("RemoveUserRole", "Roles");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        // GET: Roles
        public ActionResult Index()
        {
            ICollection<ApplicationUser> userList = db.Users.ToList();

            return View(userList);
        }

    }

}
