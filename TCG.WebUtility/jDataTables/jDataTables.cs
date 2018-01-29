using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace TCG.WebUtility
{
    public class jDataTables_Sort
    {
        public string col;
        public bool isAsc;
    }

    public class jDataTables
    {
        public static int SkipRows()
        {
            string s = HttpContext.Current.Request.QueryString["iDisplayStart"];
            return int.Parse(s);
        }

        public static int TakeRows()
        {
            string s = HttpContext.Current.Request.QueryString["iDisplayLength"];
            return int.Parse(s);
        }

        public static string SearchString()
        {
            return HttpContext.Current.Request.QueryString["sSearch"];
        }

        public static jDataTables_Sort SortCols(int sortInx = 0)
        {
            int numColsSort = 0;
            if (!int.TryParse(HttpContext.Current.Request.QueryString["iSortingCols"], out numColsSort))
                return null;

            if (sortInx + 1 > numColsSort)
                return null;

            var colIxd = HttpContext.Current.Request.QueryString["iSortCol_" + sortInx];
            var colSortDir = HttpContext.Current.Request.QueryString["sSortDir_" + sortInx];
            var mdataprop = HttpContext.Current.Request.QueryString["mDataProp_" + colIxd];

            var result = new jDataTables_Sort();
            if (colSortDir == "asc")
                result.isAsc = true;
            else if (colSortDir == "desc")
                result.isAsc = false;
            else
                return null;

            if (!string.IsNullOrEmpty(mdataprop))
                result.col = mdataprop;
            else
                return null;

            return result;
        }

        public static string jsonString(object o, int totalRowCount)
        {
            string sEchoS = HttpContext.Current.Request.QueryString["sEcho"];
            int sEcho = -1;

            if (!int.TryParse(sEchoS, out sEcho))
                sEcho = -1;

            var jsonO = new
            {
                sEcho = sEcho,
                iTotalRecords = totalRowCount,
                iTotalDisplayRecords = totalRowCount,
                aaData = o
            };
            var s = new JavaScriptSerializer().Serialize(jsonO);

            return s;
        }

        public static object jsonObject(object o, int totalRowCount)
        {
            string sEchoS = HttpContext.Current.Request.QueryString["sEcho"];
            int sEcho = -1;

            if (!int.TryParse(sEchoS, out sEcho))
                sEcho = -1;

            var jsonO = new
            {
                sEcho = sEcho,
                iTotalRecords = totalRowCount,
                iTotalDisplayRecords = totalRowCount,
                aaData = o
            };

            return jsonO;
        }

        public static string jsonString(object o, int totalRowCount, object extraVars)
        {
            string sEchoS = HttpContext.Current.Request.QueryString["sEcho"];
            int sEcho = -1;

            if (!int.TryParse(sEchoS, out sEcho))
                sEcho = -1;

            var jsonO = new
            {
                sEcho = sEcho,
                iTotalRecords = totalRowCount,
                iTotalDisplayRecords = totalRowCount,
                aaData = o,
                Extra = extraVars
            };

            var s = new JavaScriptSerializer().Serialize(jsonO);

            return s;
        }

        public static object jsonObject(object o, int totalRowCount, object extraVars)
        {
            string sEchoS = HttpContext.Current.Request.QueryString["sEcho"];
            int sEcho = -1;

            if (!int.TryParse(sEchoS, out sEcho))
                sEcho = -1;

            var jsonO = new
            {
                sEcho = sEcho,
                iTotalRecords = totalRowCount,
                iTotalDisplayRecords = totalRowCount,
                aaData = o,
                Extra = extraVars
            };

            return jsonO;
        }
    }
}
