﻿using BookingApp.WPF.ViewModel.Owner;
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

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for ReschedulingOverviewPage.xaml
    /// </summary>
    public partial class ReschedulingOverviewPage : Page
    {
        public static ReschedulingOverviewViewModel viewModel { get; set; }
        public ReschedulingOverviewPage(Domain.Model.Owner loggedInOwner)
        {
            InitializeComponent();
            viewModel = new ReschedulingOverviewViewModel(loggedInOwner);
            DataContext = viewModel;
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            viewModel.btnApprove_Click(sender, e);
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            viewModel.btnReject_Click(sender, e);
        }
    }
}
