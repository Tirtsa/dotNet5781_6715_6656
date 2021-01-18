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
	/// Interaction logic for AddLineWindow.xaml
	/// </summary>
	public partial class AddLineWindow : Window
	{
		static IBL bl;
		static BusLine busLine;
		IEnumerable<int> stations = Enumerable.Empty<int>();
		public AddLineWindow()
		{
			bl = BlFactory.GetBL();
			InitializeComponent();
			//cbArea.ItemsSource = bl.GetAreas();
			cbFirstStop.ItemsSource = bl.GetAllBusStations();
			cbLastStop.ItemsSource = bl.GetAllBusStations();
			cbAddStop.ItemsSource = bl.GetAllBusStations();
		}

        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            if (cbAddStop.SelectedIndex == -1)
            {
                MessageBox.Show("No station was selected!");
                return; 
            }

            BusStation stop = (BusStation)cbAddStop.SelectedItem;
            
            if (busLine == null)
            {
                stations.Append(stop.BusStationKey);
                MessageBox.Show("Station number: " + stop.BusStationKey.ToString() + " was successfully added.");
                return;
            }

            foreach (int stopKey in busLine.AllStationsOfLine)
                if (stopKey == stop.BusStationKey)
                {
                    MessageBox.Show("This station is already on the bus line");
                    return;
                }

			stations.Append(stop.BusStationKey);
		}

		private void tbLineNumber_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			int key = (int)e.Key;
			e.Handled = !(key > 33 && key < 44 || key > 73 && key < 84 || key == 2);
		}

		private void AddLineButton_Click(object sender, RoutedEventArgs e)
		{
			if (tbLineNumber.Text.Length == 0 || cbArea.SelectedIndex == -1 || cbFirstStop.SelectedIndex == -1 || cbLastStop.SelectedIndex == -1)
				MessageBox.Show("Not all fields are filled in");
			else
			{
				LineStation first = (LineStation)cbFirstStop.SelectedValue;
				stations.Prepend(first.StationKey);
				LineStation last = (LineStation)cbLastStop.SelectedValue;
				stations.Append(last.StationKey);

				busLine = new BusLine
				{
					BusLineNumber = int.Parse(tbLineNumber.Text),
					Area = (Areas)cbArea.SelectedItem,
					FirstStationKey = first.StationKey,
					LastStationKey = last.StationKey,
					AllStationsOfLine = stations
				};
				bl.AddBusLine(busLine);
			}
		}
	}
}

