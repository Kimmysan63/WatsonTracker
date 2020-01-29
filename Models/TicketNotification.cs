﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatsonTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool HasBeenRead { get; set; }

    }
}