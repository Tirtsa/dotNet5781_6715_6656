using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for UpdateLineWindow.xaml
	/// </summary>
	public partial class UpdateLineWindow : Window
	{
		static IBL bl;
        public BusLine busLine;

        ObservableCollection<int> stations = new ObservableCollection<int>();
		public UpdateLineWindow()
		{
			bl = BlFactory.GetBL();
			InitializeComponent();
			cbFirstStop.ItemsSource = bl.GetAllBusStations();
			cbLastStop.ItemsSource = bl.GetAllBusStations();
			cbAddStop.ItemsSource = bl.GetAllBusStations();
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            busLine = DataContext as BusLine;
			tbArea.Text = Enum.GetName(typeof(BO.Areas), busLine.Area);
			tbLineNumber.Text = busLine.BusLineNumber.ToString();

            cbFirstStop.SelectedItem = bl.GetAllBusStations().Where(s => s.BusStationKey == busLine.FirstStationKey);
            cbLastStop.SelectedValue = bl.GetAllBusStations().Where(s => s.BusStationKey == busLine.LastStationKey);
        }

            private void AddStationButton_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                if ((BusStation)cbAddStop.SelectedItem == null)
                    throw new ArgumentNullException();
                BusStation stop = (BusStation)cbAddStop.SelectedItem;

                foreach (int stopKey in busLine.AllStationsOfLine)
                    if (stopKey == stop.BusStationKey)
                        return;

                stations.Add(stop.BusStationKey);
            }
            catch (Exception)
            {
                MessageBox.Show("You have not selected a station to add");
            }
		}

		private void UpdateLineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LineStation first = (LineStation)cbFirstStop.SelectedValue;
                LineStation last = (LineStation)cbLastStop.SelectedValue;

                //first and last
                if (busLine.AllStationsOfLine.First() != first.StationKey)
                    busLine.AllStationsOfLine.Prepend(first.StationKey);

                else if (busLine.AllStationsOfLine.Last() != last.StationKey)
                    busLine.AllStationsOfLine.Append(last.StationKey);

                BusLine tempLine = new BusLine {
                    FirstStationKey = first.StationKey,
                    LastStationKey = last.StationKey,
                    AllStationsOfLine = stations
                };
                bl.UpdateBusLine(tempLine);
            }
            catch (Exception)
            {
                MessageBox.Show("You have not filled in all the fields");
            }
}
	}
}
