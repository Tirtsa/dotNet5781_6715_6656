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
    public partial class AddBusLine : Window
    {
        static IBL bl;
        public AddBusLine()
        {
            bl = BlFactory.GetBL();
            InitializeComponent();
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
            LineStationsListBox.Items.Add(currentItemText);
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
                int lineNumber;
                int.TryParse(busLineNumberTextBox.Text, out lineNumber);
                bl.AddBusLine(new BusLine {
                    BusLineNumber = lineNumber,
                    Area = (Areas)Enum.Parse(typeof(Areas), areaTextBox.Text),
                    FirstStationKey = (int)LineStationsListBox.Items.GetItemAt(0),
                    LastStationKey = (int)LineStationsListBox.Items.GetItemAt(LineStationsListBox.Items.Count - 1),
                    AllStationsOfLine = from string item in LineStationsListBox.Items
                                        select int.Parse(item.Substring(item.IndexOf(" תחנה " + 6), 5))
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(" : אירעה תקלה" + ex);
            }
        }
    }
}
