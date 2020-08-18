using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class DataSorter
    {

        public class dataSorter:IComparer<Request>
        {
            public int Compare(Request R1, Request R2)
            {
                return R1.Date.CompareTo(R2.Date); 
            }
        }

    }
}
