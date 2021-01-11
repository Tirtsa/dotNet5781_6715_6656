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

namespace WPF_UI
{
    /// <summary>
    /// Logique d'interaction pour AddStationWindow.xaml
    /// </summary>
    public partial class AddStationWindow : Window
    {
        static IBL bl;
        public AddStationWindow()
        {
            bl = BlFactory.GetBL();
            InitializeComponent();
        }

        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            bl.AddStation(new BO.BusStation
            {
                BusStationKey = int.Parse(busStationKeyTextBox.Text),
                Address = addressTextBox.Text,
                StationName = stationNameTextBox.Text
            });
        }
    }
}
