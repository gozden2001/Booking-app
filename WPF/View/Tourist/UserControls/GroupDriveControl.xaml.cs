﻿using BookingApp.WPF.ViewModel.Tourist;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Tourist.UserControls
{
    /// <summary>
    /// Interaction logic for GroupDriveControl.xaml
    /// </summary>
    public partial class GroupDriveControl : UserControl
    {
        public GroupDriveControl()
        {
            InitializeComponent();
        }

        private void btnReserve_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is GroupDriveFormViewModel viewModel)
            {
                viewModel.ReserveGroupDrive();
                MessageBox.Show("Rezervacija uspešno izvršena!");


                Window parentWindow = Window.GetWindow(this);
                parentWindow?.Close();
            }
        }
    }
}