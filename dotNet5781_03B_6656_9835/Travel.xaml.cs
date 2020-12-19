using System;
using System.Windows;
using System.Windows.Input;

namespace dotNet5781_03B_6656_9835
{
	/// <summary>
	/// Interaction logic for Travel.xaml
	/// </summary>
	public partial class Travel : Window
	{
		static int workerIndex = 0;
		public Travel()
		{
			InitializeComponent();
		}
		public object CurrentBus { get; set; }
		private bool IsValid()
		{
			Bus bus = (Bus)CurrentBus;
			bool valid = uint.TryParse(tbDistance.Text, out uint dist);

			if (!valid)
				MessageBox.Show("distance must be numeric");
			else
			{
				if ((int)dist >= 1200 || (int)(dist + bus.DistanceSinceRefuel) >= 1200)
				{
					valid = false;
					MessageBox.Show("The bus needs more fuel to go on this trip");
				}

				if (dist + bus.DistanceSinceMaintenance >= 20000 || bus.MaintenanceDate <= DateTime.Today.AddYears(-1))
				{
					MessageBox.Show("The bus must be sent to maintenence before this trip");
					valid = false;
				}
			}
			return valid;
		}
		private void tbDistance_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			int key = (int)e.Key;
			e.Handled = !(key > 33 && key < 44 || key > 73 && key < 84 || key == 2);
			
			if (e.Key == Key.Enter)
			{
				TravelButton_Click(sender, new RoutedEventArgs());
			}
		}
		private void TravelButton_Click(object sender, RoutedEventArgs e)
		{
			if(IsValid())
			{
				uint dist = uint.Parse(tbDistance.Text);
				MainWindow mw = Application.Current.MainWindow as MainWindow;
				object args = new object[3] { dist, CurrentBus, workerIndex };
				mw.Travel(args);
				this.Close();
			}
			workerIndex++;
		}
		private void closeButton_Click(object sender, RoutedEventArgs e) { this.Close(); }
	}
}
