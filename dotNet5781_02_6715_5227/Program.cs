using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace dotNet5781_02_6715_5227
{


    /*
     * Class with list of all Bus Stations
     */
    class ListBusStation
    {
        /// <summary>
        /// creat list which contains all bus station.
        /// </summary>
        public List<BusStation> ListOfAllStations=new List<BusStation>();

        /*
         * void addStation - add a new station to list of all stations
         * @parameter : BusStation (the bus station we want add)
         */
        public void addStation(BusStation myStation)
        {
            ListOfAllStations.Add(new BusStation() {BusStationKey = myStation.BusStationKey, 
                Latitude = myStation.Latitude, Longitude = myStation.Longitude, StationAdress = myStation.StationAdress});
        }

        public void deleteStation(int stationBusToDelete)
        {
            foreach(BusStation item in ListOfAllStations)
            {
                if (item.BusStationKey = stationBusToDelete)
                {
                    ListOfAllStations.Remove(item);
                    Console.WriteLine("Bus station removed with success");
                }
            }
        }

        public BusStation searchStation(int stationKey)
        {
             foreach(BusStation item in ListOfAllStations)
             {
                if (item.BusStationKey = stationKey)
                {
                    return item;
                }
             }
             // return -1;
        }
    }


    /*
     * Class BusStation - all details concern bus station
     */
    class BusStation
    {
        //random numbers for latitude and longitude
        static Random  rLatitude = new Random(DateTime.Now.Millisecond);
        static Random rLongitude = new Random(DateTime.Now.Millisecond);

        public static int codeStation = 111111;
        
        //fields of bus station
        public int BusStationKey {get; set;}
        public var Latitude { get; set; }
        public var Longitude { get; set; }
        public string StationAdress { get; set; }

        
        //initialize list of all bus stations (class ListBusStation)
        //public ListBusStation ListOfAllStations = new ListBusStation*();
        
        //constuctor with no parameter
        public BusStation(ListBusStation ListOfAllStations)
        {
            BusStationKey = codeStation ++;
            Latitude = rLatitude.NextDouble()+ 31;
            Longitude = rLongitude.NextDouble() + 34;
            StationAdress = " ";
            ListOfAllStations.addStation(this); //add the bus station to the list
        }
        //constructor with parameter : adress of station
        public BusStation(string name, ListBusStation ListOfAllStations)
        {
            BusStationKey = codeStation++;
            Latitude = rLatitude.NextDouble() + 31;
            Longitude = rLongitude.NextDouble() + 34;
            StationAdress = name;
            ListOfAllStations.addStation(this);//add the bus station to the list
        }
        
        //override of ToString method
        public override string ToString()
        {
            return "Bus Station Code:" + BusStationKey + "," + Latitude + "°N" + Longitude + "°E";
        }
    }

    /*
     * Class LineBus - a line with much of stations
     */
    class BusLine
    {
        /*
        * Class BusLineStation - a station of a line bus route
        */
        class BusLineStation
        {
            public int BusStationKey {get; set; }
            public double Distance {get; set; }
            public double TravelTime {get; set; }
            
            BusLineStation(int StationKey)
            {
                 
            }
        }

        enum areas {General, North, South, Center, Jerusalem};
        static int codeLine = 000;

        //fields
        public int BusLineNumber {get; set;}
        public BusStation FirstStation 
        {
            get
            {
                return Stations[0];
            }
        }
        public BusStation LastStation 
        {
            get
            {
                int i = 0;
                while (Stations[i]!=null)
                {
                    i++;
                }
                return Stations[i-1];
            }
        } 
        public areas Areas {get; set; }
        List<BusLineStation> Stations = new List<BusLineStation> ();
        public BusLineStation this[int index]
        {
            get
            {
                return Stations[index];
            }
            set
            {
                Stations[index] = value;
            }
        }
            public ListBusStation ListOfAllStations = new ListBusStation*();

        public int SearchStation(int codeStation)
        {
            foreach(BusLineStation item in Stations)
            {
                if (item.BusStationKey = codeStation)
                {
                    return Stations.FindIndex(item);
                }
            
            }
            return -1;
        }
        public void PrintStations ()
        {
            foreach (BusLineStation item in Stations)
                Console.Write(item.BusStationKey + " ");
        }

        //methodes
        public override string ToString()
        {
            return ("Bus line: " + BusLineNumber + " First Station: " + FirstStation + " Last Station: " + LastStation + 
                " Areas: " + Areas + "Stations: " + printStations());
        }
        public void AddStation(int num, int codeStation)
        {
            switch (num)
	        {
                case 1:
                    Stations.Insert(0, new BusLineStation(){BusStationKey = codeStation});
                    break;
                case 2:
                    Console.WriteLine("Please enter previous station's code : ");
                    int prevcode=Console.ReadLine();
                    int index = SearchStation(prevcode)
                    if (index == -1)
                        Console.WriteLine("ERROR, this code not exist in this bus line");
                    else
                        Stations.Insert(index+1, new BusLineStation(){BusStationKey = codeStation})
                    break;
                case 3:
                  Stations.Add(new BusLineStation(){BusStationKey = codeStation});
                  break;
		        default:
	        }
        }

        public void DeleteStation(int codeStation)
        {
            int index = SearchStation(codeStation);
            BusLineStation stationToDelete = Stations[index];
            Stations.Remove(stationToDelete);
        }

        public bool CheckBusStation(BusStation station1)
        {
            foreach(BusLineStation item in Stations)
                if (station1.BusStationKey == item.BusStationKey)
                    return true;
            return false;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            public ListBusStation ListOfAllStations = new ListBusStation*();
            
            BusStation test = new BusStation();
            BusStation test2 = new BusStation("adress");

            Console.WriteLine(test.ToString());
            Console.WriteLine(test2.ToString());
        }
    }
}
