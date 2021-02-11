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
using BO;
using BLApi;
using System.ComponentModel;
using System.Collections.ObjectModel;
using PO;
using System.Threading;
using System.Diagnostics;

namespace WPF_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static IBL bl = BlFactory.GetBL();
        static PL pl = new PL();

        #region fields for backgroundWorker
        BusStation station;
        IEnumerable<IGrouping<TimeSpan, PO.LineTiming>> listTest;
        TimeSpan startHour;

        LineTimingWindow newTiming;

        private Stopwatch stopwatch;
        BackgroundWorker timing;
        
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            BusStationsDg.ItemsSource = bl.GetAllBusStations();
            BusLinesDg.ItemsSource = bl.GetAllBusLines();

        }

        #region BackgroundWorder Methods
        private void Timing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void Timing_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string timerText = (startHour + TimeSpan.FromTicks(stopwatch.Elapsed.Ticks * 60)).ToString();
            timerText = timerText.Substring(0, 8);
            newTiming.TimerTextBlock.Text = timerText;

            newTiming.StationTimingDg.ItemsSource = listTest;
        
        }

        private void Timing_DoWork(object sender, DoWorkEventArgs e)
        {
            station = e.Argument as BusStation;
            try
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    newTiming = new LineTimingWindow(this) { Title = " לוח זמנים של תחנה " + station.BusStationKey 
                    + station.StationName };
                    newTiming.Show();
                });
                

                startHour = DateTime.Now.TimeOfDay;
                while (timing.CancellationPending == false)
                {
                    TimeSpan simulatedHourNow = startHour + TimeSpan.FromTicks(stopwatch.Elapsed.Ticks * 60);
                    listTest = pl.BoPoLineTimingAdapter(bl.StationTiming(station, simulatedHourNow), simulatedHourNow);
                    timing.ReportProgress(1);
                    Thread.Sleep(1);
                }
                e.Result = 1;
            }
            catch (InexistantLineTripException ex)
            {
                MessageBox.Show(ex.Message, "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region BusStation
        private void BusStationsDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusStation selectedStation = BusStationsDg.SelectedItem as BusStation;
            
            if (selectedStation != null)
            {
                IEnumerable<string> LinesInStation = from lineId in selectedStation.LinesThatPass
                                                     let line = bl.GetBusLine(lineId)
                                                     select (" קו " + line.BusLineNumber + " : לכיוון " + bl.GetBusStation(line.LastStationKey).StationName);

                LinesPassListBox.ItemsSource = LinesInStation;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BusStation selectedStation = BusStationsDg.SelectedItem as BusStation;
                bl.DeleteStation(selectedStation.BusStationKey);
                MessageBox.Show("התחנה נמחקה בהצלחה");
                BusStationsDg.ItemsSource = bl.GetAllBusStations();
            }
            catch (InexistantStationException ex)
            {
                MessageBox.Show(ex.Message, "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateStationWindow updateStationWindow = new UpdateStationWindow { DataContext = BusStationsDg.SelectedItem };
            updateStationWindow.Show();
            //Close();
        }

        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            AddStationWindow newWindow = new AddStationWindow();
            newWindow.Show();
            Close();
        }

        private void ViewArrivals_Click(object sender, RoutedEventArgs e)
        {
            //threading
            stopwatch = new Stopwatch();
            timing = new BackgroundWorker();
            timing.DoWork += Timing_DoWork;
            timing.ProgressChanged += Timing_ProgressChanged;
            timing.RunWorkerCompleted += Timing_RunWorkerCompleted;

            timing.WorkerReportsProgress = true;
            timing.WorkerSupportsCancellation = true;

            stopwatch.Restart();
            timing.RunWorkerAsync(BusStationsDg.SelectedItem);
            
        }

        public void CancelThread()
        {
            timing.CancelAsync();
        }
        #endregion

        #region BusLine
        private void BusLinesDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showStationsForLine(BusLinesDg.SelectedItem as BusLine);
        }

        private void DeleteLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BusLine selectedStation = BusLinesDg.SelectedItem as BusLine;
                bl.DeleteBusLine(selectedStation);
                MessageBox.Show("הקו נמחק בהצלחה");
                BusLinesDg.ItemsSource = bl.GetAllBusLines();
            }
            catch (InexistantLineException ex)
            {
                MessageBox.Show(ex.Message, "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateLine_Click(object sender, RoutedEventArgs e)
        {
            UpdateBusLineWindow updateLineWindow = new UpdateBusLineWindow { DataContext = BusLinesDg.SelectedItem };
            updateLineWindow.Show();
            //Close();
        }

        private void AddLineButton_Click(object sender, RoutedEventArgs e)
        {
            AddBusLineWindow newWindow = new AddBusLineWindow();
            newWindow.Show();
            Close();
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            showStationsForLine(BusLinesDg.SelectedItem as BusLine);
        }

        private void showStationsForLine(BusLine line)
        {
            if (line != null)
            {
                List<string> StationsInLine = new List<string>();
                foreach (int item in line.AllStationsOfLine)
                    StationsInLine.Add("תחנה" + item + " : " + bl.GetBusStation(item).StationName);
                StationsListBox.ItemsSource = StationsInLine;
            }
        }

        #endregion

        
    }
}
