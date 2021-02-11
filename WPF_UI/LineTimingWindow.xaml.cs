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

namespace WPF_UI
{
    /// <summary>
    /// Logique d'interaction pour LineTiming.xaml
    /// </summary>
    public partial class LineTimingWindow : Window
    {
        MainWindow main;
        public LineTimingWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            main = mainWindow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                main.CancelThread();
               
            });
        }
    }
}
