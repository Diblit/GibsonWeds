using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GibsonWeds.Models
{
    public class ActivityInfo
    {
        public long activityID { get; set; }
        public long activityCategoryID { get; set; }
        public long userID { get; set; }
    }
}