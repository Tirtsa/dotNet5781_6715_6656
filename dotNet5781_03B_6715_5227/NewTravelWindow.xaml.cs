using System;
using System.ComponentModel;
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
using System.Diagnostics;

namespace dotNet5781_03B_6715_5227
{
    /// <summary>
    /// Logique d'interaction pour NewTravelWindow.xaml
    /// </summary>
    public partial class NewTravelWindow : Window
    {
        BackgroundWorker travelling;
        Bus myBus;
        public NewTravelWindow()
        {
            InitializeComponent();

            travelling = new BackgroundWorker();
            travelling.DoWork += Travelling_DoWork;
            travelling.ProgressChanged += Travelling_ProgressChanged;
            travelling.RunWorkerCompleted += Travelling_RunWorkerCompleted;

            travelling.WorkerReportsProgress = true;
            travelling.WorkerSupportsCancellation = true;
        }

        private void Travelling_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show("Error : " + e.Error.Message);
            else if (travelling.CancellationPending == true)
            {

                float result = (float)e.Result;
                //int day = (int)(result / 1000) % 144;
                //int hour = (int)(result / 1000 - day*144) % 6;
                //int minutes = (int)((result / 1000) - day*144 - hour*6)/6*60 ;
                int hour = (int)result / 6;
                int minutes = (int)(result - hour) / 6 * 60;

                //if(day != 0)
                //    MessageBox.Show("נסיעתו של אוטובוס " + myBus.Immatriculation + " הסתיימה - היא ארכה " + 
                //    day + "ימים" + hour + " שעות ו " + minutes + "דקות");
                //else
                MessageBox.Show("נסיעתו של אוטובוס " + myBus.Immatriculation + " הסתיימה - היא ארכה " +
                hour + " שעות ו " + minutes + "דקות");
            }


        }

        private void Travelling_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            if (Application.Current.Dispatcher.CheckAccess())
                MessageBox.Show(progress + "%");
            else
                Dispatcher.BeginInvoke(new Action(() => { Travelling_ProgressChanged(sender, e); }));
        }

        private void Travelling_DoWork(object sender, DoWorkEventArgs e)
        {
            int kmTravel = (int)e.Argument;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Random rndVitesse = new Random(DateTime.Now.Millisecond);
            try
            {
                myBus.BusStatus = "באמצע נסיעה";

                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    MainWindow mainWindow = new MainWindow();
                    //mainWindow.Show();
                    this.Close();
                });

                int vitesse = rndVitesse.Next(20, 50);
                float totalTime = 6 * (kmTravel / vitesse);
                for (int i = 0; i <= totalTime; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    travelling.ReportProgress((int)(i * 100 / totalTime));
                }
                myBus.BusStatus = "מוכן לנסיעה";
                myBus.addTravel(kmTravel);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                travelling.CancelAsync();
            }
            e.Result = stopwatch.ElapsedMilliseconds;
        }

        private void AddTravelButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                if (travelling.IsBusy != true)
                {
                    int travelKm;
                    int.TryParse(travelKmTextBox.Text, out travelKm);
                    myBus = (Bus)this.DataContext;
                    travelling.RunWorkerAsync(travelKm);
                }
            });
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
