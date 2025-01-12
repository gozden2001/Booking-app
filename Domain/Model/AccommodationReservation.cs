﻿using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
        public int GuestNumber { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public bool IsRated { get; set; }
        public bool IsAccommodationAndOwnerRated { get; set; }
        public bool IsCancelled { get; set; }
        public AccommodationReservation()
        {
        }
        public AccommodationReservation(DateTime startDate, DateTime endDate, int days, int guestNumber, int accommodationId, int guestId, bool isRated, bool isAccommodationAndOwnerRated, bool isCancelled)
        {
            StartDate = startDate;
            EndDate = endDate;
            Days = days;
            GuestNumber = guestNumber;
            AccommodationId = accommodationId;
            GuestId = guestId;
            IsRated = isRated;
            IsAccommodationAndOwnerRated = isAccommodationAndOwnerRated;
            IsCancelled = isCancelled;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartDate.ToString("dd.MM.yyyy"), EndDate.ToString("dd.MM.yyyy"), Days.ToString(), GuestNumber.ToString(), AccommodationId.ToString(), GuestId.ToString(), IsRated.ToString(), IsAccommodationAndOwnerRated.ToString(), IsCancelled.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            DateTime startDate;
            if (DateTime.TryParseExact(values[1], "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out startDate))
            {
                StartDate = startDate;
            }
            else
            {
                // Handle parsing failure
                Console.WriteLine("Failed to parse start date from CSV.");
            }
            DateTime endDate;
            if (DateTime.TryParseExact(values[2], "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out endDate))
            {
                EndDate = endDate;
            }
            else
            {
                // Handle parsing failure
                Console.WriteLine("Failed to parse start date from CSV.");
            }
            Days = Convert.ToInt32(values[3]);
            GuestNumber = Convert.ToInt32(values[4]);
            AccommodationId = Convert.ToInt32(values[5]);
            GuestId = Convert.ToInt32(values[6]);
            IsRated = Convert.ToBoolean(values[7]);
            IsAccommodationAndOwnerRated = Convert.ToBoolean(values[8]);
            IsCancelled = Convert.ToBoolean(values[9]);
        }

        public override string ToString()
        {
            if (IsRated)
            {
                if (IsAccommodationAndOwnerRated)
                {
                    return $"{StartDate.ToString("dd.MM.yyyy")}, {EndDate.ToString("dd.MM.yyyy")}, Days: {Days}, Number of Guests: {GuestNumber},Guest Rated, Rated";
                }
                else
                {
                    return $"{StartDate.ToString("dd.MM.yyyy")}, {EndDate.ToString("dd.MM.yyyy")}, Days: {Days}, Number of Guests: {GuestNumber},Guest Rated, Unrated";
                }
            }
            else
            {
                if (IsAccommodationAndOwnerRated)
                {
                    return $"{StartDate.ToString("dd.MM.yyyy")}, {EndDate.ToString("dd.MM.yyyy")}, Days: {Days}, Number of Guests: {GuestNumber}, Unrated, Rated";
                }
                else
                {
                    return $"{StartDate.ToString("dd.MM.yyyy")}, {EndDate.ToString("dd.MM.yyyy")}, Days: {Days}, Number of Guests: {GuestNumber}, Unrated, Unrated";
                }

            }

        }

    }
}
