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
    
    public partial class db_GroupCouple
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public db_GroupCouple()
        {
            this.db_User = new HashSet<db_User>();
        }
    
        public long groupCoupleID { get; set; }
        public string CoupleName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<db_User> db_User { get; set; }
    }
}
