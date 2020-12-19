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
using dotNet5781_01_6715;

namespace dotNet5781_03B_6715_5227
{
    /// <summary>
    /// Logique d'interaction pour BusDetailsWindow.xaml
    /// </summary>
    public partial class BusDetailsWindow : Window
    {
        public BusDetailsWindow()
        {
            //Bus bus = new Bus();
            //this.DataContext = bus;
            //this.statusComboBox.ItemsSource = Enum.GetValues(typeof(Bus.Status));
            //dateStartDatePicker.DataContext = DataContext;
            InitializeComponent();
        }

        private void Refueling_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Maintenance_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
