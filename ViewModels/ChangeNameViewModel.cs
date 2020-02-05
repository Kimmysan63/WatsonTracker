using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatsonTracker.Models
{
    public class ChangeNameViewModel
    {
        [Required]
        [Display(Name = "Current First Name")]
        public string OldFirstName { get; set; }
        [Required]
        [Display(Name = "Current Last Name")]
        public string OldLastName { get; set; }
        [Required]
        [Display(Name = "New First Name")]
        public string NewFirstName { get; set; }
        [Required]
        [Display(Name = "New Last Name")]

        public string NewLastName { get; set; }

    }
}