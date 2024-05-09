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
    /// Interaction logic for AccommodationMonthlyStatsPage.xaml
    /// </summary>
    public partial class AccommodationMonthlyStatsPage : Page
    {
        public static AccommodationMonthlyStatsViewModel viewModel;
        public AccommodationMonthlyStatsPage(DTO.AccommodationStatisticsDTO selectedYear, Domain.Model.Accommodation accommodation)
        {
            InitializeComponent();
            viewModel = new AccommodationMonthlyStatsViewModel(selectedYear, accommodation);
            DataContext = viewModel;
        }
    }
}