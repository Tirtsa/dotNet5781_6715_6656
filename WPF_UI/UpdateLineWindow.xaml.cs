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
	/// Interaction logic for UpdateLineWindow.xaml
	/// </summary>
	public partial class UpdateLineWindow : Window
	{
		static IBL bl;
        public BusLine busLine;

        IEnumerable<int> stations;
		public UpdateLineWindow()
		{
			bl = BlFactory.GetBL();
			InitializeComponent();
            BusLine busLine = DataContext as BusLine;
			tbArea.Text = busLine.Area.ToString();
			tbLineNumber.Text = busLine.BusLineNumber.ToString();
			cbFirstStop.ItemsSource = bl.GetAllLineStations();
			cbLastStop.ItemsSource = bl.GetAllLineStations();
			cbAddStop.ItemsSource = bl.GetAllBusStations();
			stations = busLine.AllStationsOfLine;
		}

		private void cbFirstStop_Initialized(object sender, EventArgs e)
		{
			cbFirstStop.SelectedItem = busLine.FirstStationKey;
		}

		private void cbLastStop_Initialized(object sender, EventArgs e)
		{
			cbLastStop.SelectedItem = busLine.LastStationKey;
		}

		private void AddStationButton_Click(object sender, RoutedEventArgs e)
		{
			BusStation stop = (BusStation)cbAddStop.SelectedItem;
			//BusStation stop = (BusStation)cbAddStop.SelectedIValue;

			foreach (int stopKey in busLine.AllStationsOfLine)
				if (stopKey == stop.BusStationKey)
					return;

			stations.Append(stop.BusStationKey);
		}

		private void UpdateLineButton_Click(object sender, RoutedEventArgs e)
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
				AllStationsOfLine = stations,
				//TotalDistance = ,
				//TotalTime = 
			};
			bl.UpdateBusLine(tempLine);
		}
	}
}
