using GibsonWeds.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TCG.WebUtility;
using GibsonWeds.DAL.Classes.Admin;
using GibsonWeds.Models;
using TCG.Crypto;

namespace GibsonWeds.Api
{
    public class AdminController : ApiController
    {
        //------------------------- Guest Start ----------------------------
        [HttpGet]
        [Route("api/admin/guest/list")]
        public object AdminGuestList(int iDisplayStart, int iDisplayLength, string sSearch)
       {
            PagingInfo paging = new PagingInfo
            {
                skip = iDisplayStart,
                take = iDisplayLength,
                SearchString = sSearch
            };

            var sort = jDataTables.SortCols();

            if (sort != null)
            {
                paging.sort_col = sort.col;
                paging.sort_isAsc = sort.isAsc;
            }

            var data = bl_GuestList.GuestList(ref paging);

            return jDataTables.jsonObject(data, paging.result_count);
        }

        [HttpPost]
        [Route("api/admin/guest/add")]
        public object GuestAdd(GuestInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                Info.PasswordHash = PasswordManager.encrypt(Info.Cell);

                var result = bl_GuestList.Add(new bl_GuestList
                {
                    FirstName = Info.FirstName,
                    LastName = Info.LastName,
                    Email = Info.Email,
                    Cell = Info.Cell,
                    PasswordHash = Info.PasswordHash,
                    allowPlusOne = Info.allowPlusOne,
                    isPlusOne = Info.isPlusOne,
                    groupCoupleID = Info.groupCoupleID,
                    isGuest = Info.isGuest,
                    isAdmin = Info.isAdmin,
                    hasRSVPd = Info.hasRSVPd,
                    isAttending = Info.isAttending                    
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }

            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/guest/edit")]
        public object GuestEdit(GuestInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_GuestList.Edit(new bl_GuestList
                {
                    userID = Info.userID,
                    FirstName = Info.FirstName,
                    LastName = Info.LastName,
                    Email = Info.Email,
                    Cell = Info.Cell,
                    allowPlusOne = Info.allowPlusOne,         
                    groupCoupleID = Info.groupCoupleID
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }
            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/guest/delete")]
        public object DeleteGuest(long userID)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                bl_GuestList.Delete(userID);
            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        //------------------------- Guest End ----------------------------

        //------------------------- Guest Couples Start ----------------------------

        [HttpGet]
        [Route("api/admin/guestcouples/list")]
        public object AdminGuestCouplesList(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            PagingInfo paging = new PagingInfo
            {
                skip = iDisplayStart,
                take = iDisplayLength,
                SearchString = sSearch
            };

            var sort = jDataTables.SortCols();

            if (sort != null)
            {
                paging.sort_col = sort.col;
                paging.sort_isAsc = sort.isAsc;
            }

            var data = bl_GuestCouples.ListGuestCouples(ref paging);

            return jDataTables.jsonObject(data, paging.result_count);
        }

        [HttpPost]
        [Route("api/admin/guestcouples/add")]
        public object GuestCoupleAdd(GuestCoupleInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();                

                var result = bl_GuestCouples.Add(new bl_GuestCouples
                {
                    CoupleName = Info.CoupleName
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }

            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/guestcouples/edit")]
        public object GuestCoupleEdit(GuestCoupleInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_GuestCouples.Edit(new bl_GuestCouples
                {
                    groupCoupleID = Info.groupCoupleID,
                    CoupleName = Info.CoupleName
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }
            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/guestcouples/delete")]
        public object GuestCoupleDelete(long guestCoupleID)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                bl_GuestCouples.Delete(guestCoupleID);
            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        //------------------------- Guest Couples End ----------------------------

        //------------------------- Activity Category Start ----------------------------

        [HttpGet]
        [Route("api/admin/activitycategory/list")]
        public object ActivityCategoryList(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            PagingInfo paging = new PagingInfo
            {
                skip = iDisplayStart,
                take = iDisplayLength,
                SearchString = sSearch
            };

            var sort = jDataTables.SortCols();

            if (sort != null)
            {
                paging.sort_col = sort.col;
                paging.sort_isAsc = sort.isAsc;
            }

            var data = bl_ActivityCategory.ActivityCategoryList(ref paging);

            return jDataTables.jsonObject(data, paging.result_count);
        }

        [HttpPost]
        [Route("api/admin/activitycategory/add")]
        public object ActivityCategoryAdd(ActivityCategoryInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_ActivityCategory.Add(new bl_ActivityCategory
                {
                    Name = Info.Name,
                    Price = Info.Price,
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }

            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/activitycategory/edit")]
        public object ActivityCategoryEdit(ActivityCategoryInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_ActivityCategory.Edit(new bl_ActivityCategory
                {
                    activityCategoryID = Info.activityCategoryID,
                    Name = Info.Name,
                    Price = Info.Price,
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }
            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/activitycategory/delete")]
        public object ActivityCategoryDelete(long activityCategoryID)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                bl_ActivityCategory.Delete(activityCategoryID);
            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        //------------------------- Activity Category End ----------------------------

        //------------------------- Activity Start ----------------------------

        [HttpGet]
        [Route("api/admin/activity/list")]
        public object ActivityList(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            PagingInfo paging = new PagingInfo
            {
                skip = iDisplayStart,
                take = iDisplayLength,
                SearchString = sSearch
            };

            var sort = jDataTables.SortCols();

            if (sort != null)
            {
                paging.sort_col = sort.col;
                paging.sort_isAsc = sort.isAsc;
            }

            var data = bl_ActivityList.ActivityList(ref paging);

            return jDataTables.jsonObject(data, paging.result_count);
        }

        //------------------------- Activity End ----------------------------

        //------------------------- Registry Start ----------------------------

        [HttpGet]
        [Route("api/admin/registry/list")]
        public object RegistryList(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            PagingInfo paging = new PagingInfo
            {
                skip = iDisplayStart,
                take = iDisplayLength,
                SearchString = sSearch
            };

            var sort = jDataTables.SortCols();

            if (sort != null)
            {
                paging.sort_col = sort.col;
                paging.sort_isAsc = sort.isAsc;
            }

            var data = bl_Registry.RegistryList(ref paging);

            return jDataTables.jsonObject(data, paging.result_count);
        }

        [HttpPost]
        [Route("api/admin/registry/add")]
        public object RegistryAdd(RegistryInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_Registry.Add(new bl_Registry
                {
                    GiftName = Info.GiftName,
                    isSelected = false,
                    selectedUserID = null
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }

            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/registry/edit")]
        public object RegistryEdit(RegistryInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_Registry.Edit(new bl_Registry
                {
                    registryID = Info.registryID,
                    GiftName = Info.GiftName,
                    
                });

                if (result.hasError)
                {
                    return new { isSuccess = false, errorText = result.ErrorText };
                }
                else
                {
                    return new { isSuccess = true };
                }
            }
            catch (NullReferenceException e)
            {
                return new { isSuccess = false, errorText = e.Message };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/admin/registry/delete")]
        public object RegistryDelete(long registryID)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                bl_Registry.Delete(registryID);
            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = "Cannot Delete gift already assigned to Guest" };
            }

            return new { isSuccess = true, errorText = "" };
        }

        //------------------------- Registry End ----------------------------
    }
}