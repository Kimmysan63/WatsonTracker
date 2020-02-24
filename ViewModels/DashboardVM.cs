using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Models;

namespace WatsonTracker.ViewModels
{
    public class DashboardVM
    {
        public List<Ticket> ActiveTickets { get; set; }
        public List<Ticket> TicNotAssigned { get; set; }
        public List<Project> NumProjects { get; set; }
        public List<Ticket> PriorityUrgent { get; set; }
        public List<Ticket> PriorityHigh { get; set; }
        public List<Ticket> PriorityMedium { get; set; }
        public List<Ticket> PriorityLow { get; set; }
        public List<ApplicationUser> UsersAssigned { get; set; }
        public int TicByProject { get; set; }
        public List<Ticket>Tickets { get; set; }

        //assign user roles modal 

        public SelectList UserId { get; set; }
        public SelectList RoleName { get; set; }

        //create project modal 
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProjectManagerId { get; set; }
        public string PMName { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        //assign user to projects

        public SelectList ProjectId { get; set; }
        public ICollection<ApplicationUser> ProjectUser { get; set; }

        //create ticket
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public int TicketTypeId { get; set; }
        public virtual TicketType TicketType { get; set; }
    }
}