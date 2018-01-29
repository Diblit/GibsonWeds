using GibsonWeds.DAL.Classes.Admin;
using GibsonWeds.Models;
using GibsonWeds.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GibsonWeds.Api
{
    public class GuestApiController : ApiController
    {
        [HttpPost]
        [Route("api/guest/update")]
        public object GuestEdit(GuestInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_GuestList.UpdateGuest(new bl_GuestList
                {
                    userID = Info.userID,
                    hasRSVPd = Info.hasRSVPd,
                    isAttending = Info.isAttending,
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
                return new
                {
                    isSuccess = false,
                    errorText = e.Message
                };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/guest/resetpassword")]
        public object GuestResetPassword(ResetPasswordInfo Info)
        {
            var userID = this.loggedInUserID();
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var result = bl_GuestList.ResetPassword(userID, Info.OldPassword, Info.NewPassword);

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
                return new
                {
                    isSuccess = false,
                    errorText = e.Message
                };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/guest/registryselection")]
        public object RegistryEdit(RegistryInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var userID = this.loggedInUserID();

                var result = bl_Registry.UpdateSelect(new bl_Registry
                {
                    registryID = Info.registryID,
                    isSelected = Info.isSelected,
                    selectedUserID = userID,
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
                return new
                {
                    isSuccess = false,
                    errorText = e.Message
                };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }

        [HttpPost]
        [Route("api/guest/activitysategoryselect")]
        public object SelectActivityEdit(ActivityInfo Info)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new FormatException();

                var userID = this.loggedInUserID();

                var result = bl_ActivityList.AddActivity(new bl_ActivityList
                {
                    activityCategoryID = Info.activityCategoryID,
                    userID = userID,
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
                return new
                {
                    isSuccess = false,
                    errorText = e.Message
                };

            }

            catch (Exception ex)
            {
                return new { isSuccess = false, errorText = ex.Message };
            }

            return new { isSuccess = true, errorText = "" };
        }
    }
}