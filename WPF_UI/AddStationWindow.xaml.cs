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
using BLApi;
using BO;

namespace WPF_UI
{
    /// <summary>
    /// Logique d'interaction pour AddStationWindow.xaml
    /// </summary>
    public partial class AddStationWindow : Window
    {
        static IBL bl = BlFactory.GetBL();

        BusStation station;
        public AddStationWindow()
        {
            InitializeComponent();
            station = new BusStation();
            DataContext = station;
        }

        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddStation(station);

                MessageBox.Show("התחנה נוספה בהצלחה");
                //App.Current.MainWindow.DataContext = bl.GetAllBusStations();
                Close();
            }
            catch (BO.DuplicateStationException ex)
            {
                MessageBox.Show(ex.Message, "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
