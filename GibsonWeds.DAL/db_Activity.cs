//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GibsonWeds.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class db_Activity
    {
        public long activityID { get; set; }
        public long activityCategoryID { get; set; }
        public long userID { get; set; }
    
        public virtual db_ActivityCategory db_ActivityCategory { get; set; }
        public virtual db_User db_User { get; set; }
    }
}