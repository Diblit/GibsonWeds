using EntityFramework.Extensions;
using GibsonWeds.DAL.Classes.Shared;
using GibsonWeds.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Crypto;

namespace GibsonWeds.DAL.Classes.Admin
{
    public class bl_GuestList_Result
    {
        public long userID { get; set; }
        public bool hasError { get; set; }
        public string ErrorText { get; set; }
    }
    public class bl_GuestList
    {
        public long userID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Cell { get; set; }
        public bool? allowPlusOne { get; set; }
        public bool isPlusOne { get; set; }
        public long? groupCoupleID { get; set; }
        public bool isGuest { get; set; }
        public bool isAdmin { get; set; }
        public bool? hasRSVPd { get; set; }
        public bool isAttending { get; set; }

        //public List<bl_SelectBox> GuestCoupleList { get; set; }
        public static List<bl_GuestList> GuestList(ref PagingInfo paging)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_User
                        where row.isGuest == true
                        && row.isDeleted == false
                        select new bl_GuestList
                        {
                            userID = row.userID,
                            FirstName = row.FirstName,
                            LastName = row.LastName,
                            Email = row.Email,
                            Cell = row.Cell,
                            hasRSVPd = row.hasRSVPd,
                            allowPlusOne = row.allowPlusOne,
                            isAttending = row.isAttending,
                            groupCoupleID = row.groupCoupleID,
                        };

