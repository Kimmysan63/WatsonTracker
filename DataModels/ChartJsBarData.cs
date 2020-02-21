using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatsonTracker.DataModels
{
    public class OneChartJsBarData
    {
        public string ProjectName { get; set; }
        public int Urgent { get; set; }
        public int High { get; set; }
        public int Medium { get; set; }
        public int Low { get; set; }
    }
}