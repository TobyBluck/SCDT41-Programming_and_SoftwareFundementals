using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyDentistMgr.DataObjects;
using MyDentistMgr.UserObjects;

namespace MyDentistMgr.Libraries
{
    static class DataPrinting
    {
        #region lists
        public static void printPractices() 
        {
            List<DentalPractice> practices = Application.appInstance.dentalPractices;

            try
            {
                for(int i = 0; i < practices.Count; i ++) //iteration of the amount of practices  
                {
                    Console.WriteLine(practices[i].getLocation()); //get location and print it out  
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prints rooms in a given practice.
        */
        public static void printRoomsAtPractice(DentalPractice practice)
        {
            List<TreatmentRoom> rooms = practice.getRooms();

            try
            {
                for(int i = 0; i < rooms.Count; i ++) //iterates over room no.  
                {
                    Console.WriteLine("Room: {0}", rooms[i].getRoomNumber()); //Writes rooms
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prints all users.
        */
        public static void printUsers()
        {
            List<User> users = Application.appInstance.users;
            Staff staff;

            try
            {
                for (int i = 0; i < users.Count; i++) 
                {
                    if (users[i] is Staff) //Checks whether user is a Staff member. 
                    {
                        staff = (Staff)users[i]; //Casts the user object to staff object.  
                        if (staff.getPractice() is DentalPractice)
                        {
                            Console.WriteLine("{0}: {1} - {2}", staff.getUsername(), staff.getName(), staff.getPractice().getLocation());// <username>: <name> - <location>
                        }
                        else
                        {
                            Console.WriteLine("{0}: {1}", users[i].getUsername(), users[i].getName());
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0}: {1}", users[i].getUsername(), users[i].getName()); // <username>: <name>
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prints all staff
        */
        public static void printAllStaff()
        {
            List<Staff> staff = DataSearching.getStaff();
            DentistNurse practitioner;

            try
            {
                for(int i = 0; i < staff.Count; i ++)
                {
                    Console.Write($"{staff[i].getUsername()}: {staff[i].getName()} - ");

                    if(staff[i] is Receptionist)
                    {
                        Console.WriteLine("Receptionist");
                    }
                    else
                    {
                        practitioner = (DentistNurse) staff[i];
                        Console.WriteLine(practitioner.getPractitionerType());
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prints staff at a given Practice.
        */ 
        public static void printStaffAtPractice(DentalPractice practice)
        {
            List<Staff> staff = DataSearching.getStaff();
            List<Staff> staffAtPractice = new List<Staff>();
            DentistNurse denNurseToPrint;

            try
            {
                staffAtPractice = staff.FindAll(staffMem => (staffMem.getPractice() is DentalPractice) && (staffMem.getPractice().Equals(practice))); //Returns a list of all staff with the given location.

                Console.WriteLine("Staff at {0}:", practice.getLocation());

                for (int i = 0; i < staffAtPractice.Count; i++)
                {
                    Console.Write("{0}: {1} - ", staffAtPractice[i].getUsername(), staffAtPractice[i].getName());
                    
                    //Printing role
                    if(staffAtPractice[i] is Receptionist) 
                    {
                        Console.WriteLine("Receptionist");
                    }
                    else
                    {
                        denNurseToPrint = (DentistNurse) staffAtPractice[i]; //Shortened version to select practioner type & write it 
                        Console.WriteLine(denNurseToPrint.getPractitionerType());
                    }
;               }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * prints all receptionists
        */
        public static void printReceptionists()
        {
            List<Receptionist> receptionists = DataSearching.getReceptionists();

            try
            {
                for (int i = 0; i < receptionists.Count; i++)
                {
                    if (receptionists[i].getPractice() is DentalPractice)
                    {
                        Console.WriteLine("{0}: {1} - {2}", receptionists[i].getUsername(), receptionists[i].getName(), receptionists[i].getPractice().getLocation()); //if assigned write this  
                    }
                    else
                    {
                        Console.WriteLine("{0}: {1}", receptionists[i].getUsername(), receptionists[i].getName()); //if unassigned this is what is written  
                    }
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prints patients at the given practice.
        */
        public static void printPatients(DentalPractice practice)
        {
            List<Patient> patients = practice.getPatients();

            try
            {
                for(int i = 0; i < patients.Count; i ++)
                {
                    Console.WriteLine($"{patients[i].getId()}: {patients[i].getName()}");
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prints appointments for a given patient.
        */
        public static void printAppointments(Patient patient)
        {
            List<Appointment> appts = patient.getAppointments();

            try
            {
                Console.WriteLine("Appointments for " + patient.getName());

                for(int i = 0; i < appts.Count; i ++)
                {
                    Console.WriteLine($"{appts[i].getId()}: Room {appts[i].getRoom().getRoomNumber()} - {appts[i].getDate()} {appts[i].getTime()}");
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        public static void printPractitionerAppointments(DentistNurse practitioner)
        {
            List<Appointment> appointments = DataSearching.getPractitionerAppointments(practitioner);

            try
            {
                for(int i = 0; i < appointments.Count; i ++)
                {
                    Console.WriteLine($"{appointments[i].getId()} - {appointments[i].getPatient().getName()}: {appointments[i].getDate()} {appointments[i].getTime()}"); //displays apmnt ID, patient name and date and time.  
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prints notes associated with an appointment.
        */
        public static void printAppointmentNotes(Appointment appointment)
        {
            List<AppointmentNote> notes = appointment.getNotes(); //Retrieves all notes on an appointment

            try
            {
                for(int i = 0; i < notes.Count; i ++) //Iterates over the notes 
                {
                    Console.WriteLine($"Author: {notes[i].author.getName()} ({notes[i].author.getUsername()})"); // Writes the name of the prationer, and their username 
                    Console.WriteLine("Date: " + notes[i].date);
                    Console.WriteLine("Time: " + notes[i].time);
                    Console.WriteLine("Content:\n" + notes[i].content);
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prints all appointments for a practitioner on the current day.
        */
        public static void printDailyPractitionerAppointments(DentistNurse practitioner)
        {
            List<Appointment> appointments = DataSearching.getDailyPractitionerAppointments(practitioner); //gets the list of daily appointments for the given practioner

            try
            {
                for (int i = 0; i < appointments.Count; i++) //iterates over the appointmens 
                {
                    Console.WriteLine($"{appointments[i].getId()}: {appointments[i].getPatient().getName()} - {appointments[i].getTime()}"); //Prints them out in format: ID, Patient And naem + time 
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }

        }

        /**
        * Prints all appointments at a location for the current day.
        */
        public static void printDailyLocationAppointments(DentalPractice location)
        {
            List<Appointment> appointments = DataSearching.getDailyLocationAppointments(location); //gets the list of appointments for the given location 

            try
            {
                for(int i = 0; i < appointments.Count; i ++) //iterate over the the appointments 
                {
                    Console.WriteLine($"{appointments[i].getId()}: {appointments[i].getPatient().getName()} - Room {appointments[i].getRoom().getRoomNumber()} {appointments[i].getTime()}"); //prints appointments out ID, patient, name, Room no. and time 
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        #endregion

        #region record viewing

        /**
        * Views a practice
        */
        public static void viewPractice(DentalPractice practice)
        {
            List<TreatmentRoom> rooms = practice.getRooms(); //geta all rooms at practice
            try
            {
                Console.Clear();
                Console.WriteLine(practice.getLocation()); //writes the name of specific pratice 
                Console.WriteLine($"Address: {practice.getAddress()}"); //writes the address of the practice 

                Console.Write("Receptionist: ");
                if(practice.getReceptionist() is Receptionist) //Checks if receptionist is assigned 
                {
                    Console.WriteLine(practice.getReceptionist().getName()); //writes the name of receptionist 
                }
                else
                {
                    Console.WriteLine("Unassigned"); //if not 
                }

                Console.WriteLine("\nRooms\n=====");

                for(int i = 0; i < rooms.Count; i ++) //iterates over room no. 
                {
                    Console.WriteLine($"Room {i + 1}:"); //writes room no. adding +1 to index so more human friendly 

                    Console.Write("Dentist: ");
                    if(rooms[i].getDentist() is DentistNurse) //checks if dentist is assigned 
                    {
                        Console.WriteLine(rooms[i].getDentist().getName()); //gets the assigned name of the dentist 
                    }
                    else
                    {
                        Console.WriteLine("Unassigned"); 
                    }

                    Console.Write("Nurse: ");
                    if(rooms[i].getNurse() is DentistNurse) //checks if nurse is assigned 
                    {
                        Console.WriteLine(rooms[i].getNurse().getName()); //writes the name of the assigned nurse. 
                    }
                    else
                    {
                        Console.WriteLine("Unassigned");
                    }
                }
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Prints a Users information.
        */
        public static void viewUser(User user)
        {
            Staff staff;
            DentistNurse practitioner;

            try
            {
                Console.Clear();
                Console.WriteLine("Username: " + user.getUsername()); //Writes the username of the given user 
                Console.WriteLine("Name: " + user.getName()); //same as above but for name 
                Console.Write("Role: ");

                if(user is Administrator)
                {
                    Console.WriteLine("Administrator"); 
                }
                else
                {
                    staff = (Staff) user; //if not admin must be staff. casts the user to a staff object 

                    if(user is Receptionist) //if receptionist then
                    {
                        Console.WriteLine("Receptionist");
                    }
                    else
                    {
                        practitioner = (DentistNurse) user; 
                        Console.WriteLine(practitioner.getPractitionerType()); //prints the type of the practioner either Dentist or Nurse. 
                    }

                    Console.Write("Location: ");
                    if(staff.getPractice() is DentalPractice) //Checking whether staff is assigned a location 
                    {
                        Console.WriteLine(staff.getPractice().getLocation()); //Write location 
                    }
                    else
                    {
                        Console.WriteLine("Unassigned");
                    }
                }
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Displays a patients information
        */
        public static void viewPatient(Patient patient)
        {
            try
            {
                Console.Clear();
                Console.WriteLine(patient.getId()); //Patients ID 
                Console.WriteLine($"Name: {patient.getName()}"); //Patients name 
                Console.Write($"Gender: ");

                if(patient.getGender() == Gender.MALE) // if gender is MAlE then write male. 
                {
                    Console.WriteLine("Male");
                }
                else
                {
                    Console.WriteLine("Female");
                }
                
                Console.WriteLine($"Address: {patient.getAddress()}");
                Console.WriteLine($"Contact No: {patient.getContactNo()}");
                Console.WriteLine($"Practice: {patient.getPractice().getLocation()}");

                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        /**
        * Displays all information about an appointment.
        */
        public static void viewAppointment(Appointment appointment)
        {
            try
            {
                Console.WriteLine("Patient: " + appointment.getPatient().getName()); //Gets the patient from the appointment, then gets the name from the patient

                Console.WriteLine("\nRoom: " + appointment.getRoom().getRoomNumber()); //same concept as above
                Console.Write("Dentist: ");

                if(appointment.getRoom().getDentist() is DentistNurse) //Checks if the room has a dentist
                {
                    Console.WriteLine(appointment.getRoom().getDentist().getName()); //writes the name of the dentist and the assigned room. 
                }
                else
                {
                    Console.WriteLine("Unassigned");
                }

                Console.Write("Nurse: ");
                if(appointment.getRoom().getNurse() is DentistNurse) //checks if the room has a nurse 
                {
                    Console.WriteLine(appointment.getRoom().getNurse().getName()); //writes the name of the nurse and assigned room 
                }
                else
                {
                    Console.WriteLine("Unassigned");
                }
                Console.WriteLine();

                Console.WriteLine("Date: " + appointment.getDate()); //prints date from appointment
                Console.WriteLine("Time: " + appointment.getTime()); //prints time from appointment. 

                Console.WriteLine("\nNotes \n=====");
                printAppointmentNotes(appointment); //Prints the notes on the appointment. 

                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }

        #endregion
    }
}
