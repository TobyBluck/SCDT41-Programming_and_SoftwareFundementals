using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class Request
    {
        string requestor;
        int priority;
        DateTime date;

        public string Requestor
        { 
            get { return requestor; }
            set { requestor = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public Request (string requestor, int priority, DateTime date)
        {
            this.Requestor = requestor;
            this.Priority = priority;
            this.Date = date; 
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", this.Requestor, this.Priority, this.Date);
        }
    }
}
