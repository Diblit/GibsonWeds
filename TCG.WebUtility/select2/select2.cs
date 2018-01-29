using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TCG.WebUtility.select2
{
    public class select2
    {
        public static string SearchString()
        {
            return HttpContext.Current.Request.QueryString["term"];
        }
        public static int SkipRows()
        {
            var page = HttpContext.Current.Request.QueryString["page"];

            int skip = 0;
            int page_i = -1;
            if (int.TryParse(page, out page_i))
                skip = (page_i * 10) - 10;
            return skip;
        }

        public static string selectOptions(List<KeyValuePair<long, string>> values, bool includeEmptyOption)
        {
            StringBuilder sb = new StringBuilder();

            if(includeEmptyOption)
                 sb.Append("<option></option>");

            foreach (var item in values)
                sb.Append(string.Format("<option value='{0}'>{1}</option>", item.Key, item.Value));

            return sb.ToString();
        }
    }
}
