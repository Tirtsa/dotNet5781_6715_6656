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
    /// Logique d'interaction pour UpdateBusLineWindow.xaml
    /// </summary>
    public partial class UpdateBusLineWindow : Window
    {
        static IBL bl;
        BusLine myLine;
        LineTrip newLineTrip;
        List<LineTrip> listLineTrips;
        public UpdateBusLineWindow()
        {
            bl = BlFactory.GetBL();
            InitializeComponent();

            areaComboBox.ItemsSource = Enum.GetValues(typeof(Areas));

            newLineTrip = new LineTrip();
            listLineTrips = new List<LineTrip>();
            addLineTripGrid.DataContext = newLineTrip;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myLine = DataContext as BusLine;

            List<string> myStationsList = (from key in (DataContext as BusLine).AllStationsOfLine
                                           let station = bl.GetBusStation(key)
                                           select (" תחנה " + station.BusStationKey + " : " + station.StationName)).ToList();
            foreach(string item in myStationsList)
                LineStationsListBox.Items.Add(item);
            AllStationsListBox.ItemsSource = from station in bl.GetAllBusStations()
                                             select (" תחנה " + station.BusStationKey + " : " + station.StationName);
            List<LineTrip> myLineTripList = (from lineTrip in (DataContext as BusLine).AllLineTripsOfLine
                                            select lineTrip).ToList();
            foreach (LineTrip item in myLineTripList)
                lineTripDataGrid.Items.Add(item);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string currentItemText = AllStationsListBox.SelectedValue.ToString();
            if (LineStationsListBox.SelectedItem == null)
                LineStationsListBox.Items.Add(currentItemText);
            else
                LineStationsListBox.Items.Insert(LineStationsListBox.SelectedIndex, currentItemText);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int currentItemIndex = LineStationsListBox.SelectedIndex;
            LineStationsListBox.Items.RemoveAt(currentItemIndex);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myLine.AllStationsOfLine = from string item in LineStationsListBox.Items
                                           select int.Parse(item.Substring(6, 5));
                myLine.AllLineTripsOfLine = from LineTrip item in lineTripDataGrid.Items
                                            select item as LineTrip;
                bl.UpdateBusLine(myLine);
                App.Current.MainWindow.DataContext = bl.GetAllBusLines();
                Close();
            }
            catch (InexistantLineException ex)
            {
                MessageBox.Show(ex.Message, "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addLineTripButton_Click(object sender, RoutedEventArgs e)
        {
            lineTripDataGrid.Items.Add(addLineTripGrid.DataContext as LineTrip);

            //listLineTrips.Add(addLineTripGrid.DataContext as LineTrip);
            //lineTripDataGrid.ItemsSource = from l in listLineTrips select l;
            newLineTrip = new LineTrip();
            addLineTripGrid.DataContext = newLineTrip;
        }

        private void deleteLineTrip_Click(object sender, RoutedEventArgs e)
        {
            int currentItemIndex = lineTripDataGrid.SelectedIndex;
            lineTripDataGrid.Items.RemoveAt(currentItemIndex);
            //listLineTrips.Remove(lineTripDataGrid.SelectedItem as LineTrip);
            //lineTripDataGrid.ItemsSource = from l in listLineTrips select l;
        }
    }
}
