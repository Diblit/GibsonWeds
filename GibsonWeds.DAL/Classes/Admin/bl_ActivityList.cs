using GibsonWeds.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibsonWeds.DAL.Classes.Admin
{
    public class bl_ActivityList_Result
    {
        public long activityCategoryID { get; set; }
        public bool hasError { get; set; }
        public string ErrorText { get; set; }
    }
    public class bl_ActivityList
    {
        public long activityID { get; set; }
        public long activityCategoryID { get; set; }
        public string ActivityName { get; set; }
        public long userID { get; set; }
        public string GuestName { get; set; }
        public static List<bl_ActivityList> ActivityList(ref PagingInfo paging)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_Activity
                        select new bl_ActivityList
                        {
                            activityID = row.activityID,
                            activityCategoryID = row.activityCategoryID,
                            userID = row.userID,
                            ActivityName = (from rowA in metadata.db_ActivityCategory
                                            where row.activityCategoryID == rowA.activityCategoryID
                                            select rowA.Name).FirstOrDefault(),
                            GuestName = (from rowG in metadata.db_User
                                         where row.userID == rowG.userID
                                         select rowG.FirstName + " " + rowG.LastName).FirstOrDefault()

                        };

                //Search filter
                string search = paging.SearchString;
                if (!String.IsNullOrWhiteSpace(search)) //if there is a searchterm
                {
                    q = q.Where(r => r.ActivityName.Contains(search) || r.GuestName.Contains(search));
                }
                string OrderByCol = paging.sort_col;
                bool OrderDirectionAscending = paging.sort_isAsc;

                //Sorting

                if (!String.IsNullOrWhiteSpace(OrderByCol)) //if there is a column to sort by
                {
                    //do sorting
                    switch (OrderByCol)
                    {
                        case ("ActivityName"):
                            {
                                if (OrderDirectionAscending)
                                {

                                    q = q.OrderBy(r => r.ActivityName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.ActivityName);

                                }
                            }
                            break;
                        case ("GuestName"):
                            {
                                if (OrderDirectionAscending)
                                {

                                    q = q.OrderBy(r => r.GuestName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.GuestName);

                                }
                            }
                            break;
                    }
                }
                else
                {
                    q = q.OrderBy(r => r.activityID);
                }
                //futurecount and futere() after take
                var fCount = q.Count();
                var qF = q.Skip(paging.skip).Take(paging.take);

                var result = qF.ToList();
                paging.result_count = fCount;
                return result;
            }
        }
        public static bl_ActivityList_Result AddActivity(bl_ActivityList info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {

                    var newActivity = new db_Activity
                    {
                        activityCategoryID = info.activityCategoryID,
                        userID = info.userID
                    };

                    metadata.db_Activity.Add(newActivity);
                    metadata.SaveChanges();

                    var result = new bl_ActivityList_Result
                    {
                        hasError = false,
                    };
                    return result;


            }
        }
        public static bl_ActivityList_Result Edit(bl_ActivityList info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qActCat = (from row in metadata.db_ActivityCategory
                               where row.activityCategoryID == info.activityCategoryID
                               select row).FirstOrDefault();

                var item = qActCat;
                if (item == null) throw new NullReferenceException("No Activity Category found. Refresh Page");

                //item.Name = info.Name;
                //item.Price = info.Price;


                metadata.SaveChanges();

                var result = new bl_ActivityList_Result
                {
                    hasError = false
                };
                return result;

            }
        }

    }
}
