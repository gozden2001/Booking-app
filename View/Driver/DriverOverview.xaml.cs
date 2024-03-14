﻿using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BookingApp.View.Driver
{
    /// <summary>
    /// Interaction logic for DriverOverview.xaml
    /// </summary>
    public partial class DriverOverview : Window
    {
        public static ObservableCollection<Vehicle> Vehicles { get; set; }
        public List<Location> locations { get; set; }

        public static ObservableCollection<DriveReservation> DriveReservations { get; set; }
        public static DriveReservation SelectedReservation { get; set; }
        public static bool canCancel { get; set; }

        private readonly DriveReservationRepository _repository;
        private readonly LocationRepository _locationRepository;
        private readonly VehicleRepository _vehicleRepository;

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private int sec = 0;

        public DriverOverview()
        {
            InitializeComponent();
            DataContext = this;
            Vehicles = new ObservableCollection<Vehicle>();
            _repository = new DriveReservationRepository();
            DriveReservations = new ObservableCollection<DriveReservation>(_repository.GetAll());
            _locationRepository = new LocationRepository();
            _vehicleRepository = new VehicleRepository();
            locations = _locationRepository.GetAll();
            canCancel = false;
            UpdateVehicleCount();
            UpdateReservationList();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateVehicleCount()
        {
            var allVehicles = _vehicleRepository.GetAll();
            txtVehicleCount.Text = $"Ukupno registrovanih vozila: {allVehicles.Count}";
        }

        private void ShowCreateVehicleForm(object sender, RoutedEventArgs e)
        {
            VehicleForm vehicleForm = new VehicleForm();
            vehicleForm.VehicleAdded += VehicleForm_VehicleAdded;
            vehicleForm.Show();
        }


        private void ViewDrive_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedReservation != null)
            {
                ViewDrive vForm = new ViewDrive();
                vForm.reservation = SelectedReservation;
                vForm.ReservationConfirmed += DataGrid_Refresh;
                vForm.Repo = _repository;
                vForm.Show();
            }
            else
            {
                MessageBox.Show("You haven't selected any route!");
            }
        }

        private void VehicleForm_VehicleAdded(object? sender, EventArgs e)
        {
            UpdateVehicleCount();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UpdateReservationList()
        {
            ObservableCollection<DriveReservation> _reservations = new ObservableCollection<DriveReservation>(_repository.GetAll());
            DriveReservations.Clear();
            foreach(DriveReservation reservation in _reservations)
            {
                if(reservation.DriveReservationStatusId == 2)
                {
                    DriveReservations.Clear();
                    DriveReservations.Add(reservation);
                    return;
                }
                DriveReservations.Add(reservation);
            }
        }

        private void DataGrid_Refresh(object? sender, EventArgs e)
        {
            UpdateReservationList();
            if (sender is ViewDrive)
            {
                dispatcherTimer.Tick += Timer_Tick;
                dispatcherTimer.Interval = System.TimeSpan.Parse("00:00:01");
                dispatcherTimer.Start();
            }
        }

        private void ViewDrive_Respond(object? sender, EventArgs e)
        {
            if(SelectedReservation != null)
            {
                if (SelectedReservation.DriveReservationStatusId != 2)
                {
                    RespondView rvForm = new RespondView(SelectedReservation);
                    rvForm.ReservationConfirmed += DataGrid_Refresh;
                    rvForm.Show();
                }
                else
                {
                    MessageBox.Show("You already have confirmed reservation!");
                }
            }
            else
            {
                MessageBox.Show("You haven't selected any reservation!");
            }
        }

        private void ViewDrive_Cancel(object? sender, EventArgs e)
        {
            if (SelectedReservation != null)
            {
                if (canCancel)
                {
                    SelectedReservation.DriveReservationStatusId = 8;
                    _repository.Update(SelectedReservation);
                    UpdateReservationList();
                }else
                    MessageBox.Show("Still can't cancel!");
            }
            else
            {
                MessageBox.Show("You haven't selected any reservation!");
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            sec++;
            if(sec == 10)
            {
                MessageBox.Show("Client hasn't showed up!");
                canCancel = true;
            }
        }
    }
}
