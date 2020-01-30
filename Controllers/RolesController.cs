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
        public ActionResult AssignUserRole(string UserList, string RoleList)
        {
            if (helper.AddUserToRole(UserList, RoleList))
            {
                 return RedirectToAction("Index", "Home");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
    }
}