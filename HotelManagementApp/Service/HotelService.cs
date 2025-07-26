using HotelManagementApp.Model;
using HotelManagementApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementApp.Service
{
    public class HotelService
    {
        private readonly HotelRepository _hotelRepository;
        private readonly ApartmentRepository _apartmentRepository;

        public HotelService()
        {
            _hotelRepository = new HotelRepository();
            _apartmentRepository = new ApartmentRepository();
        }

        private List<Hotel> GetLinkedHotels(bool includePending = false)
        {
            List<Hotel> hotels = _hotelRepository.GetAll();

            // Ako nije eksplicitno navedeno da se uključe i hoteli na čekanju,
            // prikaži samo one koji su odobreni.
            if (!includePending)
            {
                hotels = hotels.Where(h => h.IsApproved).ToList();
            }

            var apartmentsByHotel = _apartmentRepository.GetAllGroupedByHotel();

            foreach (var hotel in hotels)
            {
                if (apartmentsByHotel.ContainsKey(hotel.Code))
                {
                    hotel.Apartments = apartmentsByHotel[hotel.Code]
                                       .ToDictionary(apt => apt.Name, apt => apt);
                }
            }
            return hotels;
        }

        public List<Hotel> GetHotels(string sortBy, string sortDirection)
        {
            List<Hotel> hotels = GetLinkedHotels(); // Automatski dobavlja samo odobrene

            switch (sortBy)
            {
                case "Name":
                    hotels = sortDirection == "Ascending"
                        ? hotels.OrderBy(h => h.Name).ToList()
                        : hotels.OrderByDescending(h => h.Name).ToList();
                    break;
                case "Stars":
                    hotels = sortDirection == "Ascending"
                        ? hotels.OrderBy(h => h.Stars).ToList()
                        : hotels.OrderByDescending(h => h.Stars).ToList();
                    break;
            }

            return hotels;
        }

        public List<Hotel> SearchHotels(string searchBy, string searchValue, string rooms = null, string guests = null, string op = null)
        {
            List<Hotel> hotels = GetLinkedHotels(); // Automatski dobavlja samo odobrene

            switch (searchBy)
            {
                case "Name":
                    return string.IsNullOrWhiteSpace(searchValue) ? hotels : hotels.Where(h => h.Name.ToLower().Contains(searchValue.ToLower())).ToList();
                case "Code":
                    return string.IsNullOrWhiteSpace(searchValue) ? hotels : hotels.Where(h => h.Code.ToLower().Contains(searchValue.ToLower())).ToList();
                case "Year Built":
                    return string.IsNullOrWhiteSpace(searchValue) ? hotels : hotels.Where(h => h.YearBuilt.ToString().Contains(searchValue)).ToList();
                case "Stars":
                    return string.IsNullOrWhiteSpace(searchValue) ? hotels : hotels.Where(h => h.Stars.ToString().Contains(searchValue)).ToList();
                case "Apartments":
                    return FilterByApartmentCriteria(hotels, rooms, guests, op);
                default:
                    return hotels;
            }
        }

        private List<Hotel> FilterByApartmentCriteria(List<Hotel> hotels, string roomsStr, string guestsStr, string op)
        {
            bool searchByRooms = int.TryParse(roomsStr, out int rooms);
            bool searchByGuests = int.TryParse(guestsStr, out int guests);

            if (!searchByRooms && !searchByGuests) return hotels;
            if (searchByRooms && !searchByGuests) return hotels.Where(h => h.Apartments.Values.Any(a => a.RoomCount == rooms)).ToList();
            if (!searchByRooms && searchByGuests) return hotels.Where(h => h.Apartments.Values.Any(a => a.MaxGuestCount == guests)).ToList();
            if (searchByRooms && searchByGuests)
            {
                if (op == "|") return hotels.Where(h => h.Apartments.Values.Any(a => a.RoomCount == rooms || a.MaxGuestCount == guests)).ToList();
                else return hotels.Where(h => h.Apartments.Values.Any(a => a.RoomCount == rooms && a.MaxGuestCount == guests)).ToList();
            }
            return hotels;
        }

        public Hotel CreateHotel(Hotel newHotel)
        {
            var allHotels = _hotelRepository.GetAll();

            if (allHotels.Any(h => h.Code.Equals(newHotel.Code, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Hotel with the same code already exists.");
            }

            newHotel.IsApproved = false;
            _hotelRepository.Add(newHotel);
            return newHotel;
        }

        public List<Hotel> GetHotelsForOwner(string ownerJmbg)
        {
            // Pozivamo GetLinkedHotels sa parametrom 'true' da bismo dobili SVE hotele (i odobrene i na čekanju)
            List<Hotel> allHotels = GetLinkedHotels(true);
            // Filtriramo listu da prikažemo samo one koji pripadaju datom vlasniku
            return allHotels.Where(h => h.OwnerJmbg == ownerJmbg).ToList();
        }

        public void ApproveHotel(string hotelCode)
        {
            List<Hotel> allHotels = _hotelRepository.GetAll();
            Hotel hotelToApprove = allHotels.FirstOrDefault(h => h.Code == hotelCode);

            if (hotelToApprove != null)
            {
                hotelToApprove.IsApproved = true;
                _hotelRepository.Save(allHotels);
            }
        }

        public void DenyHotel(string hotelCode)
        {
            List<Hotel> allHotels = _hotelRepository.GetAll();
            Hotel hotelToDeny = allHotels.FirstOrDefault(h => h.Code == hotelCode);

            if (hotelToDeny != null)
            {
                allHotels.Remove(hotelToDeny);
                _hotelRepository.Save(allHotels);
            }
        }
    }
}