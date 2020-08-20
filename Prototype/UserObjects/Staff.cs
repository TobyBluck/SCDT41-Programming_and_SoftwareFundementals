using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDentistMgr.DataObjects;
using MyDentistMgr.Libraries;

namespace MyDentistMgr.UserObjects
{
    abstract class Staff: User
    {
        protected DentalPractice practice;

        public Staff(string username, string password, string name, string location): base(username, password, name) //calls the constructor of the parent class, inheritance. https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/base
        {
            practice = DataSearching.findPractice(location);
        }

        /*
        * Gets the practice 
        */
        public DentalPractice getPractice()
        {
            return practice;
        }

        /*
        * Sets the practice 
        */
        public void setPractice(DentalPractice practice)
        {
            this.practice = practice;
        }

        /**
        * Displays information about a given patient.
        */
        protected void viewPatient(string patientId)
        {

        }
    }
}
