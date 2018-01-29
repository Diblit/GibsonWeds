using GibsonWeds.DAL.Classes.Admin;
using GibsonWeds.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TCG.WebUtility;

namespace GibsonWeds.API.Api
{
    public class AdminApiController : ApiController
    {
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
        
        
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}