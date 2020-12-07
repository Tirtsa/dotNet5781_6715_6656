using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Security.Permissions;
using System.Collections;
using System.Device.Location;

namespace dotNet5781_03A_6715_5227
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public class ListBusStation
        {
            /// <summary>
            /// List which contains all bus station.
            /// </summary>

            public static List<BusStation> ListOfAllStations = new List<BusStation>();

            public void addStation(BusStation myStation)
            {
                ListOfAllStations.Add(new BusStation(myStation));
            }

            public void deleteStation(int stationBusToDelete)
            {
                foreach (BusStation item in ListOfAllStations)
                {
                    if (item.BusStationKey == stationBusToDelete)
                    {
                        ListOfAllStations.Remove(item);
                        Console.WriteLine("Bus station removed with success");
                    }
                }
            }

            public BusStation SearchStation(int stationKey)
            {
                foreach (BusStation item in ListOfAllStations)
                {
                    if (item.BusStationKey == stationKey)
                    {
                        return item;
                    }
                }
                throw new ArgumentException($"Station {stationKey} doesn't exist");
            }
        }
        public class BusStation : ListBusStation
        {
            //random numbers for latitude and longitude
            static Random rLatitude = new Random(DateTime.Now.Millisecond);
            static Random rLongitude = new Random(DateTime.Now.Millisecond);

            public static int codeStation = 1;

            //fields of bus station
            public int BusStationKey { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string StationAdress { get; set; }


            public BusStation() //constuctor with no parameter
            {
                BusStationKey = codeStation++;
                Latitude = rLatitude.NextDouble() * (33.3 - 31) + 31;
                Longitude = rLongitude.NextDouble() * (34.3 - 35.5) + 34.3;
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

            public BusStation(BusStation myStation)//copy constructor
            {
                BusStationKey = myStation.BusStationKey;
                Latitude = myStation.Latitude;
                Longitude = myStation.Longitude;
                StationAdress = myStation.StationAdress;
            }
            public BusStation(int numStation)
            {
                BusStationKey = numStation;
                Latitude = rLatitude.NextDouble() + 31;
                Longitude = rLongitude.NextDouble() + 34;
                StationAdress = "";
                addStation(this);
            }

            //override of ToString method
            public override string ToString()
            {
                return "Bus Station Code:" + BusStationKey + ", " + Latitude + "°N " + Longitude + "°E " + StationAdress;
            }
        }
        public class BusLine : ListBusStation, IComparable
        {
            /**************************************************
            * Class BusLineStation - an interior class for station of a line bus route
            * 
            **************************************************/
            public class BusLineStation : ListBusStation
            {
                public int BusStationKey { get; set; }
                public double Distance { get; set; }
                public double TravelTime { get; set; }
                public BusLineStation(int codeStation, BusLineStation prevStation)
                {
                    BusStationKey = codeStation;
                    if (prevStation == null)
                        prevStation = this;
                    
                    Distance = DistanceFromPrevStation(SearchStation(this.BusStationKey),
                        SearchStation(prevStation.BusStationKey));
                    TravelTime = Distance * 0.02;
                }

                //functions
                public double DistanceFromPrevStation(BusStation station1, BusStation station2)
                {
                    var eCord = new GeoCoordinate(station1.Latitude, station1.Longitude);
                    var sCord = new GeoCoordinate(station2.Latitude, station2.Longitude);
                    return eCord.GetDistanceTo(sCord);
                }
            }

            //constructors
            public BusLine() { }
            public BusLine(int lineNumber, List<int> listStations)
            {
                BusLineNum = lineNumber;
                foreach (int item in listStations)
                    AddStation(3, item);
                TotalTime = 0;
                foreach (BusLineStation item in Stations)
                    TotalTime += item.TravelTime;
            }
            public BusLine(BusLine myLine)
            {
                BusLineNum = myLine.BusLineNum;
                for (int i = 0; i < myLine.Stations.Count(); i++)
                    AddStation(3, myLine.Stations[i].BusStationKey);
                TotalTime = 0;
                foreach (BusLineStation item in Stations)
                    TotalTime += item.TravelTime;
            }


            //static & enum fields
            enum areas { General, North, South, Center, Jerusalem };

            //properties
            public int BusLineNum { get; set; }
            public BusLineStation FirstStation { get { return Stations[0]; } }
            public BusLineStation LastStation
            {
                get
                {
                    return Stations[Stations.Count - 1];
                }
            }
            public double TotalTime { get; set; }
            areas Areas { get; set; }
            public List<BusLineStation> Stations = new List<BusLineStation>();
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
                if (searchStationIndex(myStation.BusStationKey) == 0)
                {
                    return myStation;
                }
                return Stations[searchStationIndex(myStation.BusStationKey) - 1];
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

            public void PrintStations()
            {
                foreach (BusLineStation item in Stations)
                    Console.Write(item.BusStationKey + " ");
                Console.WriteLine();
            }

            //methods
            public override string ToString()
            {
                Console.Write("Bus line: " + BusLineNum + "  First Station: " + FirstStation.BusStationKey + "   Last Station: " +
                    LastStation.BusStationKey + "   Areas: " + Areas + "\nStations: ");
                PrintStations();
                return " ";
            }
            public void AddStation(int num, int codeStation)
            {
                switch (num)
                {
                    case 1:
                        Stations.Insert(0, new BusLineStation(codeStation, null));
                        break;
                    case 2:
                        Console.WriteLine("Please enter previous station's code : ");
                        int prevcode = int.Parse(Console.ReadLine());
                        int index = searchStationIndex(prevcode);
                        if (index == -1)
                            Console.WriteLine("ERROR, this station not exist in this bus line");
                        else
                            Stations.Insert(index + 1, new BusLineStation(codeStation, Stations[index]));
                        break;
                    case 3:
                        if (Stations.Count > 0)
                            Stations.Add(new BusLineStation(codeStation, Stations[Stations.Count - 1]));
                        else
                            Stations.Add(new BusLineStation(codeStation, null));
                        break;  
                    default:
                        break;
                }
            }

            public void DeleteStation(int codeStation)
            {
                int index = searchStationIndex(codeStation);
                if (index != -1)
                {
                    BusLineStation stationToDelete = Stations[index];
                    Stations.Remove(stationToDelete);
                }
                throw new ArgumentException($"Line bus doesn't contains station {codeStation}.");
            }

            public bool CheckBusStation(BusStation station1)
            {
                foreach (BusLineStation item in Stations)
                    if (station1.BusStationKey == item.BusStationKey)
                        return true;
                return false;
            }

            public double getDistance(int stationKey1, int stationKey2)
            {
                var sCoord = new GeoCoordinate(ListOfAllStations[stationKey1].Latitude, ListOfAllStations[stationKey1].Longitude);
                var eCoord = new GeoCoordinate(ListOfAllStations[stationKey2].Latitude, ListOfAllStations[stationKey2].Longitude);

                return sCoord.GetDistanceTo(eCoord);
            }

            public double TravelTimeBetweenStations(int codeStation1, int codeStation2)
            {
                return 0.5 * getDistance(codeStation1, codeStation2);
            }

            public BusLine PartOfLine(int begin, int end)
            {
                List<int> PartialStations = new List<int>();
                for (int i = begin; i <= end; i++)
                {
                    PartialStations.Add(this.Stations[i].BusStationKey);
                }

                return new BusLine(this.BusLineNum, PartialStations);
            }

            public int CompareTo(object obj)
            {
                BusLine b = (BusLine)obj;
                return TravelTimeBetweenStations(FirstStation.BusStationKey, LastStation.BusStationKey).CompareTo
                    (b.TravelTimeBetweenStations(b.FirstStation.BusStationKey, b.LastStation.BusStationKey));
            }
        }
        public class BusesCollection : ListBusStation, IEnumerable
        {
            //fields ans properties
            List<BusLine> BusesLines = new List<BusLine>();


            public IEnumerator GetEnumerator()
            {
                for (int i = 0; i < BusesLines.Count; i++)
                    yield return BusesLines[i];
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
                    if (item.BusLineNum == line)
                        throw new ArgumentException(String.Format($"Bus line's number must be unique, line {line} is " +
                            "already exists."));
                }


                foreach (int item in Stations)
                {
                    bool exist = false;
                    foreach (BusStation item2 in ListOfAllStations)
                        if (exist == false)
                            if (item2.BusStationKey == item)
                                exist = true;
                    if (exist == false)
                        throw new ArgumentException($"Station number {item} doesn't exist");
                }
                BusesLines.Add(new BusLine(line, Stations));
            }

            public void Add(BusLine myLine)
            {
                BusesLines.Add(new BusLine(myLine));
            }


            /// <summary>
            /// function Remove - to remove a bus line from collection
            /// </summary>
            /// <param name="myLine">int - num of line to delete</param>
            public void Remove(BusLine myLine)
            {
                BusesLines.Remove(myLine);
            }

            /// <summary>
            /// Function LisOfLine - list of all bus' lines in a station
            /// </summary>
            /// <param name="codeStation">int - code of station</param>
            /// <returns></returns>
            public List<BusLine> ListOfLines(int codeStation)
            {
                List<BusLine> ListLines = new List<BusLine>();
                BusStation myStation = SearchStation(codeStation);
                foreach (BusLine item in BusesLines)
                {
                    if (item.searchStationIndex(codeStation) != -1)
                        ListLines.Add(item);
                }
                if (!ListLines.Any())
                    throw new ArgumentException($"No line goes through station {codeStation}");
                return ListLines;
            }

            /// <summary>
            /// Sorted List of all Bus' lines
            /// </summary>
            public void SortedBusLineList()
            {
                BusesLines.Sort(delegate (BusLine x, BusLine y)
                {
                    return x.TotalTime.CompareTo(y.TotalTime);
                });
            }

            /// <summary>
            /// Indexer - return BusLine by line number
            /// </summary>
            /// <param name="lineNumber"></param>
            /// <returns></returns>
            public BusLine this[int lineNumber]
            {
                get
                {
                    foreach (BusLine item in BusesLines)
                        if (item.BusLineNum == lineNumber)
                            return item;
                    throw new ArgumentException($"There is no Bus Line with number {lineNumber}.");
                }

            }

            public bool Empty()
            {
                if (BusesLines.Count == 0)
                    return true;
                return false;
            }
        }

        //creat random number
        public static Random NumLine = new Random();
        public static Random NumStation = new Random(DateTime.Now.Millisecond);

        //creation of bus lines collection
        public BusesCollection busLines = new BusesCollection();

        //ctor of MainWindow class
        public MainWindow()
        {

            int newline;
            int newstation;
            for (int i = 0; i <= 10; i++)
            {
                List<int> listStations = new List<int>();
                newline = NumLine.Next(150);
                for (int j = 0; j <= 5; j++)
                {
                    newstation = NumStation.Next(200);
                    new BusStation(newstation);
                    listStations.Add(newstation);
                }
                busLines.Add(newline, listStations);
            }


            //load of wpf's compoments
            InitializeComponent();

            //load bus' lines to cbBusLines
            cbBusLines.ItemsSource = busLines;
            cbBusLines.DisplayMemberPath = "BusLineNum";
            cbBusLines.SelectedIndex = 0;

        }

        private BusLine currentDisplayBusLine;
        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = busLines[index];
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine.Stations;
        }

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).BusLineNum);
        }

    }
}
