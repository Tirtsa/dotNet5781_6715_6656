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
using System.ComponentModel;
using System.Diagnostics;

namespace dotNet5781_03B_6715_5227
{
    /// <summary>
    /// Logique d'interaction pour BusDetailsWindow.xaml
    /// </summary>
    public partial class BusDetailsWindow : Window
    {
        BackgroundWorker maintenance;
        BackgroundWorker refueling;
        Bus myBus;
        public BusDetailsWindow()
        {
            InitializeComponent();

            maintenance = new BackgroundWorker();
            maintenance.DoWork += maintenance_DoWork;
            maintenance.ProgressChanged += maintenance_ProgressChanged;
            maintenance.RunWorkerCompleted += maintenance_RunWorkerCompleted;

            maintenance.WorkerReportsProgress = true;
            maintenance.WorkerSupportsCancellation = true;

            
        }

        private void maintenance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myBus.BusStatus = "מוכן לנסיעה";
            if (e.Error != null)
                MessageBox.Show("Error : " + e.Error.Message);
            else
                MessageBox.Show("הטיפול של אוטובוס " + myBus.Immatriculation + " הסתיים");
        }

        private void maintenance_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void maintenance_DoWork(object sender, DoWorkEventArgs e)
        {
            if(myBus.BusStatus != "באמצע נסיעה")
            {
                myBus.BusStatus = "בטיפול";

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                for (int i = 0; i < 144; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                }
                if (myBus.KmOfFuel < 10)
                    myBus.KmOfFuel += 900;
            }
            else
            {
                MessageBox.Show("אוטובוס " + myBus.Immatriculation + "נמצא בנסיעה. נא לחכות לסופה");
                maintenance.CancelAsync();
            }
        }

        private void Refueling_Click(object sender, RoutedEventArgs e)
        {
            refueling = new BackgroundWorker();
            refueling.DoWork += Refueling_DoWork;
            refueling.ProgressChanged += Refueling_ProgressChanged;
            refueling.RunWorkerCompleted += Refueling_RunWorkerCompleted;

            refueling.WorkerReportsProgress = true;
            refueling.WorkerSupportsCancellation = true;

            myBus = (Bus)(sender as Button).DataContext;
            refueling.RunWorkerAsync();
            refresh();
        }
        private void Refueling_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            myBus.BusStatus = "מוכן לנסיעה";
            if (e.Error != null)
                MessageBox.Show("Error : " + e.Error.Message);
            else
            {
                //resultLabel.Content = (string)e.Result + "%";
                //resultProgressBar.Value = (double)e.Result;
                MessageBox.Show("התידלוק של אוטובוס " + myBus.Immatriculation + " הסתיים");
                refresh();
            }
        }

        private void Refueling_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            resultLabel.Content = progress + "%";
            resultProgressBar.Value = progress;

        }

        private void Refueling_DoWork(object sender, DoWorkEventArgs e)
        {
            if (myBus.BusStatus != "באמצע נסיעה")
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                myBus.KmOfFuel += 900;
                myBus.BusStatus = "בתידלוק";
                refresh();
                //Application.Current.Dispatcher.Invoke(new Action(() =>
                //{
                //    resultProgressBar.Visibility = Visibility.Visible;
                //    resultLabel.Visibility = Visibility.Visible;
                //}));
                
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

        private void Maintenance_Click(object sender, RoutedEventArgs e)
        {
            if (maintenance.IsBusy != true)
            {
                myBus = (Bus)this.DataContext;

                maintenance.RunWorkerAsync();
                refresh();
            }
        }

        private void refresh()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                BusDetailsWindow busDetails = new BusDetailsWindow { DataContext = this.DataContext };
                busDetails.Show();
                this.Close();
            }));
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
