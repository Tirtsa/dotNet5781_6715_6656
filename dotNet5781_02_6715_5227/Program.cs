using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_6715_5227
{

    class ListBusStation
        {
        //ListBusStation();
        /// <summary>
        /// creat list which contains all bus station.
        /// </summary>
        public List<BusStation> ListOfAllStations=new List<BusStation>();
        /*public void addStation(BusStation myStation)
        {
            ListOfAllStations.Add(new BusStation() {});
        }*/
        }
    class BusStation
    {
        static Random  rLatitude = new Random(DateTime.Now.Millisecond);
        static Random rLongitude = new Random(DateTime.Now.Millisecond);

       public static int BusStationKey = 111111;
       
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string StationAdress { get; set; }
        ListBusStation ListOfAllStations=new ListBusStation();
        BusStation()
        {
            BusStationKey ++;
            Latitude = rLatitude.NextDouble()+ 31;
            Longitude = rLongitude.NextDouble() + 34;
            StationAdress = " ";
            ListOfAllStations.
        }
        BusStation(string name)
        {
            BusStationKey++;
            Latitude = rLatitude.NextDouble() + 31;
            Longitude = rLongitude.NextDouble() + 34;
            StationAdress = name;
        }
        
    }
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
