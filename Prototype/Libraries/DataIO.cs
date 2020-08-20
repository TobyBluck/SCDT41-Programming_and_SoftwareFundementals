using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MyDentistMgr.UserObjects;
using MyDentistMgr.DataObjects;

namespace MyDentistMgr.Libraries
{
    static class DataIO //static classes cannot be instantiated or extended
    {
        public static string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +  @"\" + MyDentistMgr.Default.DataPath; // Gets the path to My Documents. I used this so that the system could be transferred across Computers without setup. https://docs.microsoft.com/en-us/dotnet/api/system.environment.specialfolder?view=netcore-3.1 This line combines this and the DataPath defined in the MyDentistMgr.settings file.
        public static string userPath = rootPath +  @"\" + MyDentistMgr.Default.UserPath; //creates a path to the User directory. Putting @ before a string literal causes \ to not mark an escape character. (so \n would print "\n" rather than print a line break)
        public static string practicePath = rootPath + @"\" + MyDentistMgr.Default.DentalPracticePath; //creates a path to the dental practice directory. //@ ignore escape keys so / would be backslash. 
        public static string patientPath = rootPath +  @"\" + MyDentistMgr.Default.PatientsPath; //creates a path to the patients directory. 

        /**
        * Saves a singe practice file.
        */
        public static void savePracticeFile(DentalPractice practice)
        {
            StreamWriter practiceFile; //writes a textfile to the computer, Reader reads 
            string practiceFilePath;

            try
            {
                practiceFilePath = practicePath + "\\" + practice.getLocation(); // \\ escape key for backslash, takes root directory and add the backslash to determine the path in the dental practice folder. 
                if(!Directory.Exists(practiceFilePath)) //Creates a folder for the practice if it does not exist already.
                {
                    Directory.CreateDirectory(practiceFilePath);
                    Directory.CreateDirectory(practiceFilePath + @"\Rooms");
                }

                practiceFile = new StreamWriter(practiceFilePath + @"\info.txt", false); //Give true as the second parameter to append to a file or false to overwrite.
                practiceFile.WriteLine(practice.getLocation()); //First line is location.

                if (practice.getReceptionist() is Receptionist) //Checks if recepotionist exsists. 
                {
                    practiceFile.WriteLine(practice.getReceptionist().getUsername()); //Receptionist is second line.
                }
                else
                {
                    practiceFile.WriteLine();
                }

                practiceFile.WriteLine(practice.getAddress()); //Address is third.
                practiceFile.Close(); //closes streamwriter 
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Loads the Dental Practices into the system.
        */
        public static void loadPractices()
        {
            List<DentalPractice> practices = Application.appInstance.dentalPractices; //Creates a reference to the list of dental practcies. 
            StreamReader practiceInfoFile; //read practice file 
            string[] practiceDirectories; //string array 
            
            string location;
            string receptionistId;
            string address;

            try
            {
                if (!Directory.Exists(practicePath)) Directory.CreateDirectory(practicePath); //find whether it exsists if not it creates them. ! is not used to create one if it is not found 
                practiceDirectories = Directory.GetDirectories(practicePath); //Gets the list of driectories in practicePath.

                for (int i = 0; i < practiceDirectories.Length; i ++)  //iterating over every folder/ over datafolder and look through the info.txt and create the rooms folder if non exsitent 
                {
                    practiceInfoFile = new StreamReader(practiceDirectories[i] + "\\info.txt"); 
                    location = practiceInfoFile.ReadLine(); //First line will be the location.
                    receptionistId = practiceInfoFile.ReadLine(); //second line will be the receptionist.
                    address = practiceInfoFile.ReadLine();  //third line will be the address.
                    practiceInfoFile.Close(); //closes the file, error will be caused if files are not closed. 

                    practices.Add(new DentalPractice(location, address, receptionistId)); 
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * loads treatment rooms for a given location.
        */
        public static void loadTreatmentRooms(List<TreatmentRoom> rooms, DentalPractice practice)
        {
            string[] treatmentRoomFiles;
            StreamReader treatmentRoomFile; //opens file to read
            string roomPath = practicePath + "\\" + practice.getLocation() + "\\Rooms"; //takes root directory and add the backslash to determine the path in the Rooms folder within locations folder. 

            string assignedDentist;
            string assignedNurse;

            try
            {
                if (Directory.Exists(roomPath))//find whether it exsists 
                {
                    treatmentRoomFiles = Directory.GetFiles(roomPath); //Get files for stored in the rooms folder 

                    for (int i = 0; i < treatmentRoomFiles.Length; i++)//Iterates over all the room files 
                    {
                        treatmentRoomFile = new StreamReader(roomPath + "\\Room " + (i + 1) + ".txt"); //Opens up the first room file for the given location
                        assignedDentist = treatmentRoomFile.ReadLine(); //first line will be the dentist.
                        assignedNurse = treatmentRoomFile.ReadLine(); //second will be the nurse.
                        treatmentRoomFile.Close();

                        rooms.Add(new TreatmentRoom(i + 1, practice, assignedDentist, assignedNurse));
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Saves all treatment rooms.
        */
        public static void saveAllRooms(DentalPractice practice)
        {
            List<TreatmentRoom> rooms;
            StreamWriter roomFile;
            string roomInfoPath; //Path to the specific Practice Rooms folder.
            string roomFilePath;

            try
            {
                rooms = practice.getRooms(); //Gets lsit of rooms from give practice.
                roomInfoPath = practicePath + "\\" + practice.getLocation() + "\\Rooms"; //Determines the path of the directory where the given locations room files are stored. 
                if (Directory.Exists(roomInfoPath))
                {
                    Directory.Delete(roomInfoPath, true); //True tells Directory.Delete() to delete any contents within the folder.
                }
                Directory.CreateDirectory(roomInfoPath); //Recreates the empty Room directory.

                roomInfoPath += "\\Room "; //Concatenates string, joins strings together. 

                for (int i = 0; i < rooms.Count; i ++) //tracks how many items are in the list. 
                {
                    roomFilePath = roomInfoPath + (i + 1) + ".txt";
                    roomFile = new StreamWriter(roomFilePath);
                    roomFile.WriteLine(rooms[i].getDentistId());
                    roomFile.WriteLine(rooms[i].getNurseId());
                    roomFile.Close();
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Creates a default admin file when no users exist.
        */
        private static void generateAdminFile()
        {
            Administrator admin = new Administrator("Admin", "admin12345", "Administrator");
            Application.appInstance.users.Add(admin);
            saveUser(admin);
        }

        /**
        * Saves a user file.
        */
        public static void saveUser(User userToAdd)
        {
            StreamWriter userFile;
            DentistNurse denNurseToAdd;
            Receptionist recToAdd;

            string userFilePath;

            try
            {
                userFilePath = userPath + "\\" + userToAdd.getUsername() + ".txt"; //Determines the path where the user will be saved.

                userFile = new StreamWriter(userFilePath);
                userFile.WriteLine(userToAdd.getUsername()); 
                userFile.WriteLine(userToAdd.getName());

                if(userToAdd is Staff) //Check if they are Staff
                {
                    if (userToAdd is Receptionist) //Checks whether is a receptionist or not 
                    {
                        recToAdd = (Receptionist) userToAdd; 
                        userFile.WriteLine("Receptionist"); //Writes recepetionist to file
                        userFile.WriteLine(recToAdd.getPassword());
                        if (recToAdd.getPractice() is DentalPractice)  //Check if practice exsists 
                        {
                            userFile.WriteLine(recToAdd.getPractice().getLocation()); //Can assign a location to a receptionist, must check if exists 
                        }
                        else
                        {
                            userFile.WriteLine();
                        }
                    }
                    else
                    {
                        denNurseToAdd = (DentistNurse) userToAdd; //Same as receptionist. 
                        userFile.WriteLine("DentistNurse");
                        userFile.WriteLine(denNurseToAdd.getPassword());
                        if (denNurseToAdd.getPractice() is DentalPractice) //Check if practice exsists
                        {
                            userFile.WriteLine(denNurseToAdd.getPractice().getLocation()); //Can assign a location to a Dentist/nurse,
                        }
                        else
                        {
                            userFile.WriteLine();//If no practice is exsitent. 
                        }
                        userFile.WriteLine(denNurseToAdd.getPractitionerType()); 
                    }
                }
                else
                {
                    userFile.WriteLine("Administrator");
                    userFile.WriteLine(userToAdd.getPassword());
                }
                userFile.Close();
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /*
        * Loads users into the system.
        */
        public static void loadUsers()
        {
            StreamReader userFile;
            User newUser;
            List<User> users = Application.appInstance.users;
            string[] userFiles;
            string username;
            string userPassword;
            string userType;
            string userPractice;
            string practitionerType;
            string name;
            bool success;

            try
            {
                initialiseUserDirectory();
                userFiles = Directory.GetFiles(userPath); //Gets the name of every file in the user directory.

                if (userFiles.Length == 0) generateAdminFile(); //lenght of user is 0, make default admin

                for(int i = 0; i < userFiles.Length; i ++) //iterates over every file in the user directory.
                {
                    userFile = new StreamReader(userFiles[i]);
                    username = userFile.ReadLine(); //first line will be username.
                    name = userFile.ReadLine(); //Second line of the user file will be their name.
                    userType = userFile.ReadLine(); //Third line of the user file is the type.
                    userPassword = userFile.ReadLine(); //Fourth line of the user file is the password.

                    success = true;

                    switch(userType) //switch case for the type of user created 
                    {
                        case "Administrator":
                            newUser = new Administrator(username, userPassword, name); //Calls to create a new Administrator. Passing user data to Admin. 
                            break; //break case
                        case "Receptionist":
                            userPractice = userFile.ReadLine(); //Practice, if applicable, will be fifth line.
                            newUser = new Receptionist(username, userPassword, name, userPractice); //Receptionist extends Staff which extends User meaning Receptionist is also storable as User.
                            break;
                        case "DentistNurse":
                            userPractice = userFile.ReadLine(); //Practice, if applicable, will be fifth line.
                            practitionerType = userFile.ReadLine();//Practitioner type, if applicable, will be 6th line.
                            newUser = new DentistNurse(username, userPassword, name, userPractice, practitionerType); //Calls to create a new DentistNurse, which extend Staff which extend User 
                            break;
                        default:
                            newUser = new DentistNurse("", "", "", "", ""); //This is to prevent any error when attempting to add a potentially initialized/added User variable. 
                            Console.WriteLine("Invalid User File: " + userFiles[i]);
                            success = false;
                            break;
                    }

                    userFile.Close(); //closes the file. 

                    if (success) // if the file has a valid user type, user will be added.
                    {
                        users.Add(newUser);
                    }
                }

            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * deletes a user file
        */
        public static void deleteUserFile(User user) //targets user files 
        {
            string userFilePath;

            try
            {
                userFilePath = userPath + user.getUsername() + ".txt"; //Gets the file path for the User. 

                if (File.Exists(userFilePath)) File.Delete(userFilePath); //finds and deletes file 
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Associates staff with their practices. This needs to be done now as the Users, Rooms and Practices all needed to be loaded in before association can take place.
        */
        public static void associateStaffWithPractices() //Associcates staff with practices
        {
            List<DentalPractice> practices = Application.appInstance.dentalPractices;
            
            try
            {
                for(int i = 0; i < practices.Count; i ++)  
                {
                    practices[i].associateReceptionist(); //Links receptionist with the practice 
                    associateStaffWithRooms(practices[i].getRooms()); //Associate staff with room, takes rooms from practice. 
                }
            }
            catch(Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Associates staff with rooms.
        */
        private static void associateStaffWithRooms(List<TreatmentRoom> rooms) //This fucntions the same as associateStaffWithPractices()
        {
            try
            {
                for(int i = 0; i < rooms.Count; i ++)
                {
                    rooms[i].associateStaff();
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        #region patient IO

        /**
        * Loads patients from file.
        */
        public static void loadPatients()
        {
            DentalPractice practiceToAddTo;
            StreamReader patientFile;
            string[] patientDirectories;
            Patient patientToAdd;

            string id;
            string name;
            string location;
            int gender;
            string address;
            string contactNo;

            try
            {
                if (!Directory.Exists(patientPath)) //if exists do nothing if not create the directory 
                {
                    Directory.CreateDirectory(patientPath);
                }

                patientDirectories = Directory.GetDirectories(patientPath);

                for(int i = 0; i < patientDirectories.Length; i ++)
                {
                    patientFile = new StreamReader(patientDirectories[i] + "\\info.txt");
                    id = patientFile.ReadLine(); //First line of patient file will be ID.
                    name = patientFile.ReadLine(); //Second will be name.
                    location = patientFile.ReadLine(); //third will be name of practice.
                    int.TryParse(patientFile.ReadLine(), out gender); //fourth will be gender. this needs converting from string to int.
                    address = patientFile.ReadLine(); //fifth will be address.
                    contactNo = patientFile.ReadLine(); //Sixth will be contact number.
                    patientFile.Close();

                    if(DataSearching.practiceExists(location)) //Should always be the case. just a little extra error trapping.
                    {
                        practiceToAddTo = DataSearching.findPractice(location); //References the DentalPractice with the given location.
                        patientToAdd = new Patient(id, name, practiceToAddTo, gender, address, contactNo); //Creates a Patient object to hold information about the new patient.
                        practiceToAddTo.getPatients().Add(patientToAdd); //Adds the patient to the practice.
                        loadAppointments(patientToAdd); //loads the patients appointments.
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Saves a patient.
        */
        public static void savePatient(Patient patient)
        {
            StreamWriter patientFile;

            try
            {
                if (!Directory.Exists(patientPath + "\\" + patient.getId())) Directory.CreateDirectory(patientPath + "\\" + patient.getId()); //find or creates directory 
                patientFile = new StreamWriter(patientPath + "\\" + patient.getId() + "\\info.txt"); //writes everything below to this file 
                patientFile.WriteLine(patient.getId());
                patientFile.WriteLine(patient.getName());
                patientFile.WriteLine(patient.getPractice().getLocation());
                patientFile.WriteLine(patient.getGender());
                patientFile.WriteLine(patient.getAddress());
                patientFile.WriteLine(patient.getContactNo());
                patientFile.Close();
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        #endregion

        #region appointment and Notes IO

        /**
        * Loads appointments for a given patient.
        */
        public static void loadAppointments(Patient patient)
        {
            string apptmtPath = $"{patientPath}\\{patient.getId()}\\Appointments"; //streamline calling using $ instead of {0} to insert parameter 
            string[] appointmentDirectories;
            StreamReader apptmtFile; //read file 
            string apptmtId;
            int roomInt;
            TreatmentRoom room;
            string date;
            string time;

            Appointment apptToAdd;

            try
            {
                if (!Directory.Exists(apptmtPath)) Directory.CreateDirectory(apptmtPath); //finds if not creates 

                appointmentDirectories = Directory.GetDirectories(apptmtPath);

                for (int i = 0; i < appointmentDirectories.Length; i ++)
                {
                    apptmtFile = new StreamReader(appointmentDirectories[i] + "\\info.txt");

                    apptmtId = apptmtFile.ReadLine(); //First line will be appointment Id
                    int.TryParse(apptmtFile.ReadLine(), out roomInt); //Second will be room number.
                    room = patient.getPractice().getRooms()[roomInt - 1]; //Gets the room via the patients practice.
                    date = apptmtFile.ReadLine(); //third line will be date.
                    time = apptmtFile.ReadLine(); // final line will be time.
                    apptmtFile.Close();

                    apptToAdd = new Appointment(apptmtId, patient, room, date, time);
                    patient.getAppointments().Add(apptToAdd);
                    loadAppointmentNotes(apptToAdd);
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Loads notes for an appointment.
        */
        private static void loadAppointmentNotes(Appointment appt)
        {
            string notesPath = $"{patientPath}\\{appt.getPatient().getId()}\\Appointments\\{appt.getId()}\\Notes"; //Gets the file path to the folder containing the given appointments notes.
            StreamReader apptmtNoteFile;
            string[] noteFiles;

            string username;
            DentistNurse author;
            string date;
            string time;
            string content;

            try
            {
                if (!Directory.Exists(notesPath)) Directory.CreateDirectory(notesPath); //finds if not creates 

                noteFiles = Directory.GetFiles(notesPath);

                for (int i = 0; i < noteFiles.Length; i++)
                {
                    apptmtNoteFile = new StreamReader(noteFiles[i]); //Opens a note file 

                    username = apptmtNoteFile.ReadLine(); //first line will be the username of the author.

                    if(DataSearching.userExists(username))
                    {
                        author = (DentistNurse) DataSearching.findUser(username);
                    }
                    else
                    {
                        author = new DentistNurse(username, "", "Unknown user", "", "");
                    }

                    date = apptmtNoteFile.ReadLine(); //second line will be the date
                    time = apptmtNoteFile.ReadLine(); //third the time
                    content = apptmtNoteFile.ReadToEnd(); // Content will be the rest of the file. this allows for line breaks (not that the input will).
                    apptmtNoteFile.Close();

                    appt.getNotes().Add(new AppointmentNote(date, time, appt, author, content));
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Saves an appointment.
        */
        public static void saveAppointment(Appointment appt)
        {
            StreamWriter apptFile;
            string apptPath = $"{patientPath}\\{appt.getPatient().getId()}\\Appointments\\{appt.getId()}";

            try
            {
                if (!Directory.Exists(apptPath)) Directory.CreateDirectory(apptPath);

                apptFile = new StreamWriter(apptPath + "\\info.txt"); //writes info to the file 
                apptFile.WriteLine(appt.getId()); 
                apptFile.WriteLine(appt.getRoom().getRoomNumber());
                apptFile.WriteLine(appt.getDate());
                apptFile.WriteLine(appt.getTime());
                apptFile.Close();
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * saves an appointment note.
        */
        public static void saveAppointmentNote(AppointmentNote note)
        {
            string notePath = $"{patientPath}\\{note.appointment.getPatient().getId()}\\Appointments\\{note.appointment.getId()}\\Notes";
            StreamWriter noteFile;

            try
            {
                if (!Directory.Exists(notePath)) Directory.CreateDirectory(notePath);

                noteFile = new StreamWriter($"{notePath}\\{GeneralFunctions.generateId(notePath, "Note")}.txt");

                noteFile.WriteLine(note.author.getUsername()); //first line is username
                noteFile.WriteLine(note.date); //Second the date.
                noteFile.WriteLine(note.time); //third the time.
                noteFile.Write(note.content); //and finally, the content.
                noteFile.Close();

            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }

        }

        #endregion

        /*
        * ========================
        * First-time-run functions
        * ========================
        */


        /*
        * Creates the data directory.
        */
        public static void initialiseDataDirectory()
        {
            try
            {
                if /*condition*/(!Directory.Exists(rootPath)) /*code*/ Directory.CreateDirectory(rootPath); // When creating an if statement and ending the line with ; instead of {}, only execute code on that line. rootPath is myDentistMgr Folder
            }
            catch(Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        

        /*
        * Creates the user directory.
        */
        private static void initialiseUserDirectory()
        {
            try
            {
                initialiseDataDirectory();
                if (!Directory.Exists(userPath))
                {
                    Directory.CreateDirectory(userPath);
                    generateAdminFile(); //creates default admin
                }
            }
            catch(Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }
    }
}