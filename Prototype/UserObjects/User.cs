using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyDentistMgr.Libraries;

namespace MyDentistMgr.UserObjects
{
    abstract class User // Class is abstract meaning it cannot be intantiated. (cannot be created using the 'new' keyword). https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract 
    {
        private string username;
        private string password;
        private string name;
        protected bool loggedIn; //Protected means this is accessible by extended classes. 

        /*
        * Constructs the user.
        */
        public User(string username, string password, string name)
        {
            this.username = username;
            this.password = password;
            this.name = name;
        }

        /*
        * Gets the username
        */
        public string getUsername()
        {
            return username;
        }

        /*
        * sets the username
        */
        public void setUsername(string username)
        {
            this.username = username;
        }

        /*
        * Gets the password
        */
        public string getPassword()
        {
            return password;
        }

        /*
        * Sets the password
        */
        public void setPassword(string password)
        {
            this.password = password;
        }

        /*
        * Gets the name
        */
        public string getName()
        {
            return name;
        }

        /*
        * Sets the name
        */
        public void setName(string name)
        {
            this.name = name;
        }

        /**
        * Prints a welcome message to the user.
        */
        protected void printWelcomeMessage()
        {
            try
            {
                Console.WriteLine($"Welcome {name}.\nPlease select from the following options:"); // by using $, you can use {} to insert variables. https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated, examples and tuts online for this shortcut.
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        public abstract void showMainMenu(); //This defines a function that means all intiansiatable dervatives/inherited users (Administrator, Receptionist and DentistNurse) must define and override this function. https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override
    }
}
