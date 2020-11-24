using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Collections;

namespace dotNet5781_02_6715_5227
{


    /*************************************
     * Class with list of all Bus Stations
     * 
     *************************************/
    public class ListBusStation
    {
        /// <summary>
        /// creat list which contains all bus station.
        /// </summary>
        public List<BusStation> ListOfAllStations = new List<BusStation>();

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
                if (item.BusStationKey == stationBusToDelete)
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
                if (item.BusStationKey == stationKey)
                {
                    return item;
                }
             }
             return null;
        }
    }


    /*******************************************************
     * Class BusStation - all details concern bus station
     * 
     *******************************************************/
    public class BusStation : ListBusStation
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

        
        
        public BusStation() //constuctor with no parameter
        {
            BusStationKey = codeStation ++;
            Latitude = rLatitude.NextDouble() * (33.3 - 31) + 31;
            Longitude = rLongitude.NextDouble() *(34.3 - 35.5)+ 34.3;
            StationAdress = " ";
            addStation(this); //add the bus station to the list
        }

        
        public BusStation(string name) //constructor with parameter : adress of station
        {
            BusStationKey = codeStation++;
            Latitude = rLatitude.NextDouble() + 31;
            Longitude = rLongitude.NextDouble() + 34;
            StationAdress = name;
            addStation(this);//add the bus station to the list
        }
        
        //override of ToString method
        public override string ToString()
        {
            return "Bus Station Code:" + BusStationKey + "," + Latitude + "°N" + Longitude + "°E";
        }
    }

    /**************************************************
     * Class LineBus - a bbus line with many stations
     * 
     **************************************************/
    public class BusLine : ListBusStation, IComparable
    {
        /**************************************************
        * Class BusLineStation - an interior class for station of a line bus route
        * 
        **************************************************/
        public class BusLineStation : BusStation 
        {
            public double Distance 
            {
                get { return Distance; }
                set
                {
                    BusLineStation prevStation=new BusLineStation();
           
                    var eCord = new GeoCoordinate(this.Latitude, this.Longitude);
                    var sCord = new GeoCoordinate(prevStation.Latitude, prevStation.Longitude);
                    Distance = eCord.GetDistanceTo(sCord);
                }
            }
            public double TravelTime 
            {
                get { return TravelTime; }
                set { TravelTime = Distance * 0.5; }
            
            }
            public BusLineStation() { }
            
        }

        //constructors
        public BusLine() { }
        public BusLine(int lineNumber, List<int> listStations) 
        { 
            BusLineNumber = lineNumber;
            foreach (int item in listStations)
                AddStation(3, item);
        }


        //static & enum fields
        enum areas {General, North, South, Center, Jerusalem};
        static int codeLine = 000;

        //properties
        public int BusLineNumber {get; set;}
        public BusStation FirstStation { get {return Stations[0];}}
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
        public double TotalTime
        {
            get { return TotalTime; }
            set
            {
                TotalTime = getDistance(FirstStation.BusStationKey, LastStation.BusStationKey) * 0.5;
            }
        }
        areas Areas {get; set; }
        List<BusLineStation> Stations = new List<BusLineStation> ();
        BusLineStation this[int index]
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

        //functions

        /// <summary>
        /// search index of station and return station in previous index
        /// </summary>
        /// <returns>the previous station</returns>
        public BusLineStation previousStation(BusLineStation myStation)
        {
            if(searchStationIndex(myStation.BusStationKey)==0)
            {
                return myStation;
            }
            return Stations[searchStationIndex(myStation.BusStationKey)-1];
        }

        /// <summary>
        /// search index of a buslinestation in Stations' list with FindeIndex and bool function
        /// </summary>
        /// <param name="myStation">BusLineStation we want to find its index</param>
        /// <returns>int : index of the station in Stations' list</returns>
        public int searchStationIndex(int codeStation)
        {
          
            bool same(BusLineStation stat)
            {
                return stat.BusStationKey == codeStation;
            }
            return Stations.FindIndex(same);
        }

        public void PrintStations ()
        {
            foreach (BusLineStation item in Stations)
                Console.Write(item.BusStationKey + " ");
        }

        //methods
        public override string ToString()
        {
            Console.WriteLine("Bus line: " + BusLineNumber + " First Station: " + FirstStation + " Last Station: " + LastStation +
                " Areas: " + Areas + "Stations: ");
            PrintStations();
            return " ";
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
                    int prevcode = int.Parse(Console.ReadLine());
                    int index = searchStationIndex(prevcode);
                    if (index == -1)
                        Console.WriteLine("ERROR, this code not exist in this bus line");
                    else
                        Stations.Insert(index + 1, new BusLineStation() { BusStationKey = codeStation });
                    break;
                case 3:
                  Stations.Add(new BusLineStation(){BusStationKey = codeStation});
                  break;
		        default:
                    break;
	        }
        }

        public void DeleteStation(int codeStation)
        {
            int index = searchStationIndex(codeStation);
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

        public double getDistance(int codeStation1, int codeStation2)
        {
            BusLineStation myStation1 = new BusLineStation();
            BusLineStation myStation2 = new BusLineStation();
            foreach (BusLineStation item in Stations)
                if (item.BusStationKey == codeStation1)
                    myStation1 = item;
            foreach (BusLineStation item in Stations)
                if (item.BusStationKey == codeStation2)
                    myStation2 = item;
            var sCoord = new GeoCoordinate(myStation1.Latitude, myStation1.Longitude);
            var eCoord = new GeoCoordinate(myStation2.Latitude, myStation2.Longitude);

            return sCoord.GetDistanceTo(eCoord);
        }

        public double TravelTimeBetweenStations(int codeStation1, int codeStation2)
        {
            return 0.5 * getDistance(codeStation1, codeStation2);
        }

        public BusLine PartOfLine (int codeStation1, int codeStation2)
        {
            List<int> PartialStations = new List<int>();
            int begin = searchStationIndex(codeStation1);
            int end = searchStationIndex(codeStation2);
            int j = 0;
            for (int i = begin; i <= end; i++)
            {
                PartialStations[j] = Stations[i].BusStationKey;
                j++;
            }

            return new BusLine(this.BusLineNumber, PartialStations);
        }

        public int CompareTo(object obj)
        {
            BusLine b = (BusLine)obj;
            return TravelTimeBetweenStations(FirstStation.BusStationKey, LastStation.BusStationKey).CompareTo
                (b.TravelTimeBetweenStations(b.FirstStation.BusStationKey, b.LastStation.BusStationKey));
        }
    }

    class BusesCollection : ListBusStation, IEnumerable,IEnumerator
    {
        //fields ans properties
        List<BusLine> BusesLines { set; get; }
        public int Count { get; private set; }
        int index = -1;

        //implementation of IEnumerable, Ienumerator interfaces
        public object Current
        {
            get
            {
                return BusesLines[index];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            index++;
            if (index >= Count)
            {
                index = -1;
                return false;
            }
            return true;
        }

        public void Reset()
        {
            index = -1;
        }


        /// <summary>
        /// Function Add - to add a new bus line to collection
        /// </summary>
        /// <param name="line"> number of line we want to add</param>
        /// <param name="Stations">list of int - numbers of stations the bus line have</param>
        public void Add(int line, List<int> Stations)
        {
            foreach (BusLine item in BusesLines)
            {
                if (item.BusLineNumber == line)
                    throw new ArgumentException($"Bus line's number must be unique, {line} is already exists.");
            }
            bool exist = false;
            foreach (int item in Stations)
            {
                foreach (BusStation item2 in ListOfAllStations)
                    if (item2.BusStationKey == item)
                        exist = true;
                if (exist == false)
                    throw new ArgumentException($"There isn't exist station with {item} number.");
            }
            BusesLines.Add(new BusLine(line, Stations));
        }

        /// <summary>
        /// function Remove - to remove a bus line from collection
        /// </summary>
        /// <param name="myLine">int - num of line to delete</param>
        public void Remove(int myLine)
        {
            BusesLines.Remove(new BusLine() { BusLineNumber = myLine });
        }

        /// <summary>
        /// Function LisOfLine - list of all buses lines in a station
        /// </summary>
        /// <param name="codeStation">int - code of station</param>
        /// <returns></returns>
        public List<int> ListOfLines(int codeStation)
        {
            List<int> ListLines = new List<int>();
            BusStation myStation = searchStation(codeStation);
            foreach (BusLine item in BusesLines)
            {
                if (item.searchStationIndex(codeStation) != -1)
                    ListLines.Add(item.BusLineNumber);
            }
            if (!ListLines.Any())
                throw new ArgumentException("No line goes through this station");
            return ListLines;
        } 
        
        /// <summary>
        /// 
        /// </summary>
        public void SortedBusLineList()
        {
            BusesLines.Sort(delegate (BusLine x, BusLine y)
            {
                return x.TotalTime.CompareTo(y.TotalTime);
            });
        }

        public var this[int lineNumber]
        {
            get
            {
                foreach (BusLine item in BusesLines)
                    if (item.BusLineNumber == lineNumber)
                        return item;
                return new ArgumentException($"There is no Bus Line with number {lineNumber}.");
            }

        }
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
