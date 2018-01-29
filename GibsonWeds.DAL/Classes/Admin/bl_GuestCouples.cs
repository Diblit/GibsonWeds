using GibsonWeds.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibsonWeds.DAL.Classes.Admin
{
    public class bl_GuestCouples_Result
    {
        public long groupCoupleID { get; set; }
        public bool hasError { get; set; }
        public string ErrorText { get; set; }
    }
    public class bl_GuestCouples
    {
        public long groupCoupleID { get; set; }
        public string CoupleName { get; set; }
        public static List<bl_GuestCouples> ListGuestCouples(ref PagingInfo paging)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_GroupCouple                        
                        select new bl_GuestCouples
                        {
                            groupCoupleID = row.groupCoupleID,
                            CoupleName = row.CoupleName,                            
                        };

                //Search filter
                string search = paging.SearchString;
                if (!String.IsNullOrWhiteSpace(search)) //if there is a searchterm
                {
                    q = q.Where(r => r.CoupleName.Contains(search));
                }
                string OrderByCol = paging.sort_col;
                bool OrderDirectionAscending = paging.sort_isAsc;

                //Sorting

                if (!String.IsNullOrWhiteSpace(OrderByCol)) //if there is a column to sort by
                {
                    //do sorting
                    switch (OrderByCol)
                    {
                        case ("CoupleName"):
                            {
                                if (OrderDirectionAscending)
                                {

                                    q = q.OrderBy(r => r.CoupleName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.CoupleName);

                                }
                            }
                            break;
                        
                    }
                }
                else
                {
                    q = q.OrderByDescending(r => r.groupCoupleID);
                }
                //futurecount and futere() after take
                var fCount = q.Count();
                var qF = q.Skip(paging.skip).Take(paging.take);

                var result = qF.ToList();
                paging.result_count = fCount;

                return result;
            }
        }
        public static bl_GuestCouples_Result Add(bl_GuestCouples info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var qDuplicate = (from row in metadata.db_GroupCouple
                                  where row.CoupleName.ToLower().Trim() == info.CoupleName.ToLower().Trim()
                                  select row).FirstOrDefault();

                if (qDuplicate == null)
                {
                    var newGroupCouple = new db_GroupCouple
                    {
                        CoupleName = info.CoupleName,
                        
                    };

                    metadata.db_GroupCouple.Add(newGroupCouple);
                    metadata.SaveChanges();

                    var result = new bl_GuestCouples_Result
                    {
                        hasError = false,
                    };
                    return result;
                }
                else
                {
                    var result = new bl_GuestCouples_Result
                    {
                        hasError = true,
                        ErrorText = "Couple Name already exist for another Couple"
                    };
                    return result;
                }

            }
        }
        public static bl_GuestCouples_Result Edit(bl_GuestCouples info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qGuest = (from row in metadata.db_GroupCouple
                              where row.groupCoupleID == info.groupCoupleID
                              select row).FirstOrDefault();

                //Check if their is a duplicate
                var qDuplicate = (from row in metadata.db_GroupCouple
                                  where row.CoupleName.ToLower().Trim() == info.CoupleName.ToLower().Trim()
                                  && row.groupCoupleID != info.groupCoupleID
                                  select row).FirstOrDefault();


                var item = qGuest;
                if (item == null) throw new NullReferenceException("No Guest Couple found");

                var duplicate = qDuplicate;
                if (duplicate == null)
                {
                    item.CoupleName = info.CoupleName;

                    metadata.SaveChanges();

                    var result = new bl_GuestCouples_Result
                    {
                        hasError = false
                    };
                    return result;
                }
                else
                {
                    var result = new bl_GuestCouples_Result
                    {
                        hasError = true,
                        ErrorText = "Couple Name already exist for another Couple"
                    };
                    return result;
                }
            }
        }
        public static void Delete(long groupCoupleID)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var item = metadata.db_GroupCouple.Find(groupCoupleID);
                if (item == null) throw new NullReferenceException("No Couple found to Delete");

                metadata.db_GroupCouple.Remove(item);
                metadata.SaveChanges();
            }
        }
    }
}
