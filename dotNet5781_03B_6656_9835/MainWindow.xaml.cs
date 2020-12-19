using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.Generic;

namespace dotNet5781_03B_6656_9835
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	enum Month { January = 1, February, March, April, May, June, July, August, September, October, November, December }

	public partial class MainWindow : Window
	{
		const int SERVICE_TIME = 144000, REFUEL_TIME = 12000;
		
		ObservableCollection<Bus> buses = new ObservableCollection<Bus>();
		List<BackgroundWorker> travelWorkers = new List<BackgroundWorker>();
		public MainWindow()
		{
			InitializeRandBuses(buses);
			InitializeComponent();

			lvBusList.DataContext = buses;
			lvBusList.ItemsSource = buses;
			lvBusList.SelectedItem = buses[0].LicenseStr;
			ShowBusList();
			
			this.Closing += MainWindow_Closed;
		}
		private void InitializeRandBuses(ObservableCollection<Bus> buses)
		{
			for (int i = 0; i <= 10; i++)           //randomly initializing bus list of 11 buses
			{
				Thread.Sleep(100);
				int rand = new Random().Next(1000000, 99999999);

				Month month = (Month)((rand % 11) + 1); //verifies month

				int yr = (rand % 120) + 1900; //ensures year is between 1900 to 2020
				if (yr < 2018)
					if (rand >= 10000000)
						rand /= 10;  //if year is earlier than 2018 than confirms license is only 7 digits

				Bus bus = new Bus(rand, (rand % 27) + 1, month, yr, (uint)((rand % 1000) + (rand % 10)));
				int index = buses.IndexOf(bus);
				if (index != -1)
				{
					int replace = rand;
					if (yr < 2018)						//appropriately assigns correct license number length
					{
						if (replace == 9999999)
							replace -= 10;
						else
							replace++;
					}
					else
					{
						if (replace == 99999999)
							replace -= 10;
						else
							replace++;
					}
				}
				bus = new Bus(rand, (rand % 27) + 1, month, yr, (uint)((rand % 1000) + (rand % 10)));
				buses.Add(bus);					//adds bus to main list of buses
			}

			int rnd = new Random().Next(9);	//ensures that 1 bus needs maintenence soon, one needs fuel soon and one just got serviced
			buses[rnd].MaintenanceDate = DateTime.Today.AddDays(-1);
			buses[rnd + 1].MaintenanceDate = (DateTime.Today.AddYears(-1).AddDays(2));
			buses[rnd + 2].DistanceSinceRefuel = buses[rnd + 2].DistanceSinceMaintenance = 1160;
		}
		private void ShowBusList() { lvBusList.DataContext = buses; }
		private void MainWindow_Closed(object sender, EventArgs e)
		{
			Environment.Exit(Environment.ExitCode);
		}
		private void AddBusButton_Click(object sender, RoutedEventArgs e)
		{
			AddBus addBusWindow = new AddBus();
			addBusWindow.Show();
		}
		public void AddBus(int license, int day, int month, int year)
		{
			Bus bus = new Bus(license, day, (Month)month, year, 0);
			if (ValidateNewBus(bus))			//if bus is allowed it is added to the main list
				buses.Add(bus);
			else
				MessageBox.Show("Invalid bus entry, try again!");
		}
		private bool ValidateNewBus(Bus bus)
		{
			if (bus.StartDate.Year < 1900 || bus.StartDate > DateTime.Today)
				return false;        //makes sure day entered is valid

			if ((bus.StartDate.Year < 2018 && bus.LicenseStr.Length != 7) || (bus.StartDate.Year >= 2018 && bus.LicenseStr.Length != 8))
				return false;        //makes sure day entered matches number of digits in license

			return true;
		}
		private void Worker_DoRefuel(object sender, DoWorkEventArgs e)
		{
			Bus bus = (Bus)e.Argument;					//coverts object to bus which worker was sent from
			int busIndex = buses.IndexOf(bus);

			MessageBox.Show("Sent bus " + bus.LicenseStr + " to be refueled");
			Thread.Sleep(REFUEL_TIME);
			buses[busIndex].DistanceSinceRefuel = 0;
			MessageBox.Show("Bus " + bus.LicenseStr + " was successfully refueled!");
		}
		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			Array argArray = (Array)e.Argument;                 //coverts object to array which worker was sent from
			uint distance = (uint)argArray.GetValue(0);
			Bus bus = (Bus)argArray.GetValue(1);				//gets bus and distance from array parameter
			int busIndex = buses.IndexOf(bus);

			int rand = new Random().Next(20, 50);
			int temp = (int)((double)6000 * distance / rand);		//calculates speed randomly and send on trip
			Thread.Sleep(temp);

			buses[busIndex].TotalDistance += distance;
			buses[busIndex].DistanceSinceMaintenance += distance;
			buses[busIndex].DistanceSinceRefuel += distance;
			buses[busIndex].Status = 0;
		}
		private void Worker_DoService(object sender, DoWorkEventArgs e)
		{
			Bus bus = (Bus)e.Argument;
			int busIndex = buses.IndexOf(bus);

			MessageBox.Show("Sent bus " + bus.LicenseStr + " for service");
			Thread.Sleep(SERVICE_TIME);
			buses[busIndex].DistanceSinceRefuel = 0;
			buses[busIndex].DistanceSinceMaintenance = 0;		//updates all relevant fields for servicing
			buses[busIndex].MaintenanceDate = DateTime.Today;
			buses[busIndex].Status = 0;
			MessageBox.Show("Bus " + bus.LicenseStr + " has been serviced!");
		}
		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			lvBusList.Items.Refresh();			//resets info held in all buses
			lvBusList.UpdateLayout();			//resets shown fields in list
		}
		private void TravelButton_Click(object sender, RoutedEventArgs e)
		{
			Button cmd = (Button)sender;
			Bus bus = (Bus)cmd.DataContext;

			switch (bus.Status)
			{
				case 1:
					MessageBox.Show("This bus is already traveling!");
					break;
				case 2:
					MessageBox.Show("This bus is currently being refueled");
					break;
				case 3:
					MessageBox.Show("This bus is in Service and cannot travel");
					break;

				default:
					BackgroundWorker backgroundWorker = new BackgroundWorker();
					travelWorkers.Add(backgroundWorker);
					backgroundWorker.WorkerReportsProgress = true;

					Travel TravelWindow = new Travel();		//if bus is ready than sends to travel function with background worker
					TravelWindow.CurrentBus = bus;
					TravelWindow.Show();
					break;
			}
		}
		public void Travel(object args)
		{
			Array argArray = (Array)args;			//coverts object to array which worker was sent from

			Bus bus = (Bus)argArray.GetValue(1);		//gets bus from array
			int busIndex = buses.IndexOf(bus);

			int bwIndex = (int)argArray.GetValue(2);		//gets worker from list

			travelWorkers[bwIndex].DoWork += Worker_DoWork;
			travelWorkers[bwIndex].RunWorkerCompleted += Worker_RunWorkerCompleted;

			if (!travelWorkers[bwIndex].IsBusy)			//sends worker to deal with bus travel
			{
				travelWorkers[bwIndex].RunWorkerAsync(args);
				buses[busIndex].Status = 1;
				lvBusList.Items.Refresh();
				lvBusList.UpdateLayout();
			}
		}
		public void Service(object obj)
		{
			Bus bus = (Bus)obj;

			if (bus.Status == 3)
				MessageBox.Show("This bus is already being serviced!");

			else if (bus.Status == 2)
				MessageBox.Show("This bus is being refueled, just wait a moment");

			else if (bus.Status == 1)
				MessageBox.Show("This bus is in the middle of a trip and can't be serviced!");
			else
			{
				BackgroundWorker backgroundWorker = new BackgroundWorker();
				backgroundWorker.WorkerReportsProgress = true;
				backgroundWorker.DoWork += Worker_DoService;
				backgroundWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;

				if (!backgroundWorker.IsBusy)			//worker deals with bus servicing
				{
					backgroundWorker.RunWorkerAsync(bus);
					buses[buses.IndexOf(bus)].Status = 3;
					lvBusList.Items.Refresh();
					lvBusList.UpdateLayout();
				}
			}
		}
		public void RefuelAssist(object obj)
		{
			Bus bus = (Bus)obj;		//deals with refueling from info window
			if (Refuel(bus))
			{
				buses[buses.IndexOf(bus)].Status = 2;
				lvBusList.Items.Refresh();
				lvBusList.UpdateLayout();
			}
		}
		private void RefuelButton_Click(object sender, RoutedEventArgs e)
		{
			Button cmd = (Button)sender;		//deals with refuel button from main window
			Bus bus = (Bus)cmd.DataContext;
			
			if(Refuel(bus))
			{
				buses[buses.IndexOf(bus)].Status = 2;
				lvBusList.Items.Refresh();
				lvBusList.UpdateLayout();
			}
		}
		private bool Refuel(Bus bus)
		{
			bus = buses[buses.IndexOf(bus)];

			if (bus.Status == 3)
				MessageBox.Show("This bus is already being serviced and refueled!");

			if (bus.Status == 2)
				MessageBox.Show("This bus is already being refueled!");

			else if (bus.Status == 1)
				MessageBox.Show("This bus is in the middle of a trip and can't be refueled!");
			else
			{
				BackgroundWorker backgroundWorker = new BackgroundWorker();
				backgroundWorker.WorkerReportsProgress = true;
				backgroundWorker.DoWork += Worker_DoRefuel;
				backgroundWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;

				if (!backgroundWorker.IsBusy)		//worker refuels
				{
					backgroundWorker.RunWorkerAsync(bus);
					return true;
				}
			}
			return false;
		}
		private void lbBusList_DoubleClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			Bus bus = new Bus();
			ListView list = (ListView)sender;

			if (list.SelectedItem != null)
			{
				StackPanel stack = (StackPanel)list.SelectedItem;
				bus = (Bus)stack.DataContext;
			}
			else
				bus = (Bus)list.DataContext;
			
			BusInfo BusInfoWindow = new BusInfo();			//opens additional window with information on selected bus
			BusInfoWindow.CurrentBus = buses[buses.IndexOf(bus)];
			BusInfoWindow.Show();
			BusInfoWindow.tbFill();
		}
	}
}
