using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyDentistMgr.UserObjects;
using MyDentistMgr.Libraries;

namespace MyDentistMgr.DataObjects
{
    class DentalPractice
    {
        private string location;
        private string address;
        private List<TreatmentRoom> rooms = new List<TreatmentRoom>();
        private List<Patient> patients = new List<Patient>();

        private string receptionistId;
        private Receptionist receptionist;

        public DentalPractice(string location, string address, string receptionist)
        {
            this.location = location;
            this.address = address;
            receptionistId = receptionist;
            DataIO.loadTreatmentRooms(rooms, this);
        }

        #region Getters and Setters

        public string getLocation()
        {
            return location;
        }

        public void setLocation(string location)
        {
            this.location = location;
        }

        public string getAddress()
        {
            return address;
        }

        public void setAddress(string address)
        {
            this.address = address;
        }

        public Receptionist getReceptionist()
        {
            return receptionist;
        }

        public void setReceptionist(Receptionist receptionist)
        {
            this.receptionist = receptionist;
            receptionistId = receptionist.getUsername();
        }

        public List<Patient> getPatients()
        {
            return patients;
        }

        public List<TreatmentRoom> getRooms()
        {
            return rooms;
        }

        #endregion

        /**
        * gets the Receptionist object from the receptionistId.
        */
        public void associateReceptionist() //Void for the return type of variable/object
        {
            try
            {
                if (receptionistId != "")
                {
                    receptionist = (Receptionist)DataSearching.findUser(receptionistId); //Because we are taking from a list of Users and wish to assign this to a Receptionist, it must be casted back.
                    receptionist.setPractice(this);
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }
    }
}
