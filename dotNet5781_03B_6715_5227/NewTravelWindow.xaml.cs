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
            //Bus test = DataContext as Bus;
        }

        private void Travelling_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show("Error : " + e.Error.Message);
            else
            {
                long result = (long)e.Result;
                if (result < 1000)
                    MessageBox.Show("Travelling of Bus " + myBus.Immatriculation + " finished - during " + result + " ms.");
                else
                    MessageBox.Show("Travelling of Bus "+ myBus.Immatriculation +" finished - during " + result / 1000 + " sec.");
            }
                
        }

        private void Travelling_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            
        }

        private void Travelling_DoWork(object sender, DoWorkEventArgs e)
        {
            //myBus = DataContext as Bus;
            //int kmTravel = int.Parse(this.travelKmTextBox.Text);
            int kmTravel = (int)e.Argument;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Random rndVitesse = new Random(DateTime.Now.Millisecond);
            try
            {
                (this.DataContext as Bus).addTravel(kmTravel);
                int vitesse = rndVitesse.Next(20, 50);
                int totalTime = 6 * (kmTravel / vitesse);
                for (int i = 0; i <= totalTime ; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    travelling.ReportProgress(i / totalTime * 100);
                }
                e.Result = stopwatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddTravelButton_Click(object sender, RoutedEventArgs e)
        {
            if (travelling.IsBusy != true)
            {
                int travelKm;
                int.TryParse(travelKmTextBox.Text, out travelKm);
                travelling.RunWorkerAsync(12);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
