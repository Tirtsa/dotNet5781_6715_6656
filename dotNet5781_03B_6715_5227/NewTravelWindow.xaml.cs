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

namespace dotNet5781_03B_6715_5227
{
    /// <summary>
    /// Logique d'interaction pour NewTravelWindow.xaml
    /// </summary>
    public partial class NewTravelWindow : Window
    {
        BackgroundWorker travelling;
        Bus myBus;
        TravelInProgress newTravel;
        MainWindow main;
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
            else if (travelling.CancellationPending == false)
            {
                //float result = (float)e.Result;
                double result = Convert.ToDouble(e.Result);
                int day = (int)(result / 144);
                int hour = (int)(result - day * 6)/6;
                int minutes = (int) ((result - day * 144.0 - hour * 6.0) / 6.0 * 60);


                //if(day != 0)
                //    MessageBox.Show("נסיעתו של אוטובוס " + myBus.Immatriculation + " הסתיימה - היא ארכה " + 
                //    day + "ימים" + hour + " שעות ו " + minutes + "דקות");
                //else
                MessageBox.Show("נסיעתו של אוטובוס " + myBus.Immatriculation + " הסתיימה - היא ארכה " +
                hour + " שעות ו " + minutes + "דקות ");
                
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    newTravel.Close();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    main.Close();
                });
                
            }


        }

        private void Travelling_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            if (Application.Current.Dispatcher.CheckAccess())
            {
                newTravel.travelProgressLabel.Content = progress + "%";
                newTravel.travelProgressBar.Value = progress;
            }
            else
                Dispatcher.BeginInvoke(new Action(() => { Travelling_ProgressChanged(sender, e); }));
        }

        private void Travelling_DoWork(object sender, DoWorkEventArgs e)
        {
            
            
            int kmTravel = (int)e.Argument;
            Random rndVitesse = new Random(DateTime.Now.Millisecond);
            try
            {
                myBus.BusStatus = "באמצע נסיעה";
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    this.Close();
                });

                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    newTravel = new TravelInProgress { Title = "נסיעתו של אוטובוס " + myBus.Immatriculation };
                    newTravel.Show();
                });

                int vitesse = rndVitesse.Next(20, 50);
                double totalTime = 6 * ((double)kmTravel / vitesse);
                for (int i = 0; i <= totalTime; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    travelling.ReportProgress((int)(i * 100 / totalTime));
                }
                myBus.BusStatus = "מוכן לנסיעה";
                myBus.addTravel(kmTravel);

                e.Result = totalTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                travelling.CancelAsync();
            }
        }

        private void AddTravelButton_Click(object sender, RoutedEventArgs e)
        {
            int travelKm;
            int.TryParse(travelKmTextBox.Text, out travelKm);
            myBus = (Bus)this.DataContext;
            travelling.RunWorkerAsync(travelKm);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            main = new MainWindow();
            main.Show();
        }
    }
}
