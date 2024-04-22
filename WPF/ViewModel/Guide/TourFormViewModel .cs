﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Repository;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class TourFormViewModel : INotifyPropertyChanged
    {
        private readonly TourService _tourService;
        private readonly CheckpointService _checkpointService;
        private readonly ImageService _imageService;
        private readonly LocationService _locationService;
        private readonly LanguageService _languageService;
        private readonly TourInstanceService _tourInstanceService;

        public List<Location> Locations { get; set; }
        public List<Language> Languages { get; set; }
        public List<Domain.Model.Image> Images { get; set; }
        public List<TourInstance> TourStartDates { get; set; }
        public List<Checkpoint> Checkpoints { get; set; }

        private int _tourId;

        public TourFormViewModel()
        {
            _tourService = new TourService();
            _checkpointService = new CheckpointService();
            _imageService = new ImageService();
            _locationService = new LocationService();
            _languageService = new LanguageService();
            _tourInstanceService = new TourInstanceService();

            _tourId = _tourService.NextId();

            Languages = _languageService.GetAll();
            Locations = _locationService.GetAll();
            Images = new List<Domain.Model.Image>();
            Checkpoints = new List<Checkpoint>();
            TourStartDates = new List<TourInstance>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _tourName;
        public string TourName
        {
            get => _tourName;
            set
            {
                if (_tourName != value)
                {
                    _tourName = value;
                    OnPropertyChanged();
                }
            }
        }

        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                if (_selectedLocation != value)
                {
                    _selectedLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TourId
        {
            get => _tourId;
        }

        private int _capacity;
        public int Capacity
        {
            get => _capacity;
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                    OnPropertyChanged();
                }
            }
        }

        private float _durationHours;
        public float DurationHours
        {
            get => _durationHours;
            set
            {
                if (_durationHours != value)
                {
                    _durationHours = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ValidateFields()
        {
            return _capacity > 0 && !string.IsNullOrEmpty(_tourName) && _durationHours > 0 && _selectedLanguage != null &&
                   _selectedLocation != null && TourStartDates.Count > 0 && Checkpoints.Count > 0 && !string.IsNullOrEmpty(_description);
        }

        public void SaveTour()
        {
            if (ValidateFields())
            {
                Tour newTour = new Tour(_tourName, _description, _selectedLocation.Id, _selectedLanguage.languageId, _capacity, _durationHours);
                _tourService.Save(newTour);

                SaveImages();
                SaveCheckpoints();
                SaveStartDates();

                MessageBox.Show("Successfully added.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Not added.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveImages()
        {
            foreach (var image in Images)
            {
                _imageService.Save(image);
            }
        }

        private void SaveCheckpoints()
        {
            foreach (var checkpoint in Checkpoints)
            {
                _checkpointService.Save(checkpoint);
            }
        }

        private void SaveStartDates()
        {
            foreach (var startDate in TourStartDates)
            {
                _tourInstanceService.Save(startDate);
            }
        }
    }
}