using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatsonTracker.Models
{
    public class TicketStatus
    {
        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

    }

}