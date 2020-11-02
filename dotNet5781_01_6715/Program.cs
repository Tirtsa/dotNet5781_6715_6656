using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_6715
{
    class Program
    {
        public static Random r = new Random(DateTime.Now.Millisecond);


        /* ---------------------------------------
        class bus - contains infos of bus object 
        ----------------------------------------- */
        public class Bus
        {
            public string Immatriculation 
            {
                get 
                {
                    //return immatriculation in format 12-345-67 or 123-45-678
                    Console.WriteLine(DateStart.Year);
                    if (DateStart.Year < 2018)
                        return Immatriculation.Substring(0, 2) + "-" + Immatriculation.Substring(2, 3) + "-" +
                            Immatriculation.Substring(5);
                    else
                        return Immatriculation.Substring(0, 3) + "-" + Immatriculation.Substring(3, 2) + "-" +
                            Immatriculation.Substring(5);
                }
                set { Immatriculation = value; } 
            }
            public DateTime DateStart { get; set; }

            float kilometrage = 0;
            public float Kilometrage { get; }
            public float KmOfFuel { get; set; } //how much km can the bus drive with its gasoline
            public DateTime MaintenanceDate { get; set; }
            public float MaintenanceKm { get; set; }

            /// <summary>
            /// Dangerous - return if bus can do travel or is 'dangerous'
            /// </summary>
            /// <param name="km"></param>
            /// <returns></returns>
            public bool Dangerous (int km) 
            {
                if ((MaintenanceKm + km) > 20000 || (DateTime.Now - MaintenanceDate).Days >= 365)
                    return true;
                else
                    return false;
            }

            /// <summary>
            /// addTravel - add kms to kilometrage and diminue of kmoffuel
            /// </summary>
            /// <param name="km"></param>
            public void addTravel(int km)
            {
                this.kilometrage += km;
                this.KmOfFuel -= km;
            }

            //ctor
            public Bus() { }
            
        }




        /* ---------------------------------------
       void main - principal program
       ----------------------------------------- */

        static void Main(string[] args)
        {
            List<Bus> buses = new List<Bus>(); //create empty list of buses
            char choice;

            //Menu
            do
            {
                Console.WriteLine("Please choose the action you want to do :\na: Add a bus\nb: Add a bus journey\n" +
                "c: Refueling or maintenance\nd: Print kilometrage since last maintenance for all buses\ne: Exit");

                choice = Convert.ToChar(Console.Read());
                switch (choice)
                {
                    case 'a':
                        addABus();
                        break;
                    case 'b':
                        addATravelBus(busOperation(), r.Next());
                        break;
                    case 'c':
                        addRefuelMaintenance(busOperation());
                        break;
                    case 'd':
                        printKilometrages();
                        break;
                    default:
                        break;
                }

            } while (choice != 'e');


            ///<summary>
            /// Function searchBus - return Bus object find by its Immatriculation in buses list
            /// @parameter : string immat   /immatriculation of bus we have to find
            /// @return : Bus               /Bus if it exist or null
            ///</summary>
            Bus searchBus(string immat) { return buses.Find(x => x.Immatriculation.Equals(immat)); }

            ///<summary>
            ///void printKilometrages - print for all buses km since last maintenance
            ///</summary>
            void printKilometrages()
            {
                foreach(Bus item in buses)
                    Console.Write("Immatriculation : {0, -9} Km since last maintenance : {1}",
                        item.Immatriculation, (item.Kilometrage-item.MaintenanceKm));
            }
            
            ///<summary>
            ///Function busOperation - receive immatriculation number from user and return Bus or null
            ///</summary>
            Bus busOperation()
            {
                Console.WriteLine("Please enter immatriculation : ");
                return searchBus(Console.ReadLine());
            }

            ///<summary>
            ///void addABus - add bus to buses list with Immatriculation and dateStart filled in by user
            ///</summary>
            void addABus()
            {
                //read and parse date from user
                Console.WriteLine("Please enter date of start activity : ");
                Console.ReadLine();
                DateTime mydate;
                DateTime.TryParse(Console.ReadLine(), out mydate);

                Console.WriteLine("Please enter immatriculation number (xxxxxxx or xxxxxxxx) : ");
                //if immatriculation format corresponds to year of start activity add the bus to list
                if ((mydate.Year < 2018 && Console.ReadLine().Length == 7) || (mydate.Year >= 2018 &&
                    Console.ReadLine().Length == 8))
                {
                    buses.Add(new Bus() { DateStart = mydate, Immatriculation = Console.ReadLine(), KmOfFuel = 0 });
                }
                else
                {
                    Console.WriteLine("Before 2018, vehicles must have 7 digits and since 2018, they must have 8 digits");
                }
            }

            ///<summary>
            ///void addATravelBus - verifiy if bus can effectue new travel. If it's possible update infos by "addTravel()"
            ///</summary>
            void addATravelBus(Bus myBus, int newTravelKm)
            {
                if (myBus == null)
                    Console.WriteLine("There is no bus with this immatriculation");
                else if (myBus.Dangerous(newTravelKm) == true)
                    Console.WriteLine("This bus is \"dangerous\" and cannot effectue this travel");
                else if (myBus.KmOfFuel < newTravelKm)
                    Console.WriteLine("This bus have not fuel suffisant for this travel");
                else
                    myBus.addTravel(newTravelKm);
            }

            ///<summary>
            ///void addRefuelMaintenance - add to bus fuel or maintenance by updating maintenance's date and km or kmOfFuel
            ///</summary>
            void addRefuelMaintenance(Bus myBus)
            {
                if (myBus == null)
                    Console.WriteLine("There is no bus with this immatriculation");
                else
                {
                    Console.WriteLine("Please choose if you want refueling (r) or mainetnance (m) : ");
                    char ch = Convert.ToChar(Console.Read());
                    if (ch == 'r' || ch == 'R')
                        myBus.KmOfFuel += 1200;
                    if (ch == 'm' || ch == 'M')
                    {
                        myBus.MaintenanceDate = DateTime.Now;
                        myBus.MaintenanceKm = myBus.Kilometrage;
                    }
                }
            }
        }
    }
}
