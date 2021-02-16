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
    /// Interaction logic for OpeningWindow.xaml
    /// </summary>
    public partial class OpeningWindow : Window
    {
        static IBL bl = BlFactory.GetBL();
        User user;
        public OpeningWindow()
        {
            InitializeComponent();
            user = new User();
            managerDetailsGrid.DataContext = user;
            travellerDetailsGrid.DataContext = user;
        }

        private void managerConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            User myUser = new User();
            try
            {
                //User user = managerDetailsGrid.DataContext as User;
                myUser = bl.GetUser(user.UserId, user.Password);
            }
            catch(InexistantUserException ex)
            {
                MessageBox.Show(ex.Message, "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (myUser.UserStatus != UserStatus.Manager)
                MessageBox.Show("שם המשתמש והסיסמא שהוזנו אינם תואמים נתונים של מנהל", "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);

            MainWindow newWindow = new MainWindow();
            newWindow.Show();
            Close();
        }

        private void travellerConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            User myUser = new User();
            try
            {
                User user = travellerDetailsGrid.DataContext as User;
                myUser = bl.GetUser(user.UserId, user.Password);
            }
            catch (InexistantUserException ex)
            {
                MessageBox.Show(ex.Message, "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (myUser.UserStatus != UserStatus.Traveller)
                MessageBox.Show("שם המשתמש והסיסמא שהוזנו אינם תואמים נתונים של נוסע", "אירעה שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);

            MainWindow newWindow = new MainWindow();
            newWindow.Show();
            Close();
        }
    }
}
