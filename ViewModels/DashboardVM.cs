using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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


    }
}