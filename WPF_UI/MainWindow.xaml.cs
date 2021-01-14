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

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static IBL bl;
        //private ObservableCollection<BusStation> AllStations ;
		public MainWindow()
		{
			bl = BlFactory.GetBL();
			InitializeComponent();

            //IEnumerable<BusStation> stations = bl.GetAllBusStations();
            //AllStations = new ObservableCollection<BusStation>(stations.Cast<BusStation>());
            //BusStationsDg.ItemsSource = AllStations;
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
                MessageBox.Show("התחנה נמחקה בהצלחה");
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
            updateStationWindow.Show();
            Close();
        }

        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            AddStationWindow newWindow = new AddStationWindow();
            newWindow.Show();
            Close();
        }

        private void BusList_Click(object sender, RoutedEventArgs e)
		{
            DataGrid tempDG = (DataGrid)sender;
            BusStation tempS = (BusStation)tempDG.SelectedItem;
            int key = tempS.BusStationKey;
            List<BusLine> lines = new List<BusLine>();

            DisplayBusLinesWindow newWindow = new DisplayBusLinesWindow { DataContext = tempS };
            foreach (BusLine line in bl.GetAllBusLines())
                foreach (int stop in line.AllStationsOfLine)
                    if (stop == key)
                        lines.Add(line);

            newWindow.lbBusLines.ItemsSource = lines;
            newWindow.Show();
        }

        private void Refresh()
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
            Close();
        }
    }
}
