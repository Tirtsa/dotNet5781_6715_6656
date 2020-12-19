using System.Windows;
using System.Windows.Controls;

namespace dotNet5781_03B_6656_9835
{
	/// <summary>
	/// Interaction logic for AddBus.xaml
	/// </summary>
	public partial class AddBus : Window
	{
		public AddBus()
		{
			InitializeComponent();
		}

		public object NewBus { get; private set; }
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mw = Application.Current.MainWindow as MainWindow;
			mw.AddBus(int.Parse(TextBox.Text), Calendar.SelectedDate.Value.Day, Calendar.SelectedDate.Value.Month, Calendar.SelectedDate.Value.Year);
			this.Close();
		}
		private void closeButton_Click(object sender, RoutedEventArgs e) { this.Close(); }
	}
}
