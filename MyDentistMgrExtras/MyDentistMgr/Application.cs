using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDentistMgr.UserObjects; // Allows use of the UserObjects namespace.
using MyDentistMgr.Libraries; // Same for Libraries.
using MyDentistMgr.DataObjects; //and for data objects.
using System.Reflection;

namespace MyDentistMgr
{
    class Application
    {
        public static Application appInstance; // used to keep a refererence to the Application instance, There should only be one of these so its best to make this static and accessible via the class.
        public List<User> users = new List<User>();
        public List<DentalPractice> dentalPractices = new List<DentalPractice>();

        private User currentUser; //Stores the current user.

        public Application() // Constructor, called when an instance of this class is created. 
        {
            bool active = true; //used to keep the login system iterating until the application is exited.

            try
            {
                appInstance = this; // Sets the appInstance variable to reference Application object created in program.cs. 
                initialiseData(); //Loads in the data or makes the directory 
                Console.WriteLine("Welcome. Please log in. (Provide an empty username to exit.)");

                do
                {
                    active = loginUser(); //Login to system
                }
                while(active);
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /*
        * initialises program data (users, appointments, patients etc)
        */
        private void initialiseData() //doesnt need to return a function thats why it is private. 
        {
            try
            {
                DataIO.initialiseDataDirectory();
                DataIO.loadPractices();
                DataIO.loadUsers();
                DataIO.associateStaffWithPractices();
                DataIO.loadPatients();
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /*
        * Promts the user for a username and password. if nothing is entered in the username, will exit the loop.
        */
        private bool loginUser()
        {
            bool active = true; // feeds back to Application to determine if the system should keep requesting credentials or exit.
            bool invalidCredentials = true;


            string username; // holds the username following input.
            string password; // Holds the password following input.

            try
            {
                do
                {
                    Console.Write("Username: ");
                    username = Console.ReadLine();

                    active = username != ""; //checks to see if username is not empty and stores the boolean result in the active variable.

                    if (active)
                    {
                        Console.Write("Password: ");
                        password = Console.ReadLine();
                        Console.Clear();

                        if (DataSearching.userExists(username)) //Validates user response 
                        {
                            currentUser = DataSearching.findUser(username); //Finds valid user 
                            if(password == currentUser.getPassword())
                            {
                                invalidCredentials = false; //Indicates that valid credentials have been provided.
                                currentUser.showMainMenu(); // Loads the users main menu.
                            }
                        }

                        if(invalidCredentials) //if boolean value is true 
                        {
                            Console.WriteLine("Invalid credentials");
                        }
                    }
                    else
                    {
                        break; //Exits out of loop when active is false.
                    }
                    
                }
                while (invalidCredentials);
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
            return active;
        }

        /*
        * Gets the current user.
        */
        public User getCurrentUser()
        {
            return currentUser;
        }
    }
}
