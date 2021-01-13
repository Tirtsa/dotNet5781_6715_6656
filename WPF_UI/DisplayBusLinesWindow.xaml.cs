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
	/// Interaction logic for DisplayBusLinesWindow.xaml
	/// </summary>
	public partial class DisplayBusLinesWindow : Window
	{
		static IBL bl;
		public DisplayBusLinesWindow()
		{
			bl = BlFactory.GetBL();
			InitializeComponent();
            lbBusLines.ItemsSource = bl.GetAllBusLines();//.GetAllBusLinesBy();
		}

		private void AddLineButton_Click(object sender, RoutedEventArgs e)
		{
			AddLineWindow addLineWindow = new AddLineWindow();
			addLineWindow.Show();
		}

		private void UpdateLineButton_Click(object sender, RoutedEventArgs e)
		{
			UpdateLineWindow updateLineWindow = new UpdateLineWindow { DataContext = lbBusLines.SelectedItem };
			updateLineWindow.Show();
		}

		private void DeleteLineButton_Click(object sender, RoutedEventArgs e)
		{
            BusLine busLine = (BusLine)lbBusLines.SelectedItem;
            bl.DeleteBusLine(busLine);
            //foreach (int station in busLine.AllStationsOfLine)
                //if (station == )
            MessageBox.Show("Line " + busLine.FirstStationKey + " in the " + busLine.Area + " region, was deleted successfully.");
            Close();
		}
	}
}
