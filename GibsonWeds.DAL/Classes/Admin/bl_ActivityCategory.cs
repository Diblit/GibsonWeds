using GibsonWeds.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibsonWeds.DAL.Classes.Admin
{
    public class bl_ActivityCategory_Result
    {
        public long activityCategoryID { get; set; }
        public bool hasError { get; set; }
        public string ErrorText { get; set; }
    }
    public class bl_ActivityCategory
    {
        public long activityCategoryID { get; set; }
        public string Name { get; set; }
        public decimal ? Price { get; set; }
        public static List<bl_ActivityCategory> ActivityCategoryList(ref PagingInfo paging)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_ActivityCategory                        
                        select new bl_ActivityCategory
                        {
                            activityCategoryID = row.activityCategoryID,
                            Name = row.Name,
                            Price = row.Price
                        };

                //Search filter
                string search = paging.SearchString;
                if (!String.IsNullOrWhiteSpace(search)) //if there is a searchterm
                {
                    q = q.Where(r => r.Name.Contains(search));
                }
                string OrderByCol = paging.sort_col;
                bool OrderDirectionAscending = paging.sort_isAsc;

                //Sorting

                if (!String.IsNullOrWhiteSpace(OrderByCol)) //if there is a column to sort by
                {
                    //do sorting
                    switch (OrderByCol)
                    {
                        case ("FirstName"):
                            {
                                if (OrderDirectionAscending)
                                {

                                    q = q.OrderBy(r => r.Name);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.Name);

                                }
                            }
                            break;                        
                    }
                }
                else
                {
                    q = q.OrderBy(r => r.activityCategoryID);
                }
                //futurecount and futere() after take
                var fCount = q.Count();
                var qF = q.Skip(paging.skip).Take(paging.take);

                var result = qF.ToList();
                paging.result_count = fCount;
                return result;
            }
        }
        public static bl_ActivityCategory_Result Add(bl_ActivityCategory info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var qDuplicate = (from row in metadata.db_ActivityCategory
                                  where row.Name.ToLower().Trim() == info.Name.ToLower().Trim()
                                  select row).FirstOrDefault();

                if (qDuplicate == null)
                {
                    var newActivityCategory = new db_ActivityCategory
                    {
                        Name = info.Name,
                        Price = info.Price
                    };

                    metadata.db_ActivityCategory.Add(newActivityCategory);
                    metadata.SaveChanges();

                    var result = new bl_ActivityCategory_Result
                    {
                        hasError = false,
                    };
                    return result;
                }
                else
                {
                    var result = new bl_ActivityCategory_Result
                    {
                        hasError = true,
                        ErrorText = "Name already exist for another Activity"
                    };
                    return result;
                }

            }
        }
        public static bl_ActivityCategory_Result Edit(bl_ActivityCategory info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qActCat = (from row in metadata.db_ActivityCategory
                              where row.activityCategoryID == info.activityCategoryID
                              select row).FirstOrDefault();

                //Check if their is a duplicate 
                var qDuplicate = (from row in metadata.db_ActivityCategory
                                  where row.Name.ToLower().Trim() == info.Name.ToLower().Trim()
                                  && row.activityCategoryID != info.activityCategoryID
                                  select row).FirstOrDefault();


                var item = qActCat;
                if (item == null) throw new NullReferenceException("No Activity Category found. Refresh Page");

                var duplicate = qDuplicate;
                if (duplicate == null)
                {
                    item.Name = info.Name;
                    item.Price = info.Price;
                    

                    metadata.SaveChanges();

                    var result = new bl_ActivityCategory_Result
                    {
                        hasError = false
                    };
                    return result;
                }
                else
                {
                    var result = new bl_ActivityCategory_Result
                    {
                        hasError = true,
                        ErrorText = "Name already exist for another Activity Category"
                    };
                    return result;
                }
            }
        }
        public static void Delete(long activityCategoryID)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var item = metadata.db_ActivityCategory.Find(activityCategoryID);
                if (item == null) throw new NullReferenceException("No Activity Category found to Delete");

                metadata.db_ActivityCategory.Remove(item);
                metadata.SaveChanges();                               
            }
        }
        public static List<bl_ActivityCategory> GetActivities()
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_ActivityCategory
                        select new bl_ActivityCategory
                        {
                            activityCategoryID = row.activityCategoryID,
                            Name = row.Name,
                            Price = row.Price,
                        };

                var result = q.ToList();

                return result;
            }
        }
    }
}
