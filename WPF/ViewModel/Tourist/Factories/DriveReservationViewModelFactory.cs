﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Tourist.Factories
{
    public class DriveReservationViewModelFactory
    {
        private DriveReservationService _driveReservationService;
        private UserService _userService;
        private DetailedLocationService _detailedLocationService;
        private User _user;
        

        public DriveReservationViewModelFactory(User loggedUser, DriveReservationService driveReservationService, UserService userService, DetailedLocationService detailedLocationService)
        {
            _user = loggedUser;
            _driveReservationService = driveReservationService;
            _userService = userService;
            _detailedLocationService = detailedLocationService;
        }

        public List<DriveReservationViewModel> CreateViewModels()
        {
            List<DriveReservationViewModel> viewModels = new List<DriveReservationViewModel>();
            foreach (var reservation in _driveReservationService.GetActiveReservationsForUser(_user.Id))
            {
                string driver;
                if (reservation.DriverId == 0) 
                    driver = "?";
                else 
                    driver = _userService.GetById(reservation.DriverId).Username;

                DriveReservationViewModel viewModel = new DriveReservationViewModel()
                {
                    DriveReservationId = reservation.Id,
                    Driver = driver,
                    Time = reservation.DepartureTime,
                    StartAddress = _detailedLocationService.GetById(reservation.PickupLocationId).Address,
                    EndAddress = _detailedLocationService.GetById(reservation.DropoffLocationid).Address,
                    DelayDriver = reservation.DelayMinutesDriver,
                    DelayTourist = reservation.DelayMinutesTourist,
                    Status = GetReservationStatus(reservation.DriveReservationStatusId, reservation.DelayMinutesDriver, reservation.DelayMinutesTourist)

                };

                viewModels.Add(viewModel);
            }

            return viewModels;
        }


        private string GetReservationStatus(int reservationStatusId, double delayDriver, double delayTourist)
        {
            string status = "";
            if (delayDriver != 0)
            {
                status = $"Vozac kasni {delayDriver} minuta";
                
            }

            if (delayTourist != 0)
            {
                status += $"Turista kasni {delayTourist} minuta";
            }

            else
            {
                switch (reservationStatusId)
                {
                    case 12:
                        status = "Vozac jos nije pronadjen";
                        break;
                    case 4:
                        status = "Vozac je stigao na adresu";
                        break;
                    case 5:
                        status = "Prihvacena voznja";
                        break;
                    case 2:
                        status = "Prihvacena voznja";
                        break;
                    case 13:
                        status = "Prihvacena voznja";
                        break;
                    default:
                        status = "Status Unknown";  
                        break;
                }
            }
            return status;
        }
    }
}