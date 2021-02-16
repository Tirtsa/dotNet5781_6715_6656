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
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using BO;
using BLApi;
using PO;

namespace WPF_UI
{
    /// <summary>
    /// Logique d'interaction pour LineTiming.xaml
    /// </summary>
    public partial class LineTimingWindow : Window
    {
        static IBL bl = BlFactory.GetBL();
        static PL pl = new PL();

        BusStation station;
        IEnumerable<IGrouping<TimeSpan, PO.LineTiming>> listTest;
        TimeSpan startHour;

        private Stopwatch stopwatch;
        BackgroundWorker timing;

        public LineTimingWindow(BusStation station)
        {
            InitializeComponent();
            Title = " לוח זמנים של תחנה " + station.BusStationKey + " - " + station.StationName;

            stopwatch = new Stopwatch();
            timing = new BackgroundWorker();
            timing.DoWork += Timing_DoWork;
            timing.ProgressChanged += Timing_ProgressChanged;
            timing.RunWorkerCompleted += Timing_RunWorkerCompleted;

            timing.WorkerReportsProgress = true;
            timing.WorkerSupportsCancellation = true;

            stopwatch.Restart();
            timing.RunWorkerAsync(station);
        }

        private void Timing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Timing_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string timerText = (startHour + TimeSpan.FromTicks(stopwatch.Elapsed.Ticks * 60)).ToString();
            timerText = timerText.Substring(0, 8);
            TimerTextBlock.Text = timerText;

            StationTimingDg.ItemsSource = listTest;

        }

        private void Timing_DoWork(object sender, DoWorkEventArgs e)
        {
            station = e.Argument as BusStation;
            try
            { 
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



        private void Window_Closing(object sender, CancelEventArgs e)
        {
            timing.CancelAsync();
        }
    }
}
