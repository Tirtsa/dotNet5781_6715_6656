/*
 * Ex1 dotNet - construction of class Bus and main to can add buses and add travel, refueling or maintenance to buses
 * Author : tirts
 * Date : 02.11.2020
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_6715
{

    /* ---------------------------------------
    class bus - contains infos of bus object 
    ----------------------------------------- */
    public class Bus
    {
        public enum Status {Ready, Travelling, Refueling, underMaintenance};
        public string Immatriculation { get; set; }
        DateTime dateStart;
        public DateTime DateStart { get { return dateStart.Date; } set { dateStart = value; } }

        float kilometrage = 0;
        public float Kilometrage { get; set; }
        public float KmOfFuel { get; set; } //how much km can the bus drive with its gasoline
        public DateTime MaintenanceDate { get; set; }
        public float MaintenanceKm { get; set; }

        Status busStatus = Status.Ready;
        public Status BusStatus { get; set; }

        /// <summary>
        /// Dangerous - return if bus can do travel or is 'dangerous'
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public bool Dangerous(int km)
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
        public override string ToString()
        {
            if (DateStart.Year < 2018)
                return Immatriculation.Substring(0, 2) + "-" + Immatriculation.Substring(2, 3) + "-" +
                    Immatriculation.Substring(5);
            else
                return Immatriculation.Substring(0, 3) + "-" + Immatriculation.Substring(3, 2) + "-" +
                    Immatriculation.Substring(5);
        }

    }

    public class Program
    {
        public static Random r = new Random(DateTime.Now.Millisecond);

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

                Console.Read();
                choice = Convert.ToChar(Console.Read());
                switch (choice)
                {
                    case 'a':
                        addABus();
                        break;
                    case 'b':
                        addATravelBus(busOperation(), r.Next(900));
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
            Bus searchBus(string immat)
            //{
            //    foreach(Bus item in buses)
            //        if (item.)
            //}
            { return buses.Find(x => x.Immatriculation.Equals(immat)); }

            ///<summary>
            ///void printKilometrages - print for all buses km since last maintenance
            ///</summary>
            void printKilometrages()
            {
                foreach(Bus item in buses)
                    Console.Write("Immatriculation : {0, -9} \nKm since last maintenance : {1}",
                        item, (item.Kilometrage-item.MaintenanceKm));
            }
            
            ///<summary>
            ///Function busOperation - receive immatriculation number from user and return Bus or null
            ///</summary>
            Bus busOperation()
            {
                Console.Write("Please enter immatriculation : ");
                string test = Console.ReadLine();
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
                if (DateTime.TryParse(Console.ReadLine(), out mydate))
                {
                    Console.WriteLine("Please enter immatriculation number (xxxxxxx or xxxxxxxx) : ");
                    //if immatriculation format corresponds to year of start activity add the bus to list
                    string immat = Console.ReadLine();
                    if ((mydate.Year < 2018 && immat.Length == 7) || (mydate.Year >= 2018 &&
                        immat.Length == 8))
                    {
                        buses.Add(new Bus() { DateStart = mydate, Immatriculation = immat, KmOfFuel = 0 });
                    }
                    else
                    {
                        Console.WriteLine("Before 2018, vehicles must have 7 digits and since 2018, they must have 8 digits");
                    }
                }
                else
                    Console.WriteLine("You have entered an incorrect value");   
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
