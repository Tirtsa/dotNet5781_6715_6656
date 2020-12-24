using System;
using System.ComponentModel;
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

namespace dotNet5781_03B_6715_5227
{
    /// <summary>
    /// Logique d'interaction pour AddBusWindow.xaml
    /// </summary>
    public partial class AddBusWindow : Window
    {
        Bus myBus;
        public AddBusWindow()
        {
            InitializeComponent();

            myBus = new Bus();
            this.DataContext = myBus;

            this.busStatusComboBox.ItemsSource = Enum.GetValues(typeof(Bus.Status));

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListBuses.allBuses.Add(myBus);
                myBus = new Bus();
                this.DataContext = myBus;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
