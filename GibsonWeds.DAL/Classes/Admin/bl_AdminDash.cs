using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibsonWeds.DAL.Classes.Admin
{
    public class bl_AdminDash
    {
        public int invitedGuests { get; set; }
        public int RSVPdYes { get; set; }
        public int ActivityInterest { get; set; }
        public static bl_AdminDash AdminDashList()
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var qU = from row in metadata.db_User
                         where row.isGuest == true
                         && row.isDeleted == false
                         select new
                         {
                             userID = row.userID,
                         };
                var qUser = qU.Count();

                var qRs = from row in metadata.db_User
                            where row.isGuest == true
                            && row.isDeleted == false
                            && row.isAttending == true
                            select new
                            {
                                userID = row.userID,
                            };
                var qRsvp = qRs.Count();

                var qAct = from row in metadata.db_Activity                            
                            select new
                            {
                                userID = row.userID,
                            };
                var qActivity = qAct.Count();

                var q = new bl_AdminDash
                {
                    invitedGuests = qUser,
                    RSVPdYes = qRsvp,
                    ActivityInterest = qActivity
                };

                return q;

            }
        }
    }
}
