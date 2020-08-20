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
    class Administrator : User //inheritance from the user 
    {
        public Administrator(string username, string password, string name): base(username, password, name) //Call the parent classes contructor when making a new instance 
        {

        }

        /**
        * Shows the Main Menu for administrators
        */
        public override void showMainMenu()
        {
            string response;
            int responseInt;

            try
            {
                loggedIn = true;
                Console.Clear();
                printWelcomeMessage(); //Shows from the user.cs 

                do
                {
                    //Printing of options
                    Console.WriteLine("1 - Practices");
                    Console.WriteLine("2 - Treatment Rooms");
                    Console.WriteLine("3 - Users");
                    Console.WriteLine("4 - Logout");

                    response = Console.ReadLine();
                    Console.Clear();

                    if (int.TryParse(response, out responseInt)) //Parse string  and return the value to resonseInt 
                    {
                        if ((responseInt >= 1) && (responseInt <= 4)) //Checks number is in range.
                        {
                            handleSelection(responseInt);
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
                switch (selection)
                {
                    case 1:
                        showPracticeMenu();
                        break;
                    case 2:
                        showTreatmentRoomMenu();
                        break;
                    case 3:
                        showUserMenu();
                        break;
                    case 4:
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

        #region Practice Menu

        /**
        * Shows the practice menu.
        */
        private void showPracticeMenu()
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
                    Console.WriteLine("2 - Staff Management");
                    Console.WriteLine("3 - Remove");
                    Console.WriteLine("4 - Main Menu");

                    response = Console.ReadLine();
                    Console.Clear();

                    if (int.TryParse(response, out responseInt)) //Parse string  and return the value to resonseInt 
                    {
                        if ((responseInt >= 1) && (responseInt <= 4)) //Checks number is in range.
                        {
                            switch(responseInt)
                            {
                                case 1:
                                    showPracticeEditor();
                                    break;
                                case 2:
                                    showStaffMgr();
                                    break;
                                case 3:
                                    removePractice();
                                    break;
                                case 4:
                                    inMenu = false;
                                    break;
                                default:
                                    break;
                            }
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
                while (inMenu);
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Shows the staff manager allowing admins to assign staff to locations.
        */
        private void showStaffMgr()
        {
            string response;
            bool inMenu;

            try
            {
                inMenu = true;
                do
                {
                    DataPrinting.printPractices(); //Find practices and show them 
                    Console.Write("Location to manage: (enter empty location to exit)");
                    response = Console.ReadLine();
                    Console.Clear();

                    if(response == "")
                    {
                        inMenu = false;
                    }
                    else if(DataSearching.practiceExists(response)) //Checks if practice exsists with given practice  
                    {
                        managePractice(DataSearching.findPractice(response));  //finds practice based on given response 
                    }
                    else
                    {
                        Console.WriteLine("Location does not exist.");
                    }
                }
                while (inMenu);
                    
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Manages a practice
        */
        private void managePractice(DentalPractice practice)
        {
            bool inMenu;

            string receptionist = "";
            Receptionist recObj;

            try
            {
                inMenu = true;

                do
                {
                    DataPrinting.printReceptionists();//Prints out all receptionists 
                    Console.Write($"Receptionist for {practice.getLocation()}: "); //Request receptionist based on location. 
                    receptionist = Console.ReadLine();
                    Console.Clear();

                    if(receptionist == "")
                    {
                        inMenu = false;
                    }
                    else if (DataSearching.userExists(receptionist)) //Checks receptionist exsist
                    {
                        recObj = (Receptionist) DataSearching.findUser(receptionist); //Finds receptionist based on previous check. 
                        recObj.setPractice(practice); //Sets Practice to recpetionist 
                        practice.setReceptionist(recObj); // sets receptionist on the practice 

                        DataIO.saveUser(recObj); //saves file 
                        DataIO.savePracticeFile(practice); //saving file

                        inMenu = false;
                    }
                    else
                    {
                        Console.WriteLine("User does not exist");
                    }
                }
                while (inMenu);
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Shows the practice editor.
        */
        private void showPracticeEditor()
        {
            DentalPractice practiceToAdd;
            string location = "";
            string address = "";
            
            string receptionist;

            try
            {
                while(location == "") //Ensures a value is provided
                {
                    Console.Write("Location: ");
                    location = Console.ReadLine();
                    Console.Clear();
                }

                while(address == "")
                {
                    Console.Write("Address: ");
                    address = Console.ReadLine();
                    Console.Clear();
                }

                Console.Write("Receptionist: (leave blank to skip)");
                receptionist = Console.ReadLine();

                practiceToAdd = new DentalPractice(location, address, receptionist); //Creates practice with or without receptionist 
                if(receptionist != "") //check if receptionist is blank
                {
                    practiceToAdd.associateReceptionist(); //Associates the receptionist with practice 
                }
                Application.appInstance.dentalPractices.Add(practiceToAdd); //Adds dental practice to the list 
                DataIO.savePracticeFile(practiceToAdd);//Saves to file 

            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }

        }

        /**
        * Removes a practice.
        */
        private void removePractice()
        {
            List<DentalPractice> practices = Application.appInstance.dentalPractices;
            List<TreatmentRoom> associatedRooms;
            string response = "";
            DentalPractice practiceToRemove;
            bool inMenu = true;

            try
            {
                do
                {
                    Console.Write("Practice to remove (leave blank to cancel): ");
                    response = Console.ReadLine();
                    Console.Clear();

                    inMenu = (response != "");

                    if(inMenu)
                    {
                        if(practices.Exists(practice => practice.getLocation() == response)) //Checks to see the given practice exists.
                        {
                            practiceToRemove = DataSearching.findPractice(response); //Finds practice based on response 
                            associatedRooms = practiceToRemove.getRooms(); 

                            for(int i = 0; i < associatedRooms.Count; i ++)//Iterates over every room 
                            {
                                deleteRoom(associatedRooms[i], false); //Deletes all rooms
                            }

                            practices.Remove(practiceToRemove); //Removes pratice from the list 
                        }
                        else
                        {
                            Console.WriteLine("Practice not found.");
                        }
                    }
                }
                while (inMenu);
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }


        #endregion

        #region Treatment Rooms

        /**
        * Shows the treatment room menu
        */
        private void showTreatmentRoomMenu()
        {
            string response;
            int responseInt;
            bool inMenu;
            try
            {
                inMenu = true;
                Console.Clear();
                printWelcomeMessage();

                do
                {
                    //Printing of options
                    Console.WriteLine("1 - Add");
                    Console.WriteLine("2 - Room Management");
                    Console.WriteLine("3 - Remove");
                    Console.WriteLine("4 - Main Menu");

                    response = Console.ReadLine();
                    Console.Clear();

                    if (int.TryParse(response, out responseInt)) //Converts string to int value and stores it 
                    {
                        if ((responseInt >= 1) && (responseInt <= 4)) //Checks number is in range.
                        {
                            switch (responseInt)
                            {
                                case 1:
                                    showRoomEditor();
                                    break;
                                case 2:
                                    showRoomMgr();
                                    break;
                                case 3:
                                    removeRoom();
                                    break;
                                case 4:
                                    inMenu = false;
                                    break;
                                default:
                                    break;
                            }
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
                while (inMenu);
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Shows the room editor.
        */
        private void showRoomEditor()
        {
            bool inMenu;
            string response = "";
            DentalPractice practiceToAddRoom;

            try
            {
                inMenu = true;

                while (inMenu)
                {
                    DataPrinting.printPractices();
                    Console.WriteLine("Practice to add room to: (Leave blank to cancel)");
                    response = Console.ReadLine();
                    Console.Clear();

                    if(response != "")
                    {
                        if(DataSearching.practiceExists(response)) //uses response to find the practices that are a valid response 
                        {
                            practiceToAddRoom = DataSearching.findPractice(response);// takes response and gets pratice with the given response and location 
                            addRoomToPractice(practiceToAddRoom); //run method to add practice 
                        }
                        else
                        {
                            Console.WriteLine("Practice does not exist.");
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
        * Adds a room to the provided practice.
        */
        private void addRoomToPractice(DentalPractice practice)
        {
            TreatmentRoom newTreatmentRoom;
            int roomNo;

            try
            {
                roomNo = practice.getRooms().Count + 1; //Adds +1 to current room no. 

                newTreatmentRoom = new TreatmentRoom(roomNo, practice, "", ""); //"", "" are for the dentist & nurse 
                manageRoom(newTreatmentRoom);//Runs manageRoom to set up new treatment room
                practice.getRooms().Add(newTreatmentRoom); //Adds the treatment room to practice. 
                DataIO.saveAllRooms(practice); //writes to file 
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Shows the room managing interface.
        */
        private void showRoomMgr()
        {
            string response;
            int responseInt;
            bool inMenu;

            DentalPractice practice;
            List<TreatmentRoom> rooms;

            try
            {
                inMenu = true;
                bool invalidPractice;
                bool invalidRoom;

                while (inMenu)
                {
                    invalidPractice = true;
                    while (invalidPractice && inMenu) //Selecting a location and room. 
                    {
                        DataPrinting.printPractices(); //Show practices 
                        Console.Write("Practice: (leave blank to cancel)");
                        response = Console.ReadLine();
                        Console.Clear();

                        if(response == "")
                        {
                            inMenu = false; //if blank, allows loop to break.
                        }
                        else if(DataSearching.practiceExists(response)) //Checks if it is a valid response 
                        {
                            invalidPractice = false;
                            invalidRoom = true;

                            practice = DataSearching.findPractice(response); //Gets practice with given ID
                            rooms = practice.getRooms(); //Gets list of rooms 

                            while (invalidRoom && inMenu) //Selecting a room
                            {
                                DataPrinting.printRoomsAtPractice(practice); //Prints rooms based on practice location
                                Console.WriteLine("Room to manage: (leave blank to cancel)");
                                response = Console.ReadLine();

                                if(response == "")
                                {
                                    invalidRoom = false;
                                }
                                if (int.TryParse(response, out responseInt))
                                {
                                    if(responseInt > 0 && responseInt <= rooms.Count)
                                    {
                                        manageRoom(rooms[responseInt - 1]); //Brings up interface for managing a room. indexing rooms list -1. 
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Location not found");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Provides an interface allowing admin to Remove a room.
        */
        private void removeRoom()
        {
            string response;
            int responseInt;
            bool inMenu;
            bool invalidResponse;
            DentalPractice practiceToRemoveFrom;
            TreatmentRoom roomToRemove;

            try
            {
                inMenu = true;

                while (inMenu)
                {
                    DataPrinting.printPractices();
                    Console.Write("Practice to remove room from: (leave blank to cancel)");
                    response = Console.ReadLine();
                    Console.Clear();

                    if(response != "")
                    {
                        if(DataSearching.practiceExists(response)) //Checks for valid user response 
                        {
                            practiceToRemoveFrom = DataSearching.findPractice(response); //Gets practice with given ID 
                            invalidResponse = true;

                            while(inMenu && invalidResponse)
                            {
                                DataPrinting.printRoomsAtPractice(practiceToRemoveFrom); //Shows all rooms in the given practice 
                                Console.Write("Select room to remove: (leave blank to cancel)");
                                response = Console.ReadLine();
                                Console.Clear();

                                if (response != "")
                                {
                                    if (int.TryParse(response, out responseInt))//Converts value to int as responseInt 
                                    {
                                        if (responseInt > 0 && responseInt <= practiceToRemoveFrom.getRooms().Count) //Checking if response is greater than 0 or less than or equal the given amount of rooms. 
                                        {
                                            roomToRemove = practiceToRemoveFrom.getRooms()[responseInt - 1]; 
                                            deleteRoom(roomToRemove, true); //Removes the room 
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid room");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Enter a number.");
                                    }
                                }
                                else
                                {
                                    invalidResponse = false;//Breaks loop
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Practice not found.");
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
        * Shows the interface to manage a room.
        */
        private void manageRoom(TreatmentRoom room)
        {
            string response;
            bool invalidResponse;
            Staff userToCheck;
            DentistNurse userToAssign;

            try
            {
                invalidResponse = true;

                while (invalidResponse) // Prints all staff at practice. 
                {
                    DataPrinting.printStaffAtPractice(room.getPractice());
                    Console.Write("Dentist to assign: (Enter nothing to skip)");
                    response = Console.ReadLine();
                    Console.Clear();

                    if(response != "")
                    {
                        if (DataSearching.userExists(response)) //Validates the response 
                        {
                            userToCheck = (Staff) DataSearching.findUser(response); //Find staff users with the given username response. 
                            if (userToCheck is DentistNurse)
                            {
                                userToAssign = (DentistNurse) DataSearching.findUser(response); //Checks wether they are dentist or nurse and find them usign findUser method 

                                if(userToAssign.getPractitionerType() != "Dentist") //if not equal to dentist
                                {
                                    Console.WriteLine("Selected staff is not dentist.");
                                }
                                else
                                {
                                    room.setDentist(userToAssign); // sets the given room to the dentist user. 
                                    invalidResponse = false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Selected staff is not dentist.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("user does not exist.");
                        }
                    }
                    else
                    {
                        invalidResponse = false;
                    }
                }

                invalidResponse = true;

                while (invalidResponse) // Selecting the Nurse. works the same as Dentist 
                {
                    DataPrinting.printStaffAtPractice(room.getPractice());
                    Console.Write("Nurse to assign: (Enter nothing to skip)");
                    response = Console.ReadLine();
                    Console.Clear();

                    if (response != "")
                    {
                        if (DataSearching.userExists(response))
                        {
                            userToCheck = (Staff)DataSearching.findUser(response);
                            if (userToCheck is DentistNurse)
                            {
                                userToAssign = (DentistNurse)DataSearching.findUser(response);

                                if (userToAssign.getPractitionerType() != "Nurse")
                                {
                                    Console.WriteLine("Selected staff is not nurse.");
                                }
                                else
                                {
                                    room.setNurse(userToAssign);
                                    invalidResponse = false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Selected staff is not nurse.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("user does not exist.");
                        }
                    }
                    else
                    {
                        invalidResponse = false;
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Deletes a treatment room.
        */
        private void deleteRoom(TreatmentRoom room, bool singleRoom)
        {
            DentalPractice parentPractice;
            List<TreatmentRoom> rooms;
            string roomPath;

            try
            {
                parentPractice = room.getPractice();//practice that room belongs to 
                roomPath = DataIO.practicePath + "\\" + parentPractice.getLocation() + "\\Rooms\\" + room.getRoomNumber() + ".txt";//Find the file path to the room, to be deleted 
                File.Delete(roomPath); //Deletes the file 
                rooms = parentPractice.getRooms(); //Gets practice associated to room  
                rooms.Remove(room); //Remove the room from the practice.  
                if (singleRoom)
                {
                    restructureRooms(parentPractice); //No point running this when all rooms are being removed but just in case.
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Reorgasnises the rooms. Usually occurs following the removal of a room.
        */
        private void restructureRooms(DentalPractice practice)
        {
            List<TreatmentRoom> rooms = practice.getRooms();
            try
            {
                for(int i = 0; i < rooms.Count; i ++)
                {
                    rooms[i].setRoomNumber(i + 1); //Sets the room number, and converts indext to a more human friendly number (room 1 = 1 instead 0)  
                }
                DataIO.saveAllRooms(practice); //Saves all rooms to files
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }


        #endregion

        #region users

        /**
        * Shows the user management menu.
        */
        private void showUserMenu()
        {

            string response;
            int responseInt;
            bool inMenu;

            try
            {
                inMenu = true;
                while (inMenu)
                {
                    Console.Clear();
                    //Printing of options
                    Console.WriteLine("1 - Add user");
                    Console.WriteLine("2 - Delete user");
                    Console.WriteLine("3 - Main Menu");

                    response = Console.ReadLine();
                    Console.Clear();

                    if (int.TryParse(response, out responseInt)) 
                    {
                        switch (responseInt)
                        {
                            case 1:
                                addUser();
                                break;
                            case 2:
                                removeUser();
                                break;
                            case 3:
                                inMenu = false;
                                break;
                            default:
                                Console.WriteLine("Enter a valid number.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Enter a number.");
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Removes a user from the system.
        */
        private void removeUser()
        {
            List<User> users = Application.appInstance.users;
            string response = "";
            bool inMenu = true;
            User userToRemove;

            try
            {
                while (inMenu)
                {
                    response = GeneralFunctions.getOptionalInput("User: ", false, DataPrinting.printUsers); //requests user to be removed, while printing list of users. 

                    if (response != "")
                    {
                        if(DataSearching.userExists(response))//Checks if user exists with given response. 
                        {
                            userToRemove = DataSearching.findUser(response); //Find users matching with response 
                            DataIO.deleteUserFile(userToRemove); //Delete the user in the file 
                            users.Remove(userToRemove); //Remove the user from the list 
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
        * Adds a user to the system.
        */
        private void addUser()
        {
            string username;
            string password;
            string name;
            int role;
            string location = "";
            User userToAdd;
            List<User> users = Application.appInstance.users;
            DentalPractice practice;

            try
            {
                //Input to create user
                username = GeneralFunctions.getRequiredInput("Username: ");
                password = GeneralFunctions.getRequiredInput("Password: ");
                name = GeneralFunctions.getRequiredInput("Name: ");
                role = GeneralFunctions.getRequiredSelection("Role: ", new string[]{"Administrator", "Receptionist", "Dentist", "Nurse"}); //Passes a string to label the input and a string array of available options.
                
                if(role != 1) // if not administrator.
                {
                    location = getValidLocation();
                }

                switch(role)
                {
                    case 1:
                        userToAdd = new Administrator(username, password, name);
                        users.Add(userToAdd); //Adds to the Users 
                        DataIO.saveUser(userToAdd); //Saves to files 
                        break;
                    case 2:
                        userToAdd = new Receptionist(username, password, name, location);
                        users.Add(userToAdd);
                        DataIO.saveUser(userToAdd);

                        if (location != "")
                        {
                            practice = DataSearching.findPractice(location); //Uses location string to find practice  
                            practice.setReceptionist((Receptionist) userToAdd); //Sets the receptionist at the practice to the new user. 
                        }
                        break;
                    case 3:
                        userToAdd = new DentistNurse(username, password, name, location, "Dentist"); //Creates a new user and sets practioner type to Dentist 
                        users.Add(userToAdd);
                        DataIO.saveUser(userToAdd);
                        break;
                    case 4:
                        userToAdd = new DentistNurse(username, password, name, location, "Nurse"); //Creates a new user and sets practioner type to Nurse 
                        users.Add(userToAdd);
                        DataIO.saveUser(userToAdd);
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

        /**
        * gets a valid practice.
        */
        private string getValidLocation()
        {
            bool validLocation = false;
            string location = "";

            do
            {
                location = GeneralFunctions.getOptionalInput("Location: ", true, DataPrinting.printPractices); //requests a location from a user, while printing the list of practices out. 

                if (location != "") //If location is not null 
                {
                    if (DataSearching.practiceExists(location)) //Check the pratice provided exsists. 
                    {
                        validLocation = true; //Changes bool value, marks given location is valid. 
                    }
                    else
                    {
                        Console.WriteLine("Invalid Location");
                    }
                }
                else
                {
                    validLocation = true;
                }

            }
            while (!validLocation); //While valid location is false, run loop. 

            return location;
        }
        


        #endregion
    }
}
