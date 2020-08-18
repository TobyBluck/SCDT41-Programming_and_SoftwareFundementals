using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Request> reqList = new List<Request>();
            reqList.Add(new Request("Dan", 2, new DateTime(2011, 4, 2)));
            reqList.Add(new Request("Alice", 5, new DateTime(2011, 2, 5)));
            reqList.Add(new Request("Bill", 3, new DateTime(2011, 6, 19)));

            Console.WriteLine("Sorted by date");
            reqList.Sort(new DataSorter()); 

            foreach (Request req in reqList)
            {
                Console.WriteLine(req.ToString());
            }
            Console.ReadLine(); 
        }
    }
}
