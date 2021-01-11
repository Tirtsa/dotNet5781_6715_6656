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
    /// Logique d'interaction pour UpdateStation.xaml
    /// </summary>
    public partial class UpdateStationWindow : Window
    {
        static IBL bl;
        public UpdateStationWindow()
        {
            bl = BlFactory.GetBL();
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bl.UpdateBusStation(new BO.BusStation
            {
                BusStationKey = int.Parse(busStationKeyTextBox.Text),
                Address = addressTextBox.Text,
                StationName = stationNameTextBox.Text
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource busStationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("busStationViewSource")));
            // Charger les données en définissant la propriété CollectionViewSource.Source :
            // busStationViewSource.Source = [source de données générique]
        }
    }
}
