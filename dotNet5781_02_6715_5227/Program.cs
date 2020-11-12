using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string StationAdress { get; set; }

        
        //initialize list of all bus stations (class ListBusStation)
        ListBusStation ListOfAllStations=new ListBusStation();
        
        //constuctor with no parameter
        public BusStation()
        {
            BusStationKey = codeStation ++;
            Latitude = rLatitude.NextDouble()+ 31;
            Longitude = rLongitude.NextDouble() + 34;
            StationAdress = " ";
            ListOfAllStations.addStation(this); //add the bus station to the list
        }
        //constructor with parameter : adress of station
        public BusStation(string name)
        {
            BusStationKey = helpBusStationKey ++;
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

        public int BusLine {get; set;}
        public BusStation FirstStation {get; set; }
        public BusStation LastStation {get; set; }
        public areas Area {get; set; }
        List<BusLineStation> Stations = new List<BusLineStation> ();
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            BusStation test = new BusStation();
            BusStation test2 = new BusStation("adress");

            Console.WriteLine(test.ToString());
            Console.WriteLine(test2.ToString());
        }
    }
}
