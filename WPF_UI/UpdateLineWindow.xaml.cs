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
		public UpdateLineWindow()
		{
			bl = BlFactory.GetBL();
			InitializeComponent();
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AllStationsListBox.SelectedIndex = 0;
            LineStationsListBox.SelectedIndex = 0;
            busLine = DataContext as BusLine;

            tbArea.Text = Enum.GetName(typeof(BO.Areas), busLine.Area);
			tbLineNumber.Text = busLine.BusLineNumber.ToString();

            tbTotalDistance.Text = busLine.TotalDistance.ToString();
            tbTotalTime.Text = busLine.TotalTime.ToString();

            List<string> myStationsList = (from key in (DataContext as BusLine).AllStationsOfLine
                                           let station = bl.GetBusStation(key)
                                           select (" תחנה " + station.BusStationKey + " : " + station.StationName)).ToList();
            foreach (string item in myStationsList)
            {
                LineStationsListBox.Items.Add(item);
            }
            AllStationsListBox.ItemsSource = from station in bl.GetAllBusStations()
                                             select (" תחנה " + station.BusStationKey + " : " + station.StationName);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string currentItemText = AllStationsListBox.SelectedValue.ToString();
            if (!LineStationsListBox.Items.Cast<String>().Any(s => s.ToString() == currentItemText))
                LineStationsListBox.Items.Add(currentItemText);
            else
                MessageBox.Show("תחנה זו כבר ברשימה");
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int currentItemIndex = LineStationsListBox.SelectedIndex;
            LineStationsListBox.Items.RemoveAt(currentItemIndex);
        }

        private void UpdateLineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {//move up and down
                busLine.TotalDistance = double.Parse(tbTotalDistance.Text);
                busLine.TotalTime = double.Parse(tbTotalTime.Text);

                string test;
                foreach (string item in LineStationsListBox.Items)
                    test = item.Substring(6, 5);
                busLine.AllStationsOfLine = from string item in LineStationsListBox.Items
                                            select int.Parse(item.Substring(6, 5));
                
                LineStationsListBox.SelectedIndex = 0;
                BusStation selected = LineStationsListBox.SelectedItem as BusStation;
                busLine.FirstStationKey = selected.BusStationKey;
                
                LineStationsListBox.SelectedIndex = LineStationsListBox.Items.Count - 1;
                selected = LineStationsListBox.SelectedItem as BusStation;
                busLine.LastStationKey = selected.BusStationKey;

                //if (busLine.AllStationsOfLine.First() != first.StationKey)
                //    busLine.AllStationsOfLine.Prepend(first.StationKey);

                //else if (busLine.AllStationsOfLine.Last() != last.StationKey)
                //    busLine.AllStationsOfLine.Append(last.StationKey);

                bl.UpdateBusLine(busLine);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error thrown: " + ex);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
