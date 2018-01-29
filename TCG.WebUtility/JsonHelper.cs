using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TCG.WebUtility
{
    public class JsonHelper
    {
        public static string readJsonFromRequest()
        {
            string json;
            using (var sr = new System.IO.StreamReader(HttpContext.Current.Request.InputStream))
            {
                json = sr.ReadToEnd();
            }
            return json;
        }
    }
}
