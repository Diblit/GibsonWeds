using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO.Compression;

namespace TCG.WebUtility
{
    public class HandlerHelper
    {
        public static void setGzip()
        {
            var context = HttpContext.Current;
            // GZip
            string pageEncoding = context.Request.Headers["Accept-Encoding"];

            if (!string.IsNullOrEmpty(pageEncoding))
                if (pageEncoding.ToLower().Contains("gzip"))
                {
                    context.Response.AppendHeader("Content-encoding", "gzip");
                    context.Response.Filter = new
                    GZipStream(context.Response.Filter, CompressionMode.Compress);
                }
        }
    }
}
