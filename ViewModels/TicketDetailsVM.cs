using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatsonTracker.Models;

namespace WatsonTracker.ViewModels
{
    public class TicketDetailsVM
    {
        public ICollection<TicketHistory> Histories { get; set; }
    }
}