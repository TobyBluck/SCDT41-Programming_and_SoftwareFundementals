using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDentistMgr.UserObjects;
using MyDentistMgr.DataObjects;

namespace MyDentistMgr.Libraries
{
    static class DataSearching
    {

       /**
       * Finds a the practice with the given name.
       */
        public static DentalPractice findPractice(string location)
        {
            DentalPractice practiceToFind;
            List<DentalPractice> practices = Application.appInstance.dentalPractices;

            try
            {
                practiceToFind = practices.Find(practice => practice.getLocation() == location); //The List.Find() function takes what is called a Predicate. which acts is basically a search filter. https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.find?view=netcore-3.1
            }
            catch (Exception e)
            {
                practiceToFind = new DentalPractice("", "", ""); //havent defined it outside, this is used to prevent an error 
                GeneralFunctions.errorHandler(e);
            }
            return practiceToFind;
        }

        /**
        * Checks if a Practice exists.
        */
        public static bool practiceExists(string location)
        {
            bool exists;
            List<DentalPractice> practices = Application.appInstance.dentalPractices;

            try
            {
                exists = practices.Exists(practice => practice.getLocation() == location); //same as above  
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
                exists = false; //prevents errors as exsist is empty if it fails.  
            }
            return exists;
        }

        /**
        * Finds a user with the given username.
        */
        public static User findUser(string username)
        {
            User userToFind;
            List<User> users = Application.appInstance.users;

            try
            {
                userToFind = users.Find(user => user.getUsername() == username); //same as findPractice 
            }
            catch (Exception e)
            {
                userToFind = new DentistNurse("", "", "", "", ""); //prevents errors 
                GeneralFunctions.errorHandler(e);
            }
            return userToFind;
        }

        /**
        * Determines if the user with the given name exists,
        */
        public static bool userExists(string username)
        {
            return Application.appInstance.users.Exists(user => user.getUsername() == username);
        }

        /**
        * Returns a list of all staff.
        */
        public static List<Staff> getStaff()
        {
            List<Staff> staff = new List<Staff>();
            List<User> users = Application.appInstance.users;

            try
            {
                for(int i = 0; i < users.Count; i ++)
                {
                    if(users[i] is Staff) //is keyword checks to see if the object on the left is or is derived from the class on the right. https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/is
                    {
                        staff.Add((Staff) users[i]); //Cast the user into a staff object and adds it to the staff list.
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
            return staff;
        }

        /**
        * Gets all Receptionists.
        */
        public static List<Receptionist> getReceptionists()
        {
            List<Receptionist> receptionists = new List<Receptionist>();
            List<User> users = Application.appInstance.users;

            try
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i] is Receptionist) //runs through users and find anyone that is a receptionist 
                    {
                        receptionists.Add((Receptionist)users[i]); //Cast the user into a staff object and adds it t the staff list.
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
            return receptionists;
        }

        /**
        * Checks a patient with the given id exists at the given practice.
        */
        public static bool patientExists(DentalPractice practice, string id)
        {
            return practice.getPatients().Exists(patient => patient.getId() == id); //Checks if a patient at the given location with the given id exists.
        }

        /**
        * gets a patient with the given id and given practice.
        */
        public static Patient findPatient(DentalPractice practice, string id)
        {
            return practice.getPatients().Find(patient => patient.getId() == id); //Returns the patient objects with the given patient ID 
        }

        /**
        * checks if an appointment with the given id exists.
        */
        public static bool appointmentExists(Patient patient, string id)
        {
            return patient.getAppointments().Exists(appointment => appointment.getId() == id); //Checks if the given patient has an appointment with the given ID 
        }

        public static Appointment findAppointment(Patient patient, string id)
        {
            return patient.getAppointments().Find(appointment => appointment.getId() == id); //Gets the appointment based on given ID  
        }

        /**
        * Gets all appointments at a given location.
        */
        public static List<Appointment> getAppointments(DentalPractice practice)
        {
            List<Patient> patients = practice.getPatients(); //Gets the patients within the practice/s 
            List<Appointment> appointments = new List<Appointment>(); ; //Creates a list to store appointments. 

            try
            {
                for(int i = 0; i < patients.Count; i ++) //iterates on patients  
                {
                    appointments.AddRange(patients[i].getAppointments()); //Adds all appointments for the patient to the appointments list. //addrange takes one list and adds another list to it 
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
            return appointments;
        }

        /**
        * Gets the daily appointments for a given practitioner
        */
        public static List<Appointment> getDailyPractitionerAppointments(DentistNurse practitioner) 
        {
            List<Appointment> practitionerAppointments = getPractitionerAppointments(practitioner); //Gets the list of all appointments for the practioner 
            DateTime currentDate = DateTime.Now; //Create a time object corresponding to the current time 
            string currentDateStr;

            List<Appointment> dailyAppointments; //List to store appointments on the day 

            try
            {
                currentDateStr = currentDate.ToString("D"); //converts the date object to a string & deterimes format dd/mm/yyyy 
                dailyAppointments = practitionerAppointments.FindAll(appointment => appointment.getDate() == currentDateStr); //finds all the apppointments on the current date 
            }
            catch (Exception e)
            {
                dailyAppointments = new List<Appointment>(); //Creates a empty list for daily appointments in case of error 
                GeneralFunctions.errorHandler(e);
            }
            return dailyAppointments;
        }

        /**
        * Gets the daily appointments for a given location
        */
        public static List<Appointment> getDailyLocationAppointments(DentalPractice location) 
        {
            List<Appointment> allAppointments = getAppointments(location); //gets the list of appointments at a location 
            DateTime currentDate = DateTime.Now; //Create a time object corresponding to the current time  
            string currentDateStr;

            List<Appointment> dailyAppointments; //List to store appointments on the day

            try
            {
                currentDateStr = currentDate.ToString("D"); //converts the date object to a string & deterimes format dd/mm/yyyy 
                dailyAppointments = allAppointments.FindAll(appointment => appointment.getDate() == currentDateStr); //finds all the apppointments on the current date 
            }
            catch (Exception e)
            {
                dailyAppointments = new List<Appointment>();
                GeneralFunctions.errorHandler(e);
            }
            return dailyAppointments;
        }

        /**
        * Gets appointments for a given Dentist/Nurse.
        */
        public static List<Appointment> getPractitionerAppointments(DentistNurse practitioner)
        {
            List<Appointment> allAppointments = getAppointments(practitioner.getPractice()); //Get all appointments at a practioners practice 
            List<Appointment> practitionerAppointments;

            try
            {
                practitionerAppointments = allAppointments.FindAll(appointment => appointment.getRoom().getDentist().Equals(practitioner) || appointment.getRoom().getNurse().Equals(practitioner)); //Gets all appointments for the pratictioner or nurse. || is an OR operator which runs for the nurse. .equals is used for objects but is the same as == operator 
            }
            catch (Exception e)
            {
                practitionerAppointments = new List<Appointment>();
                GeneralFunctions.errorHandler(e);
            }
            return practitionerAppointments;
        }
    }
}
