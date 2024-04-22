﻿using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.WPF.View.Guest;
using BookingApp.WPF.View.Guide;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class ReschedulingOverviewViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ReschedulingRequestDTO> _requests;
        public ObservableCollection<ReschedulingRequestDTO> Requests
        {
            get { return _requests; }
            set
            {
                _requests = value;
                OnPropertyChanged(nameof(Requests));
            }
        }
        public ReschedulingRequestDTO SelectedRequest { get; set; }
        public List<AccommodationReservation> AccommodationReservations { get; set; }
        public List<int> AccommodationReservationsIds { get; set; }
        public List<ReservationModificationRequest> ReservationModificationRequests { get; set; }
        private static ReservationModificationRequestService _reservationModificationRequestService;
        private static AccommodationReservationService _accommodationReservationService;
        private static UserService _userService;
        private static AccommodationService _accommodationService;

        public ReschedulingOverviewViewModel(List<Domain.Model.AccommodationReservation> ownerAccommodationReservations)
        {
            InitializeServices();
            Requests = new ObservableCollection<ReschedulingRequestDTO>();
            AccommodationReservations = ownerAccommodationReservations;
            AccommodationReservationsIds = AccommodationReservations.Select(a => a.Id).ToList();
            ReservationModificationRequests = _reservationModificationRequestService.GetByReservationIds(AccommodationReservationsIds);
            Update();
        }

        private void InitializeServices()
        {
            _reservationModificationRequestService = new ReservationModificationRequestService();
            _accommodationReservationService = new AccommodationReservationService();
            _userService = new UserService();
            _accommodationService = new AccommodationService();
        }

        private void Update()
        {
            Requests.Clear();
            foreach(var request in ReservationModificationRequests)
            {
                var reservation = _accommodationReservationService.GetByReservationId(request.ReservationId);
                var guest = _userService.GetById(reservation.GuestId);
                var accommodation = _accommodationService.GetById(reservation.AccommodationId);
                ReschedulingRequestDTO requestDTO = new ReschedulingRequestDTO(request, guest, accommodation, !IsReserved(request.NewStartDate, request.NewEndDate, request.OldStartDate, request.OldEndDate));
                Requests.Add(requestDTO);
            }
        }

        internal void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedRequest == null)
            {
                MessageBox.Show("Please select a request",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ApproveRequest();
                Update();
            }
        }

        private void ApproveRequest()
        {
            ReservationModificationRequest request = _reservationModificationRequestService.GetById(SelectedRequest.Id);
            AccommodationReservation reservation = _accommodationReservationService.GetByReservationId(request.ReservationId);
            if (request.Status == ReservationModificationRequest.RequestStatus.APPROVED)
            {
                MessageBox.Show("Request is already approved!",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                request.Status = ReservationModificationRequest.RequestStatus.APPROVED;
                reservation.StartDate = SelectedRequest.NewStartDate;
                reservation.EndDate = SelectedRequest.NewEndDate;
                _reservationModificationRequestService.Update(request);
                _accommodationReservationService.Update(reservation);
            }
        }

        internal void btnReject_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRequest == null)
            {
                MessageBox.Show("Please select a request",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                RejectRequest();
                Update();
            }
        }

        private void RejectRequest()
        {
            ReservationModificationRequest request = _reservationModificationRequestService.GetById(SelectedRequest.Id);
            string comment = PromptForComment();
            if (request.Status == ReservationModificationRequest.RequestStatus.REJECTED)
            {
                MessageBox.Show("Request is already rejected!",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(comment.Length > 0) 
            {
                request.Status = ReservationModificationRequest.RequestStatus.REJECTED;
                request.OwnerComment = comment;
                _reservationModificationRequestService.Update(request);
            }
            return;
        }

        private string PromptForComment()
        {
            string comment = string.Empty;

            var inputDialog = new CommentInputDialog();
            if (inputDialog.ShowDialog() == true)
            {
                comment = inputDialog.Answer;
            }

            return comment;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsReserved(DateTime startDate, DateTime endDate, DateTime oldStartDate, DateTime oldEndDate)
        {
            foreach (var reservation in AccommodationReservations)
            {
                if (((startDate <= reservation.EndDate && endDate >= reservation.StartDate) ||
                    (startDate >= reservation.StartDate && endDate <= reservation.EndDate)) 
                    && !isOldReservation(reservation, oldStartDate, oldEndDate))
                {
                    return true;
                }
            }

            return false;
        }

        private bool isOldReservation(AccommodationReservation reservation, DateTime oldStartDate, DateTime oldEndDate)
        {
            return (reservation.StartDate == oldStartDate && reservation.EndDate == oldEndDate);
        }
    }
}