using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyDentistMgr.Libraries;
using MyDentistMgr.DataObjects;

namespace MyDentistMgr.UserObjects
{
    class DentistNurse: Staff
    {
        private string practitionerType;

        public DentistNurse(string username, string password, string name, string location, string type): base(username, password, name, location) //inherits from Staff.cs
        {
            practitionerType = type; //
        }

        /**
        * Main menu for a DentistNurse.
        */
        public override void showMainMenu() //override from User.cs & defined
        {
            int response;

            try
            {
                loggedIn = true;
                while(loggedIn)
                {
                    Console.Clear();
                    response = GeneralFunctions.getRequiredSelection("Main Menu", new string[] {"View Patients", "Appointments", "Log out"}); //Selection for menu 

                    switch(response)
                    {
                        case 1:
                            Console.Clear();
                            DataPrinting.printPatients(practice); //Prints all Patients 
                            Console.WriteLine("Press Enter to continue.");
                            Console.ReadLine();
                            break;
                        case 2:
                            showAppointments();
                            break;
                        case 3:
                            loggedIn = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * displays appointments and allows user to add notes.
        */
        private void showAppointments()
        {
            List<Appointment> appointments = DataSearching.getPractitionerAppointments(this); //this refers to instance of object, Gets the list of appointments for the current logged in practioner. w
            string response = "";
            bool inMenu = true;

            try
            {
                while(inMenu)
                {
                    Console.Clear();
                    DataPrinting.printPractitionerAppointments(this); //Prints Appointments based on the current logged in Practioner. 
                    response = GeneralFunctions.getOptionalInput("Add note to appointment: ", false);

                    if(response != "")
                    {
                        if(appointments.Exists(appointment => appointment.getId() == response)) //finds defined condition, acts as a search filter, has been explained in a different a usertype .cs
                        {
                            addNote(appointments.Find(appointment => appointment.getId() == response)); 
                        }
                        else
                        {
                            Console.WriteLine("Appointment not found.");
                        }
                    }
                    else
                    {
                        inMenu = false;
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Allows a user to add a note to an appointment.
        */
        private void addNote(Appointment appointment)
        {
            AppointmentNote note;
            string content = "";

            try
            {
                content = GeneralFunctions.getRequiredInput("Note: ");
                note = new AppointmentNote(DateTime.Now, appointment, this, content);
                appointment.getNotes().Add(note); //addnote to appointment
                DataIO.saveAppointmentNote(note); //saves the files//saves the files
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }

        }

        //Regions can be used to mark collapsable blocks in code. start these with #region <name> and end them with #endregion.
        #region Getters And Setters

        public string getPractitionerType()
        {
            return practitionerType;
        }

        public void setPractitionerType(string type)
        {
            practitionerType = type;
        }

        #endregion
    }
}
