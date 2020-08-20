using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyDentistMgr.Libraries;
using MyDentistMgr.UserObjects;

namespace MyDentistMgr.DataObjects
{
    enum Gender { MALE, FEMALE }; //Using genders enums to set gender of patient  

    class Patient
    {
        private List<Appointment> appointments = new List<Appointment>();

        private string patientId;
        private string name;
        private DentalPractice practice;
        private Gender gender;
        private string address;
        private string contactNo;

        public Patient(string patientId, string name, DentalPractice practice, int gender, string address, string contactNo)
        {
            this.patientId = patientId;
            this.name = name;
            this.practice = practice;
            this.gender = (Gender) gender;
            this.address = address;
            this.contactNo = contactNo;
        }

        /*
        * Getters and Setters are used because sometimes there is extra code to be run to handle the update of that variable
        * Values of variable might dictate another value so we do not want direct access to the variables which is why we have our object properties declared private.
       */
        #region Getters and Setters

        public string getId()
        {
            return patientId;
        }

        public void setId(string id)
        {
            patientId = id;
        }

        public string getName()
        {
            return name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public DentalPractice getPractice()
        {
            return practice;
        }

        public void setPractice(DentalPractice practice)
        {
            this.practice = practice;
        }

        public void setPractice(string location)
        {
            if(DataSearching.practiceExists(location))
            {
                practice = DataSearching.findPractice(location);
            }
        }

        public Gender getGender()
        {
            return gender;
        }

        public void setGender(Gender gender)
        {
            this.gender = gender;
        }

        public void setGender(int gender) //Having a function with the same name but different parameters is called overloading.
        {
            this.gender = (Gender) gender; //Enums can be casted from integers.
        }

        public string getAddress()
        {
            return address;
        }

        public void setAddress(string address)
        {
            this.address = address;
        }

        public string getContactNo()
        {
            return contactNo;
        }

        public void setContactNo(string contactNo)
        {
            this.contactNo = contactNo;
        }

        public List<Appointment> getAppointments()
        {
            return appointments;
        }

        #endregion


    }
}
