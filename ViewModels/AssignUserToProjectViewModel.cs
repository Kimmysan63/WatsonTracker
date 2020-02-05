using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WatsonTracker.Models
{
    public class AssignUserToProjectViewModel
    {
        public SelectList UserId { get; set; }
        public SelectList ProjectId { get; set; }
        public ICollection<ApplicationUser> ProjectUser { get; set; }

    }
}