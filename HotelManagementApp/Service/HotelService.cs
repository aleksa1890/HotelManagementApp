using HotelManagementApp.Model;
using HotelManagementApp.Repository;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementApp.Service
{
    public class HotelService
    {
        private readonly HotelRepository _hotelRepository;

        public HotelService()
        {
            _hotelRepository = new HotelRepository();
        }

        /// <summary>
        /// Gets a list of hotels, optionally sorted by a specified property.
        /// </summary>
        /// <param name="sortBy">The property to sort by ("Name" or "Stars").</param>
        /// <param name="sortDirection">The direction to sort ("Ascending" or "Descending").</param>
        /// <returns>A sorted list of hotels.</returns>
        public List<Hotel> GetHotels(string sortBy, string sortDirection)
        {
            List<Hotel> hotels = _hotelRepository.GetAll();

            // Apply sorting based on the provided parameters.
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
    }
}