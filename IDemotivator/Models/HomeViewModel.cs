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
        public string GetTagClass(int demotivators, int DemCount)
        {
            var result = (demotivators * 100) / DemCount;
            if (result <= 1)
                return "tag1";
            if (result <= 4)
                return "tag2";
            if (result <= 8)
                return "tag3";
            if (result <= 12)
                return "tag4";
            if (result <= 18)
                return "tag5";
            if (result <= 30)
                return "tag6";
            return result <= 50 ? "tag7" : "";
        }
    }

    public class MenuCategory
    {
        public int CountOfCategory { get; set; }
        public int TotalArticles { get; set; }
    }
}