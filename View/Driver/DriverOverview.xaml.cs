﻿using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.View.Driver
{
    /// <summary>
    /// Interaction logic for DriverOverview.xaml
    /// </summary>
    public partial class DriverOverview : Window
    {
        public static ObservableCollection<Vehicle> Vehicles { get; set; }
        public DriverOverview()
        {
            InitializeComponent();
            DataContext = this;
            Vehicles = new ObservableCollection<Vehicle>();
        }
        private void ShowCreateVehicleForm(object sender, RoutedEventArgs e)
        {
            VehicleForm vehicleForm = new VehicleForm();
            vehicleForm.Show();
        }
    }
}