using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatsonTracker.Models;

namespace WatsonTracker.ViewModels
{
    public class AssignProjectManagerToProject
    {
        public SelectList ProjectManagerId { get; set; }
        public SelectList ProjectId { get; set; }
        public List<ProjectWithPMName> Projects {get; set; }
    }

    public class ProjectWithPMName
    {
        public string PMName { get; set; }
        public string Name { get; set; }
    }
}