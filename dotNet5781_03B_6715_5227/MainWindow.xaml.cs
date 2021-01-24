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
using System.ComponentModel;
using System.Diagnostics;

namespace dotNet5781_03B_6715_5227
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>

    public static class ListBuses
    {
        public static List<Bus> allBuses = new List<Bus>();

        static void InitializeList()
        {
            //initialization of 10 buses
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("01/12/2000"), Immatriculation = "1234567", KmOfFuel = 900, 
                MaintenanceDate = DateTime.Parse("01/12/2020"), MaintenanceKm = 10000, Kilometrage = 15000 });
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("01/05/2004"), Immatriculation = "2345678", KmOfFuel = 1200,
                MaintenanceDate = DateTime.Parse("01/12/2019"), MaintenanceKm = 15000, Kilometrage = 16000 });
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("15/09/2010"), Immatriculation = "3456789", KmOfFuel = 1200,
                MaintenanceDate = DateTime.Parse("21/06/2020"), MaintenanceKm = 5000, Kilometrage = 24000 });
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("24/11/2018"), Immatriculation = "12345678", KmOfFuel = 1000,
                MaintenanceDate = DateTime.Parse("01/08/2020"), MaintenanceKm = 4000, Kilometrage = 8000 });
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("20/01/2002"), Immatriculation = "4567891", KmOfFuel = 500,
                MaintenanceDate = DateTime.Parse("11/09/2020"), MaintenanceKm = 12000, Kilometrage = 19000 });
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("01/07/2012"), Immatriculation = "5678912", KmOfFuel = 1200,
                MaintenanceDate = DateTime.Parse("15/10/2020"), MaintenanceKm = 20000, Kilometrage = 22000 });
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("05/01/2019"), Immatriculation = "23456789", KmOfFuel = 450,
                MaintenanceDate = DateTime.Parse("04/11/2020"), MaintenanceKm = 7000, Kilometrage = 13000 });
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("11/11/2011"), Immatriculation = "6789123", KmOfFuel = 200,
                MaintenanceDate = DateTime.Parse("20/04/2020"), MaintenanceKm = 22000, Kilometrage = 30000 });
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("03/12/2015"), Immatriculation = "7891234", KmOfFuel = 1000,
                MaintenanceDate = DateTime.Parse("01/07/2020"), MaintenanceKm = 6000, Kilometrage = 23000 });
            allBuses.Add(new Bus() { DateStart = DateTime.Parse("01/08/2016"), Immatriculation = "8912345", KmOfFuel = 100,
                MaintenanceDate = DateTime.Parse("09/10/2020"), MaintenanceKm = 20500, Kilometrage = 35000 });
        }

        //ctor to initialize list
        static ListBuses() { InitializeList(); }

    }

    public partial class MainWindow : Window
    {
        BackgroundWorker refueling;
        Bus myBus;

        public MainWindow()
        {  
            InitializeComponent();

            DataContext = ListBuses.allBuses;
            this.BusListDataGrid.ItemsSource = ListBuses.allBuses;
        }

        private void AddBusButton_Click(object sender, RoutedEventArgs e)
        {
            
            AddBusWindow addBusWindow = new AddBusWindow();
            addBusWindow.Show();
            this.Close();
        }

        public void FuelButton_Click(object sender, RoutedEventArgs e)
        {
            refueling = new BackgroundWorker();
            refueling.DoWork += Refueling_DoWork;
            refueling.ProgressChanged += Refueling_ProgressChanged;
            refueling.RunWorkerCompleted += Refueling_RunWorkerCompleted;

            refueling.WorkerReportsProgress = true;
            refueling.WorkerSupportsCancellation = true;

            myBus = (Bus)(sender as Button).DataContext;
            refueling.RunWorkerAsync();
        }

        private void Refueling_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myBus.BusStatus = "מוכן לנסיעה";
            if (e.Error != null)
                MessageBox.Show("Error : " + e.Error.Message);
            else
            {
                MessageBox.Show("התידלוק של אוטובוס " + myBus.Immatriculation + " הסתיים");
                Refresh();
            }
        }

        private void Refueling_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            mytest.Content = progress + "%"; 
        }

        private void Refueling_DoWork(object sender, DoWorkEventArgs e)
        {
            if(myBus.BusStatus != "באמצע נסיעה")
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                myBus.KmOfFuel += 900;
                myBus.BusStatus = "בתידלוק";
                Refresh();
                for (int i = 0; i < 12; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    refueling.ReportProgress(i * 100 / 12);
                }
                e.Result = stopwatch.ElapsedMilliseconds;
            }
            else
            {
                MessageBox.Show("אוטובוס " + myBus.Immatriculation + "נמצא בנסיעה. נא לחכות לסופה");
                refueling.CancelAsync();
            }
        }

        private void TravelButton_Click(object sender, RoutedEventArgs e)
        {
            Button test = sender as Button;
            NewTravelWindow newTravelWindow = new NewTravelWindow { DataContext = test.DataContext };
            newTravelWindow.Show();
            this.Close();
        }


        private void BusListDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BusDetailsWindow busDetailsWindow = new BusDetailsWindow { DataContext = BusListDataGrid.SelectedItem };
            busDetailsWindow.Show();
            this.Close();
        }
        private void Refresh()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MainWindow newWindow = new MainWindow();
                newWindow.Show();
                this.Close();
            }));
            
        }
    }
}
