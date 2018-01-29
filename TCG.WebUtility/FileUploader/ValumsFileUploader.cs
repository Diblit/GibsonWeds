using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;

namespace TCG.WebUtility
{
    public class ValumsFileUploaderResult
    {
        public bool isSuccess;
        public string FileName;
        public byte[] bytes;
        public string ContentType;
    }

    public class ValumsFileUploader
    {
        public static ValumsFileUploaderResult process(ref HttpContext context)
        {
            // Get Filename
            String filename = HttpContext.Current.Request.Headers["X-File-Name"];

            // Check if anything is being uploaded
            if (string.IsNullOrEmpty(filename) && HttpContext.Current.Request.Files.Count <= 0)
                return new ValumsFileUploaderResult { isSuccess = false };

            if (filename == null)
            {
                //This work for IE
                try
                {
                    // get Posted File
                    HttpPostedFile uploadedfile = context.Request.Files[0];
                    
                    // Get FileName
                    filename = uploadedfile.FileName;

                    // Read bytes
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(uploadedfile.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(uploadedfile.ContentLength);
                    }

                    return new ValumsFileUploaderResult { isSuccess = true, FileName = filename, bytes = fileData, ContentType = uploadedfile.ContentType };
                }
                catch (Exception)
                {
                    return new ValumsFileUploaderResult { isSuccess = false };
                }
            }
            else
            {
                //This work for Firefox and Chrome.
                try
                {
                    // Read bytes
                    byte[] data;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        context.Request.InputStream.CopyTo(ms);
                        // If you need it...
                        data = ms.ToArray();
                    }

                    return new ValumsFileUploaderResult { isSuccess = true, FileName = filename, bytes = data };
                }
                catch (Exception)
                {
                    return new ValumsFileUploaderResult { isSuccess = false };
                }
            }
        }

        public static void writeResultToResponseStream(bool isSuccess, object data, ref HttpContext context)
        {
            if (isSuccess)
                context.Response.Write(new JavaScriptSerializer().Serialize(new { success = true, data = data }));
            else
            {
                if (data != null)
                    context.Response.Write(new JavaScriptSerializer().Serialize(new { success = false, data = data }));
                else
                    context.Response.Write(new JavaScriptSerializer().Serialize(new { success = false }));
            }
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }
}
