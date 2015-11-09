using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDemotivator.Models
{
    public class HomeViewModel
    {
        public ICollection<Demotivator> demotivators { get; set; }
        public ICollection<tag> tags { get; set; }
        public int DemCount { get; set; }
        public ICollection<Demotivator> TopDateDemotivators { get; set; }
        public ICollection<Demotivator> TopRateDemotivators { get; set; }
        public ICollection<Demotivator> TopDiscusDemotivators { get; set; }
    }
}