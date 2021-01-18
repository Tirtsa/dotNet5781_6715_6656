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
            BusLinesDg.ItemsSource = bl.GetAllBusLines();
        }

        private void BusStationsDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			BusStation selectedStation = BusStationsDg.SelectedItem as BusStation;
            //List<string> LinesInStation = new List<string>();
            //foreach(int item in selectedStation.LinesThatPass)
            //{
            //    BusLine line = bl.GetBusLine(item);
            //    LinesInStation.Add("קו " + line.BusLineNumber + "לכיוון " + bl.GetBusStation(line.LastStationKey).StationName);
            //}
            IEnumerable<string> LinesInStation = from lineId in selectedStation.LinesThatPass
                                                 let line = bl.GetBusLine(lineId)
                                                 select (" קו " + line.BusLineNumber + " : לכיוון " + bl.GetBusStation(line.LastStationKey).StationName);

            LinesPassListBox.ItemsSource = LinesInStation;
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

        private void UpdateLineButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateLineWindow updateLineWindow = new UpdateLineWindow();// { DataContext = lbBusLines.SelectedItem };
            //busLine
            updateLineWindow.Show();
        }

        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            AddStationWindow newWindow = new AddStationWindow();
            newWindow.Show();
            Close();
        }
        private void AddLineButton_Click(object sender, RoutedEventArgs e)
        {
            AddBusLine newWindow = new AddBusLine();
            newWindow.Show();
        }

        private void BusList_Click(object sender, RoutedEventArgs e)
        {
            DataGrid tempDG = (DataGrid)sender;
            BusStation tempS = (BusStation)tempDG.SelectedItem;
            int key = tempS.BusStationKey;

            DisplayBusLinesWindow newWindow = new DisplayBusLinesWindow { DataContext = bl.GetBusStation(tempS.BusStationKey) };

            List<BusLine> lines = new List<BusLine>();
            foreach (BusLine line in bl.GetAllBusLines())
                foreach (int stop in line.AllStationsOfLine)
                    if (stop == key)
                    {
                        lines.Add(line);
                        break;
                    }

            newWindow.lbBusLines.ItemsSource = lines;
            newWindow.Show();
        }

        private void Refresh()
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
            Close();
        }

        private void BusLinesDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusLine selectedStation = BusLinesDg.SelectedItem as BusLine;
            List<string> StationsInLine = new List<string>();
            foreach (int item in selectedStation.AllStationsOfLine)
                StationsInLine.Add("תחנה" + item + " : " + bl.GetBusStation(item).StationName  );
            StationsListBox.ItemsSource = StationsInLine;
        }

        private void DeleteLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BusLine selectedStation = BusLinesDg.SelectedItem as BusLine;
                bl.DeleteBusLine(selectedStation);
                MessageBox.Show("הקו נמחק בהצלחה");
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured " + ex);
            }
        }

        private void UpdateLine_Click(object sender, RoutedEventArgs e)
        {
            UpdateBusLineWindow updateBusLineWindow = new UpdateBusLineWindow { DataContext = BusLinesDg.SelectedItem };
            updateBusLineWindow.Show();
            Close();
        }
    }
}
