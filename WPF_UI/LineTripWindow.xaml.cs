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
using System.Threading;
using System.Diagnostics;
using BLApi;
using BO;

namespace WPF_UI
{
    /// <summary>
    /// Interaction logic for LineTripWindow.xaml
    /// </summary>
    public partial class LineTripWindow : Window
    {
        static IBL bl = BlFactory.GetBL();
        private Stopwatch stopWatch;
        private bool isTimerRun;
        private Thread timerThread;
        private TimeSpan start = TimeSpan.Zero;
        public LineTripWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stopWatch = new Stopwatch();
            timerTB.Text = stopWatch.Elapsed.ToString();

            lbStations.ItemsSource = from station in bl.GetAllBusStations()
                                     select " תחנה " + station.BusStationKey + " : " + station.StationName;
        }

        private void StartClock()
        {
            if (!isTimerRun)
            {
                stopWatch.Restart();
                isTimerRun = true;

                timerThread = new Thread(RunTimer);
                timerThread.Start();
            }
        }

        private void RunTimer()
        {
            while (isTimerRun)
            {
                string timerText = (start + stopWatch.Elapsed).ToString();
                timerText = timerText.Substring(0, 8);

                SetTimer(timerText);
                Thread.Sleep(1000);
            }
        }

        void SetText(string text)
        {
            timerTB.Text = text;
        }

        void SetTimer(string timerText)
        {
            if (!CheckAccess())
            {
                Action<string> d = SetText;
                Dispatcher.BeginInvoke(d, timerText);
            }
            else
            {
                timerTB.Text = timerText;
            }
        }

        private void startSimulationButton_Click(object sender, RoutedEventArgs e)
        {   
            timerTB.Text = start.ToString();
            start = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            StartClock();
        }
        private void lbStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.SelectedIndex != -1)
            {
                string station = lb.SelectedItem as string;
                lbBuses.ItemsSource = from line in bl.GetAllBusLines()
                                      where line.AllStationsOfLine.Contains(int.Parse(station.Substring(6, 5)))
                                      select "קו: " + line.BusLineNumber;
                UpdateLayout();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

    }
}
