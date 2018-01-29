using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibsonWeds.DAL.Utils
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException() : base() { }
        public RecordNotFoundException(string message) : base(message) { }
        public RecordNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
