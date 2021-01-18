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
    /// Interaction logic for AddLineToStationWindow.xaml
    /// </summary>
    public partial class AddLineToStationWindow : Window
    {
        static IBL bl;
        public AddLineToStationWindow()
        {
            bl = BlFactory.GetBL();
            InitializeComponent();
            cbAreaSelect.ItemsSource = Enum.GetValues(typeof(BO.Areas));
        }
        private void tbLineNumber_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;
            e.Handled = !(key > 33 && key < 44 || key > 73 && key < 84 || key == 2);
        }
        private void AddLineButton_Click(object sender, RoutedEventArgs e)
        {
            BusLine busLine = bl.GetBusLine(int.Parse(tbLineNumber.Text), bl.AreasAdapter((BO.Areas)cbAreaSelect.SelectedValue));
            BusStation busStation = (BusStation)DataContext;
            IEnumerable<int> toAdd = new int[] { busStation.BusStationKey };
            busLine.AllStationsOfLine = busLine.AllStationsOfLine.Concat(toAdd);
            bl.UpdateBusLine(busLine);
            //UpdateLayout();?
            Close();
        }
    }
}
