using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingWeek2_OutClass
{
    class Program
    {

        public class Person
        {

            public string Name; //name 
            public int Age; //Age 

            public Person()
            {

            }

            public Person(string name)
            {
                this.Name = name; 
            }

            public Person (string name, int age)
            {
                this.Name = name;
                this.Age = age; 
            }

        }

        //------------------------------------------------------------------------

        public class Staff : Person
        {

            public int StaffID;
            public int StaffSalary;

            public Staff()
            {

            }

            public Staff(int staffID)
            {
                this.StaffID = staffID; 
            }

            public Staff(int staffID, int staffSalary)
            {
                this.StaffID = staffID;
                this.StaffSalary = staffSalary;
            }

            public void ShowPass()
            {
                Console.WriteLine("My name is {0}", Name);
                Console.WriteLine("My ID number is: " + StaffID);
                Console.WriteLine("Salary is: " + StaffSalary);
            }

            //------------------------------------------------------------------------

            public class Visitors : Person
            {

                public string WhoSee;
                public int LenghtTime;

                public Visitors()
                {
                
                }

                public Visitors(string whoSee)
                {
                    this.WhoSee = whoSee; 
                }

                public Visitors(string whoSee, int lenghtTime)
                {
                    this.WhoSee = whoSee;
                    this.LenghtTime = lenghtTime;
                }

                public void ShowPass()
                {
                    Console.WriteLine("My name is {0}", Name);
                    Console.WriteLine("Who to see is {1}", WhoSee);
                    Console.WriteLine("Lenght of time(HRS) = " + LenghtTime);
                   
                }

            }

            //-------------------------------------------------------------------------

            public class Clients : Person
            {

                // boolean for mainte or Delivery 

                //GUID as a identification which prints as proof, use something like bleow which adds a time stamps. 
                /*var ticks = DateTime.Now.Ticks;
                var guid = Guid.NewGuid().ToString();
                var uniqueSessionId = ticks.ToString() + '-' + guid; //guid created by combining ticks and guid

                var datetime = new DateTime(ticks);//for checking purpose
                var datetimenow = DateTime.Now;    //both these date times are different.*/
            }

        }

        static void Main(string[] args)
        {
           /* Stopwatch timer = new Stopwatch();
            timer.Start();
            ---code here---
            timer.Stop();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", stopwatch.Elapsed); */
        }
    }
}
