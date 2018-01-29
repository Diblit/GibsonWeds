using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GibsonWeds.Models
{
    public class RegistryInfo
    {
        public long registryID { get; set; }
        public string GiftName { get; set; }
        public bool isSelected { get; set; }
        public long ? selectedUserID { get; set; }
    }
}