using System.Windows;
using System.Windows.Input;

namespace dotNet5781_03B_6656_9835
{
	/// <summary>
	/// Interaction logic for BusInfo.xaml
	/// </summary>
	public partial class BusInfo : Window
	{
		public BusInfo()
		{
			InitializeComponent();
		}
		public object CurrentBus { get; set; }
		public void tbFill()
		{
			tbInfo.Text = CurrentBus.ToString();
		}
		private void EditName_Click(object sender, RoutedEventArgs e)
		{
			EditName.Visibility = Visibility.Hidden;
			tbAddName.Visibility = Visibility.Visible;
		}
		private void tbAddName_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			int key = (int)e.Key;
			e.Handled = !(key > 43 && key < 70 || key == 18 || key == 2);
			
			if (e.Key == Key.Enter)
			{
				tbAddName.Visibility = Visibility.Hidden; 
				EditName.Visibility = Visibility.Visible;
				Bus bus = (Bus)CurrentBus;
				bus.DriverName = tbAddName.Text;
				tbFill();
			}
		}
		private void Service_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mw = Application.Current.MainWindow as MainWindow;
			mw.Service(CurrentBus);
			this.Close();
		}
		private void Refuel_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mw = Application.Current.MainWindow as MainWindow;
			Bus bus = (Bus)CurrentBus;
			mw.RefuelAssist(bus);
			this.Close();
		}
		private void closeButton_Click(object sender, RoutedEventArgs e) { this.Close(); }
	}
}
