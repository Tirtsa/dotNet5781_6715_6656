﻿using System;
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


        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            bl.UpdateBusStation(new BusStation
            {
                BusStationKey = (int)busStationKeyLabel.Content,
                Address = addressTextBox.Text,
                StationName = stationNameTextBox.Text,
               LinesThatPass = (DataContext as BusStation).LinesThatPass
            });

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