                //Search filter
                string search = paging.SearchString;
                if (!String.IsNullOrWhiteSpace(search)) //if there is a searchterm
                {
                    q = q.Where(r => r.FirstName.Contains(search) || r.LastName.Contains(search) || r.Email.Contains(search) || r.Cell.Contains(search));
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

                                    q = q.OrderBy(r => r.FirstName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.FirstName);

                                }
                            }
                            break;
                        case ("LastName"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.LastName);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.LastName);

                                }
                            }
                            break;
                        case ("Email"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.Email);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.Email);

                                }
                            }
                            break;
                        case ("Cell"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.Cell);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.Cell);

                                }
                            }
                            break;
                        case ("hasRSVPd"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.hasRSVPd);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.hasRSVPd);

                                }
                            }
                            break;
                        case ("isAttending"):
                            {
                                if (OrderDirectionAscending)
                                {
                                    q = q.OrderBy(r => r.isAttending);
                                }
                                else
                                {
                                    q = q.OrderByDescending(r => r.isAttending);

                                }
                            }
                            break;
                    }
                }
                else
                {
                    q = q.OrderBy(r => r.userID);
                }
                //futurecount and futere() after take
                var fCount = q.Count();
                var qF = q.Skip(paging.skip).Take(paging.take);

                var result = qF.ToList();
                paging.result_count = fCount;
                return result;
            }
        }
        public static bl_GuestList_Result Add(bl_GuestList info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var qDuplicate = (from row in metadata.db_User
                                  where row.Email.ToLower().Trim() == info.Email.ToLower().Trim()
                                  select row).FirstOrDefault();

                if (qDuplicate == null)
                {
                    var newGuest = new db_User
                    {
                        FirstName = info.FirstName,
                        LastName = info.LastName,
                        Email = info.Email,
                        Cell = info.Cell,
                        PasswordHash = info.PasswordHash,
                        allowPlusOne = info.allowPlusOne,
                        isPlusOne = info.isPlusOne,
                        groupCoupleID = info.groupCoupleID,
                        isGuest = info.isGuest,
                        isAdmin = info.isAdmin,
                        hasRSVPd = info.hasRSVPd,
                        isAttending = info.isAttending,
                    };

                    metadata.db_User.Add(newGuest);
                    metadata.SaveChanges();

                    var result = new bl_GuestList_Result
                    {
                        hasError = false,
                    };
                    return result;
                }
                else
                {
                    var result = new bl_GuestList_Result
                    {
                        hasError = true,
                        ErrorText = "Email already exist for another Guest"
                    };
                    return result;
                }

            }
        }
        public static bl_GuestList_Result Edit(bl_GuestList info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qGuest = (from row in metadata.db_User
                              where row.userID == info.userID
                              select row).FirstOrDefault();

                //Check if their is a duplicate 
                var qDuplicate = (from row in metadata.db_User
                                  where row.Email.ToLower().Trim() == info.Email.ToLower().Trim()
                                  && row.userID != info.userID
                                  select row).FirstOrDefault();


                var item = qGuest;
                if (item == null) throw new NullReferenceException("No Guest found");

                var duplicate = qDuplicate;
                if (duplicate == null)
                {
                    item.FirstName = info.FirstName;
                    item.LastName = info.LastName;
                    item.Email = info.Email;
                    item.Cell = info.Cell;
                    item.allowPlusOne = info.allowPlusOne;
                    item.groupCoupleID = info.groupCoupleID;

                    metadata.SaveChanges();

                    var result = new bl_GuestList_Result
                    {
                        hasError = false
                    };
                    return result;
                }
                else
                {
                    var result = new bl_GuestList_Result
                    {
                        hasError = true,
                        ErrorText = "Email already exist for another Guest"
                    };
                    return result;
                }
            }
        }
        public static void Delete(long userID)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var item = metadata.db_User.Find(userID);
                if (item == null) throw new NullReferenceException("No Guest found to Delete");

                try
                {
                    metadata.db_User.Remove(item);
                    metadata.SaveChanges();
                }
                catch
                {
                    item.isDeleted = true;
                    metadata.SaveChanges();
                }

            }
        }
        public static List<bl_GuestList> GetGuest(long userID)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = from row in metadata.db_User
                        where userID == row.userID
                        && row.isDeleted == false
                        && row.isGuest == true

                        select new bl_GuestList
                        {
                            userID = row.userID,
                            FirstName = row.FirstName,
                            LastName = row.LastName,
                            Email = row.Email,
                            Cell = row.Cell,
                            hasRSVPd = row.hasRSVPd,
                            allowPlusOne = row.allowPlusOne,
                            isAttending = row.isAttending,
                            groupCoupleID = row.groupCoupleID,
                        };
                var qUser = q.ToList();
                var qUserGroupCoupleID = qUser.Select(r => r.groupCoupleID).FirstOrDefault();

                if (qUserGroupCoupleID != null)
                {
                    var qC = from row in metadata.db_User
                             where row.groupCoupleID == qUserGroupCoupleID
                             select new bl_GuestList
                             {
                                 userID = row.userID,
                                 FirstName = row.FirstName,
                                 LastName = row.LastName,
                                 Email = row.Email,
                                 Cell = row.Cell,
                                 hasRSVPd = row.hasRSVPd,
                                 isAttending = row.isAttending,
                                 allowPlusOne = row.allowPlusOne,
                                 groupCoupleID = row.groupCoupleID,
                             };
                    var qCouple = qC.ToList();

                    return qCouple;
                }



                var result = q.ToList();
                return result;
            }
        }
        public static bool HasGuestRsvpD(long userID)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                var q = (from row in metadata.db_User
                         where userID == row.userID
                         select row.isAttending).FirstOrDefault();

                return q;
            }
        }
        public static bl_GuestList_Result UpdateGuest(bl_GuestList info)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qGuest = (from row in metadata.db_User
                              where row.userID == info.userID
                              select row).FirstOrDefault();

                var item = qGuest;
                if (item == null) throw new NullReferenceException("No Guest found");

                item.hasRSVPd = info.hasRSVPd;
                item.isAttending = info.isAttending;                

                metadata.SaveChanges();

                var result = new bl_GuestList_Result
                {
                    hasError = false
                };
                return result;
            }
        }

        public static bl_GuestList_Result ResetPassword(long userID, string oldPassword, string newPassword)
        {
            using (var metadata = DataAccess.getDesktopMetadata())
            {
                //Get original guest record
                var qGuest = (from row in metadata.db_User
                              where row.userID == userID
                              select row).FirstOrDefault();

                var item = qGuest;
                if (item == null) throw new NullReferenceException("No Guest found");

                //validate password
                if (!PasswordManager.verify(oldPassword, qGuest.PasswordHash))
                    return null;

                item.PasswordHash = PasswordManager.encrypt(newPassword);                

                metadata.SaveChanges();

                var result = new bl_GuestList_Result
                {
                    hasError = false
                };
                return result;
            }
        }
    }
}

