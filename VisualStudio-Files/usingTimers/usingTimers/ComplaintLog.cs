using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usingTimers
{
    public class Complaint_Log
    {

        public string Title;
        public string Name;
        DateTime Submission;
        public string Details;
        public int Phone; 

        public void LaunchComplaint()
        {

            Console.WriteLine("What is your name?");
            Name = Console.ReadLine();

            Console.WriteLine("What is your contact number?");
            Phone = int.Parse(Console.ReadLine());

            Console.WriteLine("What is the complaint?");
            Console.ReadLine();

            Submission = DateTime.Now; 

        }

    }
}
