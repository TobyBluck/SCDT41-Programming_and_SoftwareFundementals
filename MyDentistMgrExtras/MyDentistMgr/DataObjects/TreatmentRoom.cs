using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDentistMgr.UserObjects;
using MyDentistMgr.Libraries;

namespace MyDentistMgr.DataObjects
{
    class TreatmentRoom
    {
        private int roomNumber;
        private DentalPractice practice;
        private DentistNurse dentist;
        private DentistNurse nurse;

        private string dentistId;
        private string nurseId;


        public TreatmentRoom(int roomNumber, DentalPractice practice, string dentist, string nurse)
        {
            this.roomNumber = roomNumber;
            this.practice = practice;
            dentistId = dentist;
            nurseId = nurse;
        }

        /**
        * Allocates dentists and nurses to the room.
        */
        public void associateStaff()
        {
            dentist = (DentistNurse) DataSearching.findUser(dentistId); //Finds the user with the provided ID to set as rooms the dentist.
            nurse = (DentistNurse) DataSearching.findUser(nurseId);
        }

        /*
        * Getters and Setters are used because sometimes there is extra code to be run to handle the update of that variable
        * Values of variable might dictate another value so we do not want direct access to the variables which is why we have our object properties declared private.
        */
        #region Getters and Setters

        public int getRoomNumber()
        {
            return roomNumber;
        }

        public void setRoomNumber(int number)
        {
            roomNumber = number;
        }

        public DentalPractice getPractice()
        {
            return practice;
        }

        public void setPractice(DentalPractice practice)
        {
            this.practice = practice;
        }

        public DentistNurse getDentist()
        {
            return dentist;
        }

        public void setDentist(DentistNurse dentist)
        {
            this.dentist = dentist;
            dentistId = dentist.getUsername();
        }

        public DentistNurse getNurse()
        {
            return nurse;
        }

        public void setNurse(DentistNurse nurse)
        {
            this.nurse = nurse;
            nurseId = nurse.getUsername();
        }

        public string getDentistId()
        {
            return dentistId;
        }

        public string getNurseId()
        {
            return nurseId;
        }

        #endregion

    }
}
