using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDemotivator.Models
{
    public class SearchViewModel
    {
        public ICollection<Demotivator> demotivators { get; set; }
        public ICollection<ApplicationUser> User { get; set; }
    }
}