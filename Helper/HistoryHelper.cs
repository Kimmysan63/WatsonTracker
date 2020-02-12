using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatsonTracker.Models;

namespace WatsonTracker.Helper
{
    public class HistoryHelper
    {
        TicketHistory history = new TicketHistory();
        ApplicationDbContext db = new ApplicationDbContext();
        ApplicationUser user = new ApplicationUser();
        public void GenHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
            //var user = db.Users.Find(userId);

            if (oldTicket.Title != newTicket.Title)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Title";
                history.OldValue = oldTicket.Title;
                history.NewValue = newTicket.Title;
                history.Changed = DateTimeOffset.Now;
                history.UserId = user.DisplayName;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.Description != newTicket.Description)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Description";
                history.OldValue = oldTicket.Description;
                history.NewValue = newTicket.Description;
                history.Changed = DateTimeOffset.Now;
                history.UserId = user.DisplayName;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Dev Assigned";
                history.OldValue = oldTicket.AssignedToUserId;
                history.NewValue = newTicket.AssignedToUserId;
                history.Changed = DateTimeOffset.Now;
                history.UserId = user.DisplayName;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Priority";
                history.OldValue = oldTicket.TicketPriority.Name;
                history.NewValue = newTicket.TicketPriority.Name;
                history.Changed = DateTimeOffset.Now;
                history.UserId = user.DisplayName;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Status";
                history.OldValue = oldTicket.TicketStatus.Name;
                history.NewValue = newTicket.TicketStatus.Name;
                history.Changed = DateTimeOffset.Now;
                history.UserId = user.DisplayName;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                history.TicketId = newTicket.Id;
                history.Property = "Type";
                history.OldValue = oldTicket.TicketType.Name;
                history.NewValue = newTicket.TicketType.Name;
                history.Changed = DateTimeOffset.Now;
                history.UserId = user.DisplayName;
                db.TicketHistories.Add(history);
                db.SaveChanges();
            }
        }
    }
}

