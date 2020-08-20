using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyDentistMgr.Libraries;
using MyDentistMgr.DataObjects;

namespace MyDentistMgr.UserObjects
{
    class Receptionist: Staff
    {
        public Receptionist(string username, string password, string name, string location): base(username, password, name, location) //Same as Staff.cs calling the User.cs 
        {

        }

        /**
        * Code to show the main menu for a receptionist.
        */
        public override void showMainMenu() //Override showMainMenu from the User.cs 
        {
            string response;
            int responseInt;

            try
            {
                loggedIn = true; //bool value for chekcing if user is logged in
                Console.Clear();
                printWelcomeMessage();//pulled from generalFucntions and will show name as well 

                do
                {
                    //Printing of options
                    Console.WriteLine("1 - Patients");
                    Console.WriteLine("2 - Appointments");
                    Console.WriteLine("3 - Logout");

                    response = Console.ReadLine();
                    Console.Clear();

                    if (int.TryParse(response, out responseInt)) //int.TryParse will determine if the response is an integer, return true or false depending on this and if true, store the result in the responseInt. https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
                    {
                        if((responseInt >= 1) && (responseInt <= 3)) //Checks number is in range. && is a conditional logical operator 
                        {
                            handleSelection(responseInt); //uses responseInt for switch case selection
                        }
                        else
                        {
                            Console.WriteLine("Please enter a number within range.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number.");
                    }
                }
                while (loggedIn);
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * handles the selected option
        */
        private void handleSelection(int selection) 
        {
            try
            {
                switch(selection)
                {
                    case 1:
                        showPatientMenu();
                        break; //always break after a swicth 
                    case 2:
                        showAppointmentMenu();
                        break;
                    case 3:
                        loggedIn = false;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }
        
        #region Patient Menu Functions

        /**
        * Shows the patient menu.
        */
        private void showPatientMenu()
        {
            string response;
            int responseInt;
            bool inMenu;

            try
            {
                inMenu = true;
                Console.Clear();

                do
                {

                    //Printing of options
                    Console.WriteLine("1 - Add");
                    Console.WriteLine("2 - Remove");
                    Console.WriteLine("3 - Main Menu");

                    response = Console.ReadLine(); //reads user input 
                    Console.Clear();

                    if (int.TryParse(response, out responseInt)) //int.TryParse will determine if the 1st param is an integer, return true or false depending on this and if true, store the result in the second param. https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
                    {
                        if ((responseInt >= 1) && (responseInt <= 3)) //Checks number is in range.
                        {
                           switch(responseInt)
                            {
                                case 1:
                                    showPatientEditor();
                                    break;
                                case 2:
                                    removePatient();
                                    break;
                                case 3:
                                    inMenu = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else //if 0 or 4 or higher do this
                        {
                            Console.WriteLine("Please enter a number within range.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number.");
                    }
                }
                while (inMenu); //if inMenu is false close the loop 

            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }


        /**
        * Allows the creation of a patient.
        */
        private void showPatientEditor()
        {
            string patientId;
            string name;
            int gender;
            string address;
            string contactNo;

            Patient newPatient;
           
            try
            {
                name = GeneralFunctions.getOptionalInput("Name: ", false);//Request a name for patient, or allows cancellation 

                if(name != "") //not equal to empty 
                {
                    gender =  GeneralFunctions.getRequiredSelection("Gender: ", new string[] { "Male", "Female" }) - 1; //Takes a selection from a user and subtractes one, so it can be indexed. 
                    address = GeneralFunctions.getRequiredInput("Address: "); //Address must be entered, due to getRequiredInput
                    contactNo = GeneralFunctions.getRequiredInput("Contact Number: ");
                    patientId = GeneralFunctions.generateId(DataIO.patientPath, name); //Generates unique ID for patient. 
                    newPatient = new Patient(patientId, name, practice, gender, address, contactNo); //creates new patient 
                    practice.getPatients().Add(newPatient); //add the new patient to the practice. 
                    DataIO.savePatient(newPatient); //saves patient to files
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prompts the user for a patient to remove.
        */
        private void removePatient()
        {
            string response = "";
            Patient patientToRemove;
            bool validResponse = false;

            try
            {
                do
                {
                    DataPrinting.printPatients(practice);//show patients based on practices 
                    response = GeneralFunctions.getOptionalInput("Patient to remove: ", false); //boolean to skip 

                    if (response != "")
                    {
                        if (DataSearching.patientExists(practice, response)) //check patient exists with given ID
                        {
                            File.Delete(DataIO.patientPath + "\\" + response + ".txt"); //Deletes the patient file.
                            patientToRemove = DataSearching.findPatient(practice, response); //Finds the patient with the given ID 
                            practice.getPatients().Remove(patientToRemove);//Remove the patient from the practice. 
                            validResponse = true;
                        }
                        else
                        {
                            Console.WriteLine("Patient does not exist.");
                        }
                    }
                    else
                    {
                        validResponse = true;
                    }
                }
                while (!validResponse);
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        #endregion

        /**
        * Shows the appointment menu
        */
        private void showAppointmentMenu()
        {
            int response;
            bool inMenu = true;

            try
            {
                while(inMenu)
                {
                    Console.Clear();
                    response = GeneralFunctions.getRequiredSelection("Appointments:\n", new string[] {"Add", "Remove", "Main Menu"});

                    switch(response)
                    {
                        case 1:
                            createAppointment();
                            break;
                        case 2:
                            removeAppointment();
                            break;
                        case 3:
                            inMenu = false;
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
        * Allows the user to create an appointment.
        */
        private void createAppointment()
        {
            string response = "";
            int responseInt;
            string[] roomOptions = new string[practice.getRooms().Count]; //Creates an array to store the room options to be printed when requesting user input
            Patient patient;
            TreatmentRoom room;
            bool inMenu = true;
            Appointment aptmtToAdd;
            DateTime dateTime;

            try
            {
                for(int i = 0; i < practice.getRooms().Count; i ++) //determines available practices. 
                {
                    roomOptions[i] =  $"{i + 1} - {practice.getRooms()[i].getDentist().getName()}/{practice.getRooms()[i].getNurse().getName()}"; //Adds an option to be displayed when requesting a room. formatt: <roomNo> - <dentist>/<Nurse>
                }

                while(inMenu)
                {
                    Console.Clear();
                    DataPrinting.printPatients(practice);
                    response = GeneralFunctions.getOptionalInput("Patient: ", false); 

                    if(response != "")
                    {
                        if (DataSearching.patientExists(practice, response)) //check patient exsist 
                        {
                            patient = DataSearching.findPatient(practice, response); //Finds patient at a practice based on given ID. 

                            responseInt = GeneralFunctions.getRequiredSelection("Room: ", roomOptions) - 1; //Requests user for the appointment and takes response and subtracts one to conver to index. 
                            room = practice.getRooms()[responseInt];

                            dateTime = GeneralFunctions.getDateTimeInput();

                            aptmtToAdd = new Appointment(patient, room, dateTime);
                            patient.getAppointments().Add(aptmtToAdd);
                            DataIO.saveAppointment(aptmtToAdd); //save to files
                        }
                        else
                        {
                            Console.WriteLine("Patient not found.");
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
        * Allows user to remove an appointment.
        */
        private void removeAppointment() //Accesible by the class and does not return a value. 
        {
            string response = "";//Use input
            Appointment apptToRemove;
            Patient apptOwner;
            bool inMenu = true;
            bool invalidResponse = true;
            string apptPath = "";

            try
            {
                while (inMenu)
                {
                    DataPrinting.printPatients(practice);
                    response = GeneralFunctions.getOptionalInput("Select Patient: ", false); //breaks loop to appointments menu

                    if(response != "") //not equal to blank 
                    {
                        if(DataSearching.patientExists(practice, response))//Checks patient exsists based on given ID 
                        {
                            apptOwner = DataSearching.findPatient(practice, response); //sets patient as apptOwner with given ID 
                            invalidResponse = true; //Runs next while loop
                            while (invalidResponse)
                            {
                                DataPrinting.printAppointments(apptOwner); //Shows appointment of apptOwner 
                                response = GeneralFunctions.getOptionalInput("Appointment: ", false); //Request appointment to delete or option to cancel
                                if (response != "")
                                { 
                                    if (DataSearching.appointmentExists(apptOwner, response)) //Checks if appointment exsists based on given ID 
                                    {
                                        apptToRemove = DataSearching.findAppointment(apptOwner, response); //Gets the appointment with that ID 
                                        apptPath = $"{DataIO.patientPath}\\{apptOwner.getId()}\\Appointments\\{apptToRemove.getId()}"; //Determines the path to the appointments directory

                                        apptOwner.getAppointments().Remove(apptToRemove); //Gets the list of appointments and removes the given appointment. 
                                        Directory.Delete(apptPath, true); //path to appointments, true = allowing you to deletes all the folders content. 
                                        invalidResponse = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid appointment id.");
                                    }
                                }
                                else
                                {
                                    invalidResponse = false;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Patient does not exist.");
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
    }
}
