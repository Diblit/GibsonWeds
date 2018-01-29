using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GibsonWeds.Utility
{
    public class Config
    {
        public static string sitemode
        {
            get { return ConfigurationManager.AppSettings["sitemode"]; }
        }

        //public static string baseurl
        //{
        //    get { return ConfigurationManager.AppSettings["baseurl"]; }
        //}

        //public static string secureurl
        //{
        //    get { return ConfigurationManager.AppSettings["secureurl"]; }
        //}
        public static string url_relative_path_base
        {
            get { return ConfigurationManager.AppSettings["url_relative_path_base"]; }

        }


        public static string imageUploadBaseURL
        {
            get { return ConfigurationManager.AppSettings["ImageUploads"]; }
        }

        public static bool BundleMinify
        {
            get
            {
                var val = ConfigurationManager.AppSettings["BundleMinify"];
                bool result;
                if (!bool.TryParse(val, out result))
                    return false;
                return result;
            }
        }
    }
}