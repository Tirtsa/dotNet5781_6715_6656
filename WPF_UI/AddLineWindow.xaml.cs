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
using System.Collections.ObjectModel;

namespace WPF_UI
{
	/// <summary>
	/// Interaction logic for AddLineWindow.xaml
	/// </summary>
	public partial class AddLineWindow : Window
	{
		static IBL bl;
		static BusLine busLine;
        static ObservableCollection<int> stations = new ObservableCollection<int>();
        public AddLineWindow()
		{
			bl = BlFactory.GetBL();
			InitializeComponent();
            cbArea.ItemsSource = Enum.GetValues(typeof(BO.Areas));
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
                stations.Add(stop.BusStationKey);
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
            try
            {
                if (tbLineNumber.Text.Length == 0 || cbArea.SelectedIndex == -1 || cbFirstStop.SelectedIndex == -1 || cbLastStop.SelectedIndex == -1)
                    throw new Exception();
                else
                {
                    BusStation addStopField = (BusStation)cbAddStop.SelectedItem;
                    bool found = false;
                    foreach (int stop in stations)
                        if (stop == addStopField.BusStationKey)
                            found = true;
                    if (!found)
                        stations.Add(addStopField.BusStationKey);

                    BusStation last = (BusStation)cbLastStop.SelectedValue;
                    stations.Add(last.BusStationKey); BusStation first = (BusStation)cbFirstStop.SelectedValue;

                    stations.Prepend(first.BusStationKey);

                    busLine = new BusLine {
                        BusLineNumber = int.Parse(tbLineNumber.Text),
                        Area = (BO.Areas)cbArea.SelectedItem,
                        FirstStationKey = first.BusStationKey,
                        LastStationKey = last.BusStationKey,
                        AllStationsOfLine = stations
                    };
                    bl.AddBusLine(busLine);
                    Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Not all fields are filled in");
            }

        }
	}
}

