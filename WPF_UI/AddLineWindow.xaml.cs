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
            
            busLine = new BusLine();
            DataContext = busLine;

            cbFirstStop.ItemsSource = bl.GetAllBusStations();
            cbLastStop.ItemsSource = bl.GetAllBusStations();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AllStationsListBox.ItemsSource = from station in bl.GetAllBusStations()
                                             select (" תחנה " + station.BusStationKey + " : " + station.StationName);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string currentItemText = AllStationsListBox.SelectedValue.ToString();
            LineStationsListBox.Items.Add(currentItemText);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int currentItemIndex = LineStationsListBox.SelectedIndex;
            LineStationsListBox.Items.RemoveAt(currentItemIndex);
        }
        private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
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
                    busLine.AllStationsOfLine = from string item in LineStationsListBox.Items
                                                select int.Parse(item.Substring(6, 5));
                    bl.AddBusLine(busLine);
                    busLine = new BusLine();
                    DataContext = busLine;
                    LineStationsListBox.Items.Clear();

                    //BusStation addStopField = (BusStation)cbAddStop.SelectedItem;
                    //bool found = false;
                    //foreach (int stop in stations)
                    //    if (stop == addStopField.BusStationKey)
                    //        found = true;
                    //if (!found)
                    //    stations.Add(addStopField.BusStationKey);

                    //BusStation last = (BusStation)cbLastStop.SelectedValue;
                    //stations.Add(last.BusStationKey); BusStation first = (BusStation)cbFirstStop.SelectedValue;

                    //stations.Prepend(first.BusStationKey);

                    //busLine = new BusLine {
                    //    BusLineNumber = int.Parse(tbLineNumber.Text),
                    //    Area = (BO.Areas)cbArea.SelectedItem,
                    //    FirstStationKey = first.BusStationKey,
                    //    LastStationKey = last.BusStationKey,
                    //    AllStationsOfLine = stations
                    //};
                    //bl.AddBusLine(busLine);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error thrown: " + ex);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
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

