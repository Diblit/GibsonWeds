using GibsonWeds.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibsonWeds.DAL.Classes.Admin
{
    public class bl_Registry_Result
    {
        public long registryID { get; set; }
        public bool hasError { get; set; }
        public string ErrorText { get; set; }
    }
    public class bl_Registry
    {
        public long registryID { get; set; }
        public string GiftName { get; set; }
        public bool? isSelected { get; set; }
        public long? selectedUserID { get; set; }
        public string selectedUserName { get; set; }
        public static List<bl_Registry> RegistryList(ref PagingInfo paging)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_Registry
                        select new bl_Registry
                        {
                            registryID = row.registryID,
                            GiftName = row.GiftName,
                            isSelected = row.isSelected,
                            selectedUserID = row.selectedUserID,
                            selectedUserName = (from rowSUN in metadata.db_User
                                                where row.selectedUserID == rowSUN.userID
                                                select rowSUN.FirstName + " " + rowSUN.LastName).FirstOrDefault(),
                        };

                //Search filter
                string search = paging.SearchString;
                if (!String.IsNullOrWhiteSpace(search)) //if there is a searchterm
                {
                    q = q.Where(r => r.GiftName.Contains(search) || r.selectedUserName.Contains(search));
                }
                string OrderByCol = paging.sort_col;
                bool OrderDirectionAscending = paging.sort_isAsc;

                //Sorting

                if (!String.IsNullOrWhiteSpace(OrderByCol)) //if there is a column to sort by
                {
                    //do sorting
                    switch (OrderByCol)
                    {
                        case ("GiftName"):
                            {
                                if (OrderDirectionAscending)
                                {

                                    q = q.OrderBy(r => r.GiftName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.GiftName);

                                }
                            }
                            break;
                        case ("selectedUserName"):
                            {
                                if (OrderDirectionAscending)
                                {

                                    q = q.OrderBy(r => r.selectedUserName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.selectedUserName);

                                }
                            }
                            break;
                    }
                }
                else
                {
                    q = q.OrderBy(r => r.registryID);
                }
                //futurecount and futere() after take
                var fCount = q.Count();
                var qF = q.Skip(paging.skip).Take(paging.take);

                var result = qF.ToList();
                paging.result_count = fCount;
                return result;
            }
        }
        public static bl_Registry_Result Add(bl_Registry info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var qDuplicate = (from row in metadata.db_Registry
                                  where row.GiftName.ToLower().Trim() == info.GiftName.ToLower().Trim()
                                  select row).FirstOrDefault();

                if (qDuplicate == null)
                {
                    var newRegistry = new db_Registry
                    {
                        GiftName = info.GiftName,
                        isSelected = false,
                    };

                    metadata.db_Registry.Add(newRegistry);
                    metadata.SaveChanges();

                    var result = new bl_Registry_Result
                    {
                        hasError = false,
                    };
                    return result;
                }
                else
                {
                    var result = new bl_Registry_Result
                    {
                        hasError = true,
                        ErrorText = "Gift already exist"
                    };
                    return result;
                }

            }
        }
        public static bl_Registry_Result Edit(bl_Registry info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qReg = (from row in metadata.db_Registry
                            where row.registryID == info.registryID
                            select row).FirstOrDefault();

                //Check if their is a duplicate 
                var qDuplicate = (from row in metadata.db_Registry
                                  where row.GiftName.ToLower().Trim() == info.GiftName.ToLower().Trim()
                                  && row.registryID != info.registryID
                                  select row).FirstOrDefault();


                var item = qReg;
                if (item == null) throw new NullReferenceException("No Gift found. Refresh Page");

                var duplicate = qDuplicate;
                if (duplicate == null && item.isSelected == false)
                {
                    item.GiftName = info.GiftName;

                    metadata.SaveChanges();

                    var result = new bl_Registry_Result
                    {
                        hasError = false
                    };
                    return result;
                }
                else if (duplicate == null && item.isSelected == true)
                {
                    var result = new bl_Registry_Result
                    {
                        hasError = true,
                        ErrorText = "Cannot edit a gift already chosen by a Guest"
                    };
                    return result;
                }
                else
                {
                    var result = new bl_Registry_Result
                    {
                        hasError = true,
                        ErrorText = "Gift already exist"
                    };
                    return result;
                }
            }
        }
        public static void Delete(long registryID)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var item = metadata.db_Registry.Find(registryID);
                if (item == null) throw new NullReferenceException("No Registry Gift found to Delete");

                metadata.db_Registry.Remove(item);
                metadata.SaveChanges();
            }
        }
        public static List<bl_Registry> GetRegistry(long userID)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_Registry
                        where row.isSelected == false
                        select new bl_Registry
                        {
                            registryID = row.registryID,
                            GiftName = row.GiftName,
                            isSelected = row.isSelected,
                            selectedUserID = row.selectedUserID,
                        };

                var qS = from row in metadata.db_Registry
                         where row.selectedUserID == userID
                         select new bl_Registry
                         {
                             registryID = row.registryID,
                             GiftName = row.GiftName,
                             isSelected = row.isSelected,
                             selectedUserID = row.selectedUserID,
                         };

                var qList = q.ToList();
                var qSList = qS.ToList();

                List<bl_Registry> reg = new List<bl_Registry>();
                foreach (var item in qList)
                {
                    reg.Add(item);
                }
                foreach (var item in qSList)
                {
                    reg.Add(item);
                }

                var result = reg;

                return result;
            }
        }
        public static bl_Registry_Result UpdateSelect(bl_Registry info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qReg = (from row in metadata.db_Registry
                            where row.registryID == info.registryID
                            select row).FirstOrDefault();

                var item = qReg;
                if (item == null) throw new NullReferenceException("No Gift found. Refresh Page");

                item.isSelected = info.isSelected;
                item.selectedUserID = info.selectedUserID;

                metadata.SaveChanges();

                var result = new bl_Registry_Result
                {
                    hasError = false
                };
                return result;
            }
        }
    }
}
