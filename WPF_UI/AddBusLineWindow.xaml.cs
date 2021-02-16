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
    /// Logique d'interaction pour AddBusLine.xaml
    /// </summary>
    public partial class AddBusLineWindow : Window
    {
        static IBL bl;
        BusLine myLine;
        LineTrip newLineTrip;
        List<LineTrip> listLineTrips;
        public AddBusLineWindow()
        {
            bl = BlFactory.GetBL();
            InitializeComponent();

            myLine = new BusLine();
            lineDetailsGrid.DataContext = myLine;
            newLineTrip = new LineTrip();
            addLineTripGrid.DataContext = newLineTrip;
            listLineTrips = new List<LineTrip>();

            areaComboBox.ItemsSource = Enum.GetValues(typeof(Areas));
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AllStationsListBox.ItemsSource = from station in bl.GetAllBusStations()
                                             select (" תחנה " + station.BusStationKey + " : " + station.StationName);
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

        private void AddLineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myLine.AllStationsOfLine = from string item in LineStationsListBox.Items
                                           select int.Parse(item.Substring(6,5));
                myLine.FirstStationKey = myLine.AllStationsOfLine.First();
                myLine.LastStationKey = myLine.AllStationsOfLine.Last();
                myLine.AllLineTripsOfLine = listLineTrips;
                bl.AddBusLine(myLine);
                
                myLine = new BusLine();
                lineDetailsGrid.DataContext = myLine;
                listLineTrips.Clear();
                lineTripDataGrid.ItemsSource = listLineTrips;
                LineStationsListBox.Items.Clear();
            }
            catch (DuplicateLineException ex)
            {
                MessageBox.Show(ex.Message, "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addLineTripButton_Click(object sender, RoutedEventArgs e)
        {
            listLineTrips.Add(addLineTripGrid.DataContext as LineTrip);
            lineTripDataGrid.ItemsSource = from l in listLineTrips select l;
            newLineTrip = new LineTrip();
            addLineTripGrid.DataContext = newLineTrip;
        }
    }
}
