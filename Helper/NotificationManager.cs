using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatsonTracker.Models;

namespace WatsonTracker.Helper
{
    public class NotificationManager
    {
        public static void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            //Determine if DeveloperId has changed.  Either assigned, unassigned, reassigned.
            ManageGeneralAssignmentNotification(oldTicket, newTicket);
            ManagePropertyChangeNotifications(oldTicket, newTicket);
            //ManageAttachmentNotifications(ticketAttachment);

        }
        private static void ManageGeneralAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            //This is where decide what type of notification needed
            var assigned = oldTicket.AssignedToUserId == null && newTicket.AssignedToUserId != null;
            var unassigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId == null;
            var reassigned = newTicket.AssignedToUserId != null && newTicket.AssignedToUserId != oldTicket.AssignedToUserId;

            var newNotification = new TicketNotification();
            newNotification.TicketId = newTicket.Id;

            if (assigned)
            {
                newNotification.RecipientId = newTicket.AssignedToUserId;
                newNotification.NotificationBody = $"You have been assigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
            }
            else if (unassigned)
            {
                newNotification.RecipientId = newTicket.AssignedToUserId;
                newNotification.NotificationBody = $"You have been unassigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
            }
            else if (reassigned)
            {
                newNotification.RecipientId = newTicket.AssignedToUserId;
                newNotification.NotificationBody = $"You have been unassigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);

                newNotification.RecipientId = newTicket.AssignedToUserId;
                newNotification.NotificationBody = $"You have been reassigned to Ticket Id {newTicket.Id}";
                GenerateNotification(newNotification);
            }
        }
        private static void ManagePropertyChangeNotifications(Ticket oldTicket, Ticket newTicket)
        {
            //if (newTicket.AssignedToUserId != null)
            //Manually compare Ticket properties

            if (oldTicket.Title != newTicket.Title)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Title has changed for Ticket Id {newTicket.Id} from {oldTicket.Title} to { newTicket.Title }"
                });
            }
            if (oldTicket.Description != newTicket.Description)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Description has changed for Ticket Id {newTicket.Id} from {oldTicket.Description} to { newTicket.Description }"
                });
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Ticket Type has changed for Ticket Id {newTicket.Id} from {oldTicket.TicketType.Name} to { newTicket.TicketType.Name }"
                });
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Ticket Status has changed for Ticket Id {newTicket.Id} from {oldTicket.TicketStatus.Name} to { newTicket.TicketStatus.Name }"
                });
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                GenerateNotification(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    RecipientId = newTicket.AssignedToUserId,
                    NotificationBody = $"The Ticket Priority has changed for Ticket Id {newTicket.Id} from {oldTicket.TicketPriority.Name} to { newTicket.TicketPriority.Name }"
                });
            }
        }


        public static void ManageAttachmentNotifications(TicketAttachment ticketAttachment)
        {
            GenerateNotification(new TicketNotification
            {
                TicketId = ticketAttachment.TicketId,
                RecipientId = ticketAttachment.UserId,
                NotificationBody = $"A new attachment has been added for Ticket Id {ticketAttachment.TicketId}"
            });
        }
        public static void ManageCommentNotifications(TicketComment ticketComment)
        {
            GenerateNotification(new TicketNotification
            {
                TicketId = ticketComment.TicketId,
                RecipientId = ticketComment.UserId,
                NotificationBody = $"A new comment has been added for Ticket Id {ticketComment.TicketId}.  "
            });
        }

        private static void GenerateNotification(TicketNotification notification)
        {
            var db = new ApplicationDbContext();
            //newNotification.SenderId = HttpContext.Current.User.Identity.GetUserId(),
            //var newNotification = new TicketNotification
            var newNotification = new TicketNotification
            {
                Created = DateTime.Now,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = notification.RecipientId,
                NotificationBody = notification.NotificationBody,
                HasBeenRead = false,
                TicketId = notification.TicketId
            };
            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }

    }
}