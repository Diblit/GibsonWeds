using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibsonWeds.DAL.Classes.Shared
{
    public class bl_SelectBox
    {
        public long value { get; set; }
        public string text { get; set; }
        public string extra { get; set; }
        public long ExtraID { get; set; }
        public bool isSelected { get; set; }
    }
    public class bl_KeyPair
    {
        public string value { get; set; }
        public string text { get; set; }
        public bool isSelected { get; set; }
    }
}
