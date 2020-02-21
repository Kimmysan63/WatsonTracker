using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.DataModels;
using WatsonTracker.Models;

namespace WatsonTracker.Controllers

{
    public class ChartsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Charts
        public JsonResult GetTicketPrioritiesBarChart()
        {
            List<OneChartJsBarData> charts = new List<OneChartJsBarData>();
            var projects = db.Projects.OrderByDescending(p => p.Tickets.Count()).Take(3).ToList();

            foreach(var project in projects)
            {
            var dataObject = new OneChartJsBarData();

            dataObject.ProjectName = project.Name;
            dataObject.Urgent = project.Tickets.Where(t => t.TicketPriority.Name == "Urgent").Count();
            dataObject.High = project.Tickets.Where(t => t.TicketPriority.Name == "High").Count();
            dataObject.Medium = project.Tickets.Where(t => t.TicketPriority.Name == "Medium").Count();
            dataObject.Low = project.Tickets.Where(t => t.TicketPriority.Name == "Low").Count();

                charts.Add(dataObject);
            }

            return Json(charts);
        }
    }
}