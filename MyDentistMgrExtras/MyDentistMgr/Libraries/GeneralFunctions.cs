using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDentistMgr.Libraries
{
    static class GeneralFunctions //Class is used as a library and therefore does not need to be instantiated. As a result of the class beingf static, all functions contained must also be static. https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members
    {
        /*
        * Function to handle errors in try/catch blocks.
        */
        public static void errorHandler(Exception e)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Error occured at: " + e.Source + "\n");
                Console.WriteLine(e.Message);
                Console.WriteLine("\nStack Trace:\n============");
                Console.WriteLine(e.StackTrace); //Calling constructors etc gets added to the stack, after execution gets removed, stack trace used to find route of coude and prints when error occurs  
                Console.WriteLine("\nPress Enter to continue.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                errorHandler(ex);
            }
        }

        /**
       * Generates a unique ID from the given text.
       * This will take the first 5 letters of a given text and append a number to avoid duplicates.
       */
        public static string generateId(string dataPath, string text)
        {
            string id = "";
            int numInc = 1;
            string idText;

            try
            {
                if (text.Length > 5) //checks id text string is greater than 5  
                {
                    idText = text.Substring(0, 5); //Gets a section of starting at point 0 with a length of 5.
                }
                else
                {
                    idText = text;
                }

                while (File.Exists($"{dataPath}\\{idText}{numInc}.txt") || Directory.Exists($"{dataPath}\\{idText}{numInc}")) // || is a logical operator OR. takes booleans and converts it to one, checking for the existence of a folder or a file. if either exist then the result will be true.
                {
                    numInc++; //Checks to see if the file already exists. if so, increments the trailing number. In the same way that if statments can be closed off with ; and execute only that line, as can while loops and for loops. 
                }
                id = idText + numInc;
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
            return id; //Output the value.  
        }

        /**
        * Gets a required input.
        */
        public static string getRequiredInput(string message)
        {
            string response = "";

            try
            {
                while (response == "") //Response is equal to nothing
                {
                    Console.Write(message);
                    response = Console.ReadLine(); //Records input  
                    Console.Clear(); //Clears console  

                    if (response == "")
                    {
                        Console.WriteLine("Please enter a value.");//No input displays this  
                    }
                }

            }
            catch (Exception e)
            {
                errorHandler(e);
            }
            return response;
        }

        /**
        * Gets a required input and executes provided function.
        */
        public static string getRequiredInput(string message, Action printFunction) //Overloads the getRequiredInput function allowing a reference to a function that prints options to be passed in. this will be executed before printing the message. Actions are a method without prarametres adn does not return values. Functions that return values are referred to as Func (can be found in micorsoft doc and tuts on websites.)
        {
            string response = "";

            try
            {
                while (response == "")
                {
                    Console.Write(message);
                    printFunction(); //Executing the passed function.
                    response = Console.ReadLine();
                    Console.Clear();

                    if (response == "")
                    {
                        Console.WriteLine("Please enter a value.");
                    }
                }

            }
            catch (Exception e)
            {
                errorHandler(e);
            }
            return response;
        }

        /**
        * Prints a list of options and returns the selected option.
        */
        public static int getRequiredSelection(string message, string[] options)
        {
            string response = "";
            int responseInt = 0;
            bool invalidResponse = true;
            string error = "";

            try
            {
                

                while (invalidResponse)
                {
                    Console.WriteLine(message);
                    for (int i = 0; i < message.Length; i++)
                    {
                        Console.Write("=");
                    }
                    Console.WriteLine();

                    for (int i = 0; i < options.Length; i ++) //Print options
                    {
                        Console.WriteLine($"{i + 1} - {options[i]}"); //$ is equal to {0}, {1} etc. streamlines dev.  writes lines for options indexed. 
                    }

                    if(error != "") 
                    {
                        Console.WriteLine(error); //Writes error
                    }

                    Console.Write("Selection: ");
                    response = Console.ReadLine();
                    Console.Clear();

                    if(int.TryParse(response, out responseInt))//Parse string to int vaule chekcs if they are #'s if true stores value  
                    {
                        if(responseInt > 0 && responseInt <= options.Length)//&& operator is AND. responseInt lesst than 0 and respnseInt greater than or equal to options lenght 
                        {
                            invalidResponse = false; //Changes bool value that was set to true 
                        }
                        else
                        {
                            error = "Please enter a number between 1 and " + options.Length;
                        }
                    }
                    else
                    {
                        error = "Please enter a number.";
                    }
                }
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
            return responseInt;
        }

        /**
        * Gets an optional input. if skip is true, message will state "press Eneter to skip" rather than "cancel".
        */
        public static string getOptionalInput(string message, bool skip)
        {
            string response = "";
            string messageEnd = "";

            try
            {
                if(skip)
                {
                    messageEnd = "skip"; //Adds one of these dependant to the CW 

                }
                else
                {
                    messageEnd = "cancel"; //Adds one of these dependant to the CW 

                }

                Console.Write(message + "(press enter to " + messageEnd + ".)");
                response = Console.ReadLine();
                Console.Clear();
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
            return response;
        }
        
        /**
        * Gets an optional input. if skip is true, message will state "press Eneter to skip" rather than "cancel".
        */
        public static string getOptionalInput(string message, bool skip, Action printMessage) //Overloaded with printmessage, option with and without a printmessage 
        {
            string response = "";
            string messageEnd = "";

            try
            {
                if (skip)
                {
                    messageEnd = "skip";
                }
                else
                {
                    messageEnd = "cancel";
                }

                printMessage();
                Console.Write(message + "(press enter to " + messageEnd + ".)");
                response = Console.ReadLine();
                Console.Clear();
            }
            catch (Exception e)
            {
                errorHandler(e);
            }
            return response;
        }

        /**
        * Requests a date input from a user.
        */
        public static DateTime getDateTimeInput()
        {
            int day, month, year, hour, minute;
            DateTime dateTime;

            try
            {
                while(!int.TryParse(getRequiredInput("Day: "), out day)) ; 
                while (!int.TryParse(getRequiredInput("Month: "), out month)); 
                while (!int.TryParse(getRequiredInput("Year: "), out year));
                while (!int.TryParse(getRequiredInput("Hour: "), out hour));
                while (!int.TryParse(getRequiredInput("Minute: "), out minute));
                dateTime = new DateTime(year, month, day, hour, minute, 0);
            }
            catch (Exception e)
            {
                errorHandler(e);
                dateTime = new DateTime();
            }
            return dateTime;
        }
    }
}
