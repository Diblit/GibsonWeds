using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace GibsonWeds.Utility
{
    public class BundleXmlReader
    {
        public static Bundling readBundleXml(string path)
        {
            string filePath = String.Empty;
            string bundleName = String.Empty;
            try
            {
                string actualPath = HttpRuntime.AppDomainAppPath + path.Replace("~", string.Empty).Replace('/', '\\');

                StreamReader reader = new StreamReader(actualPath);
                XmlSerializer serializer = new XmlSerializer(typeof(Bundling));
                Bundling bundlingInfo = (Bundling)serializer.Deserialize(reader);
                reader.Close();
                reader.Dispose();
                return bundlingInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("There is a problem while creating Bundle", ex);
            }
        }
    }
    [Serializable]
    public class Js
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlElement]
        public string[] Path { get; set; }
    }
    public class Css
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlElement]
        public string[] Path { get; set; }
    }
    [Serializable]
    public class Bundling
    {
        [XmlElement]
        public Js[] Js { get; set; }
        [XmlElement]
        public Css[] Css { get; set; }
    }
}