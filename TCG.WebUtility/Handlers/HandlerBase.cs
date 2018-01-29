using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace TCG.WebUtility
{
    public abstract partial class HandlerBase : IHttpHandler, IRequiresSessionState
    {
        private HttpContext _context = null;
        public HttpContext context
        {
            get { return _context; }
            private set { _context = value; }
        }

        private string returnType = "application/json";
        private bool doTransform = true;
        private bool cached = false;
        private bool gzip = true;

        public void ProcessRequest(HttpContext context)
        {
            this.context = context;

            // Get all Query String Values

            // Get method to invoke
            var method = context.Request.QueryString["method"];


            // check if in different namespace


            // Get Method Info
            MethodInfo m = null;
            if (string.IsNullOrEmpty(method))
                throw new Exception("Method Not Found!");
            m = this.GetType().GetMethod(method);
            if (m == null)
                throw new Exception("Method Not Found!");

            // Method Variables
            List<object> a = new List<object>();
            foreach (var param in m.GetParameters())
            {
                Type propertyType = param.ParameterType;
                var qs = context.Request.QueryString[param.Name];

                // string, int, long, double, decimal
                // 
                if (string.IsNullOrEmpty(qs))
                {
                    if (propertyType.Equals(typeof(String))
                        || propertyType.Equals(typeof(Int16?))
                        || propertyType.Equals(typeof(Int32?))
                        || propertyType.Equals(typeof(Int64?))
                        || propertyType.Equals(typeof(long?))
                        || propertyType.Equals(typeof(Decimal?))
                        || propertyType.Equals(typeof(Double?))
                        || propertyType.Equals(typeof(bool?)))
                    {
                        a.Add(null);
                    }
                    else
                    {
                        throw new Exception(string.Format("Null Method Property {0} could not be converted", param.Name));
                    }
                }
                else
                {
                    if (propertyType.Equals(typeof(String)))
                    {
                        a.Add(qs);
                    }
                    else if (propertyType.Equals(typeof(Int16))
                        || propertyType.Equals(typeof(Int32))
                        || propertyType.Equals(typeof(Int64))
                        || propertyType.Equals(typeof(long))
                        || propertyType.Equals(typeof(Decimal))
                        || propertyType.Equals(typeof(Double))
                        || propertyType.Equals(typeof(bool))

                        || propertyType.Equals(typeof(Int16?))
                        || propertyType.Equals(typeof(Int32?))
                        || propertyType.Equals(typeof(Int64?))
                        || propertyType.Equals(typeof(long?))
                        || propertyType.Equals(typeof(Decimal?))
                        || propertyType.Equals(typeof(Double?))
                        || propertyType.Equals(typeof(bool?)))
                    {
                        TypeConverter conv = TypeDescriptor.GetConverter(propertyType);
                        a.Add(conv.ConvertFromInvariantString(qs));
                    }
                    else
                    {
                        throw new Exception(string.Format("Method Property {0} could not be converted", param.Name));
                    }
                }
            }

            // invoke method
            object invokeResult = m.Invoke(this, a.ToArray());

            // set return type
            context.Response.ContentType = returnType;

            // check return type / transform if needed / write response
            if (invokeResult == null)
            {
                context.Response.Write(string.Empty);
            }
            else if (doTransform)
            {
                switch (context.Response.ContentType.ToLower())
                {
                    case "application/json":
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        string json = jsonSerializer.Serialize(invokeResult);
                        context.Response.Write(json);
                        break;
                    case "application/xml":
                        System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(invokeResult.GetType());
                        StringBuilder xmlSb = new StringBuilder();
                        System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(xmlSb);
                        xmlSerializer.Serialize(xmlWriter, invokeResult);
                        context.Response.Write(xmlSb.ToString());
                        break;
                    case "text/html":
                        context.Response.Write(invokeResult);
                        break;
                    default:
                        throw new Exception(string.Format("Unsupported content type [{0}]", context.Response.ContentType));
                }
            }
            else
            {
                context.Response.Write(invokeResult);
            }


            // check if No cache
            if (!cached)
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // check zip
            if (gzip)
            {
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// application/json (default)
        /// application/xml
        /// image/jpg
        /// </summary>
        /// <param name="value"></param>
        public void SetResponseContentType(string value)
        {
            returnType = value;
        }

        public void SetDoTransformation(bool yes)
        {
            doTransform = yes;
        }

        public void SetCacheResult(bool yes)
        {
            cached = yes;
        }

        public void SetCompression(bool yes)
        {
            gzip = yes;
        }
    }
}
