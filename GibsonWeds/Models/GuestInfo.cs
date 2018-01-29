using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GibsonWeds.Models
{
    public class GuestInfo
    {
        public long userID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Cell { get; set; }
        public bool allowPlusOne { get; set; }
        public bool isPlusOne { get; set; }
        public long ? groupCoupleID { get; set; }
        public bool isGuest { get; set; }
        public bool isAdmin { get; set; }
        public bool hasRSVPd { get; set; }
        public bool isAttending { get; set; }
    }
}