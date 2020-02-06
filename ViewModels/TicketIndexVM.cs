using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Models;

namespace WatsonTracker.ViewModels
{
    public class TicketIndexVM
    {
        public ICollection<Ticket> MyTickets { get; set; }
        public ICollection<Ticket> AssignedTickets { get; set; }

    }
}