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
        public static void printPractices()
        {
            List<DentalPractice> practices = Application.appInstance.dentalPractices;

            try
            {
                for(int i = 0; i < practices.Count; i ++)//iteration of the amount of practices 
                {
                    Console.WriteLine(practices[i].getLocation()); //get location andprint it out 

                    for (int locLen = 0; locLen < practices[i].getLocation().Length; locLen ++) //Underlines the location with "="
                    {
                        Console.Write("=");
                    }
                    Console.WriteLine();

                    if (practices[i].getReceptionist() is Receptionist) //check if receptionist has been allocated, if not returns null. 
                    {
                        Console.WriteLine($"Receptionist: {practices[i].getReceptionist().getName()}"); //$ can open curly brackets and insert variables within it
                    }
                    Console.WriteLine($"Address: {practices[i].getLocation()}\n"); // \n new line 
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
                    Console.WriteLine("Room: {0}", rooms[i].getRoomNumber()); 
                    if (rooms[i].getDentist() is DentistNurse)
                    {
                        Console.WriteLine("Dentist: {0} - {1}", rooms[i].getDentistId(), rooms[i].getDentist().getName()); //prints "Dentist: <username> - <name>"
                    }

                    if (rooms[i].getNurse() is DentistNurse)
                    {
                        Console.WriteLine("Nurse: {0} - {1}\n", rooms[i].getNurseId(), rooms[i].getNurse().getName()); //Same as above for nurse.
                    }
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
            List<Staff> staff = DataSearching.getStaff(); //Return a list the user objects that are staff
            DentistNurse dentistNurse;

            try
            {
                for(int i = 0; i < staff.Count; i ++)
                {
                    Console.WriteLine(staff[i].getUsername());

                    for (int locLen = 0; locLen < staff[i].getUsername().Length; locLen ++) //Underlines the ussername with "="
                    {
                        Console.Write("=");
                    }

                    Console.WriteLine("\nName: {0}", staff[i].getName()); //writes staff name 
                    if (staff[i].getPractice() is DentalPractice)
                    {
                        Console.WriteLine("Practice: : {0}", staff[i].getPractice().getLocation()); //gets practice info and writes its location
                    }

                    Console.Write("Role: ");//writes staff role depenedant on prationer type 

                    if(staff[i] is Receptionist)
                    {
                        Console.WriteLine("Receptionist");
                    }
                    else
                    {
                        dentistNurse = (DentistNurse)staff[i];
                        if(dentistNurse.getPractitionerType() == "Dentist") //if equal to dentist write this otherwise it will be nurse 
                        {
                            Console.WriteLine("Dentist");
                        }
                        else
                        {
                            Console.WriteLine("Nurse");
                        }
                    }

                    Console.WriteLine();
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
                //Returns a list of all staff with the given location.
                staffAtPractice = staff.FindAll(staffMem => (staffMem.getPractice() is DentalPractice) && (staffMem.getPractice().Equals(practice))); 

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
                        denNurseToPrint = (DentistNurse) staffAtPractice[i];

                        if (denNurseToPrint.getPractitionerType() == "Dentist")
                        {
                            Console.WriteLine("Dentist");
                        }
                        else
                        {
                            Console.WriteLine("Nurse");

                        }
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
                        Console.WriteLine("{0}: {1} - {2}", receptionists[i].getUsername(), receptionists[i].getName(), receptionists[i].getPractice().getLocation());//if assigned write this 
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
                    Console.WriteLine($"{patients[i].getId()}: {patients[i].getName()} - {patients[i].getContactNo()}\n{patients[i].getAddress()}\n"); //writes patient information from the desired practice 
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
            List<AppointmentNote> notes;

            try
            {
                for(int i = 0; i < appts.Count; i ++) //iterates appointments
                {
                    Console.WriteLine($"{appts[i].getId()}: {appts[i].getPatient().getName()} - Room {appts[i].getRoom().getRoomNumber()}: {appts[i].getRoom().getDentist().getName()} - {appts[i].getRoom().getNurse().getName()}");
                    Console.WriteLine($"{appts[i].getDate()} {appts[i].getTime()}");
                    notes = appts[i].getNotes();

                    Console.WriteLine("Notes:\n======");
                    for (int note = 0; note < notes.Count; note ++) //iterates notes 
                    {
                        Console.WriteLine($"{notes[note].author.getName()} - {notes[note].date} {notes[note].time}"); //<author> - d/MM/yyyy HH:mm
                        Console.WriteLine(notes[note].content + "\n");
                    }
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
                for(int i = 0; i < appointments.Count; i ++) //iterates appointments 
                {
                    Console.WriteLine($"{appointments[i].getId()} - {appointments[i].getPatient().getName()}: {appointments[i].getDate()} {appointments[i].getTime()}"); //displays apmnt ID, patient name and date and time. 
                }
            }
            catch (Exception e)
            {
                GeneralFunctions.errorHandler(e);
            }
        }
    }
}
