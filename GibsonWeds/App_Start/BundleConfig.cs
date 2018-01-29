using GibsonWeds.Utility;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace GibsonWeds
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        const string bundleXmlPath = "~/App_Data/bundles.xml";
        public static void RegisterBundles(BundleCollection bundles)
        {
            // register bundles
            BundleTable.Bundles.Clear();
            addXmlBundles(bundles);

            var xmlFilePath = HttpContext.Current.Server.MapPath(bundleXmlPath);

            FileSystemWatcher fWatcher = new FileSystemWatcher();

            fWatcher.Path = Path.GetDirectoryName(xmlFilePath);
            fWatcher.Filter = Path.GetFileName(xmlFilePath);
            fWatcher.EnableRaisingEvents = true;
            fWatcher.Changed += new FileSystemEventHandler(delegate (object sender, FileSystemEventArgs e)
            {
                // on change we force app domain restart
                HttpRuntime.UnloadAppDomain();
                return;
            });

            // switch optimizations on
            if (Config.BundleMinify)
                BundleTable.EnableOptimizations = true;
            BundleTable.EnableOptimizations = false;

        }
        private static void addXmlBundles(BundleCollection bundles)
        {
            var bundlingInfo = BundleXmlReader.readBundleXml(bundleXmlPath);

            if (bundlingInfo.Css != null)
            {
                foreach (var item in bundlingInfo.Css)
                {
                    // check if has .less
                    bool hasLess = false;
                    foreach (var file in item.Path)
                        if (file.EndsWith(".less"))
                            hasLess = true;

                    // Create bundle with Transforms
                    var newStyleBundle = new StyleBundle(item.Name);
                    //if (hasLess)
                    //    newStyleBundle.Transforms.Add(new LessTransform());

                    // Include Paths with CSS Rewrite Rules
                    var paths = item.Path.Distinct().ToArray();
                    foreach (var path in paths)
                        newStyleBundle.Include(path, new CssRewriteUrlTransformFixed());

                    // Add Bundle
                    bundles.Add(newStyleBundle);

                    //if (hasLess)
                    //    bundles.Add(new Bundle(item.Name, new LessTransform(), new CssMinify()).Include(item.Path.Distinct().ToArray()));//bundles.Add(new Bundle(item.Name, new LessTransform(), new CssMinify()).Include(item.Path.Distinct().ToArray()));
                    //else
                    //    bundles.Add(new Bundle(item.Name, new CssMinify()).Include(item.Path.Distinct().ToArray()));                                      
                }
            }

            // add JS bundles
            if (bundlingInfo.Js != null)
                foreach (var item in bundlingInfo.Js)
                    bundles.Add(new Bundle(item.Name, new JsMinify()).Include(item.Path.Distinct().ToArray()));
        }


        //// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        //public static void RegisterBundles(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                "~/Scripts/jquery-{version}.js"));

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        //                "~/Scripts/jquery.validate*"));

        //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
        //    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //                "~/Scripts/modernizr-*"));

        //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //              "~/Scripts/bootstrap.js",
        //              "~/Scripts/respond.js"));

        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //              "~/Content/bootstrap.css",
        //              "~/Content/site.css"));
        //}
    }
}
