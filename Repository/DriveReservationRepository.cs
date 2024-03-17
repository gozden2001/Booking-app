﻿using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class DriveReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/drivereservation.csv";
        private readonly Serializer<DriveReservation> _serializer;
        private List<DriveReservation> _driveReservations;

        public DriveReservationRepository()
        {
            _serializer = new Serializer<DriveReservation>();
            _driveReservations = _serializer.FromCSV(FilePath);
        }

        public List<DriveReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public DriveReservation Save(DriveReservation driveReservation)
        {
            driveReservation.Id = NextId();
            _driveReservations = _serializer.FromCSV(FilePath);
            _driveReservations.Add(driveReservation);
            _serializer.ToCSV(FilePath, _driveReservations);
            return driveReservation;
        }

        public int NextId()
        {
            _driveReservations = _serializer.FromCSV(FilePath);
            if (_driveReservations.Count < 1)
            {
                return 1;
            }
            return _driveReservations.Max(r => r.Id) + 1;
        }

        public void Delete(DriveReservation driveReservation)
        {
            _driveReservations = _serializer.FromCSV(FilePath);
            DriveReservation founded = _driveReservations.Find(r => r.Id == driveReservation.Id);
            _driveReservations.Remove(founded);
            _serializer.ToCSV(FilePath, _driveReservations);
        }

        public DriveReservation Update(DriveReservation driveReservation)
        {
            _driveReservations = _serializer.FromCSV(FilePath);
            DriveReservation current = _driveReservations.Find(r => r.Id == driveReservation.Id);
            int index = _driveReservations.IndexOf(current);
            _driveReservations.Remove(current);
            _driveReservations.Insert(index, driveReservation);
            _serializer.ToCSV(FilePath, _driveReservations);
            return driveReservation;
        }

        public List<DriveReservation> GetByDriver(int driverId)
        {
            _driveReservations = _serializer.FromCSV(FilePath);
            return _driveReservations.FindAll(r => r.DriverId == driverId);
        }

        public List<DriveReservation> GetByTourist(int touristId)
        {
            _driveReservations = _serializer.FromCSV(FilePath);
            return _driveReservations.FindAll(r => r.TouristId == touristId);
        }
    }
}