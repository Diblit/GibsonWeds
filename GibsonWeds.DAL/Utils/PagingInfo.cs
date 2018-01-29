using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibsonWeds.DAL.Utils
{
    public class PagingInfo
    {
        //paging 
        public int skip { get; set; }
        public int take { get; set; }
        public int result_count { get; set; }

        //searching
        public string SearchString { get; set; }

        //sorting
        public string sort_col { get; set; }
        public bool sort_isAsc { get; set; }

        public PagingInfo() { }

        public PagingInfo(int skip, int take, string SearchString)
        {
            this.skip = skip;
            this.take = take;
            this.SearchString = SearchString;
        }
    }
}
