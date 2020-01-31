using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WatsonTracker.Models
{
    public class RemoveUserRoleViewModel : Controller
    {
        // GET: RemoveUserRoleViewModel
        public SelectList UserId { get; set; }
        public SelectList RoleName { get; set; }
        public ICollection<ApplicationUser> UserRole { get; set; } 
    }

}
