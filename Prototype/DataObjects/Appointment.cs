using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyDentistMgr.Libraries;
using MyDentistMgr.UserObjects;

namespace MyDentistMgr.DataObjects
{
    struct AppointmentNote //Uses a structure as all data can be initialised at once and will not change. 
    {
        public string date;
        public string time;
        public DentistNurse author;
        public Appointment appointment;
        public string content;

        public AppointmentNote(string date, string time, Appointment appointment, DentistNurse author, string content) 
        {
            this.date = date;
            this.time = time;
            this.appointment = appointment;
            this.author = author;
            this.content = content;
        }

        public AppointmentNote(DateTime date, Appointment appointment, DentistNurse author, string content)
        {
            this.author = author;
            this.content = content;
            this.appointment = appointment;

            this.date = date.ToString("D");//Converts a date to a string, parameter determines format. This particular returns as D/MM/YYYY https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tostring?view=netcore-3.1#System_DateTime_ToString_System_String_ 
            time = date.ToString("T"); //H:mm am/pm
        }
    }

    class Appointment //Create class w/ private only accessible in class or struct declared above 
    {
        private List<AppointmentNote> notes = new List<AppointmentNote>(); //Creating note list 
        private TreatmentRoom room;
        private Patient patient;
        private string appointmentId;
        private string date;
        private string time;

        #region Constructor and overloads

        public Appointment(Patient patient, TreatmentRoom room, DateTime dateTime)
        {
            string apptPath = DataIO.patientPath + $"\\{patient.getId()}\\Appointments"; //File path for appointments 

            this.patient = patient;
            this.room = room;

            date = dateTime.ToString("D");
            time = dateTime.ToString("t");

            appointmentId = GeneralFunctions.generateId(apptPath, "Apmnt"); //Generates its own ID
        }

        public Appointment(string appointmentId, Patient patient, TreatmentRoom room, string date, string time) //already has its own ID so doesnt need to generate one 
        {
            this.patient = patient;
            this.room = room;
            this.appointmentId = appointmentId;
            this.date = date;
            this.time = time;
        }

        #endregion
        /*
         * Getters and Setters are used because sometimes there is extra code to be run to handle the update of that variable
         * Values of variable might dictate another value so we do not want direct access to the variables which is why we have our object properties declared private.
        */
        #region Getters and Setters

        public string getDate()
        {
            return date;
        }

        public void setDate(string date)
        {
            this.date = date;
        }

        public void setDateTime(DateTime date)
        {
            this.date = date.ToString("D");
            time = date.ToString("t");
        }

        public void setTime(string time)
        {
            this.time = time;
        }

        public string getTime()
        {
            return time;
        }

        public string getId()
        {
            return appointmentId;
        }

        public void setId(string appointmentId)
        {
            this.appointmentId = appointmentId;
        }

        public TreatmentRoom getRoom()
        {
            return room;
        }

        public void setRoom(TreatmentRoom room)
        {
            this.room = room;
        }

        public Patient getPatient()
        {
            return patient;
        }

        public void setPatient(Patient patient)
        {
            this.patient = patient;
        }

        public List<AppointmentNote> getNotes()
        {
            return notes;
        }

        #endregion //regions allows you to capture a block of code to expand or collapse keeping the .cs clean and consitent 
    }
}
