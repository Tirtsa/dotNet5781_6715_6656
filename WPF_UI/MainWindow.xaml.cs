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

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static IBL bl;
		public MainWindow()
		{
			bl = BlFactory.GetBL();
			InitializeComponent();
			BusStationsDg.ItemsSource = bl.GetAllBusStations();
		}

        private void BusStationsDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			BusStation selectedStation = BusStationsDg.SelectedItem as BusStation;
			LinesPassListBox.ItemsSource = selectedStation.LinesThatPass;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BusStation selectedStation = BusStationsDg.SelectedItem as BusStation;
                bl.DeleteStation(selectedStation.BusStationKey);
                //BusStationsDg.ItemsSource = bl.GetAllBusStations();

                Refresh();
            }
			catch (Exception ex)
            {
                MessageBox.Show("An error occured " + ex);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateStationWindow updateStationWindow = new UpdateStationWindow { DataContext = BusStationsDg.SelectedItem };
            updateStationWindow.ShowDialog();
        }

        private void Refresh()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MainWindow newWindow = new MainWindow();
                newWindow.Show();
                Close();
            }));

        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
