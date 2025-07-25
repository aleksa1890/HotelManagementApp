using HotelManagementApp.Model;
using HotelManagementApp.Service;
using System.Collections.Generic;

namespace HotelManagementApp.Controller
{
    public class HotelController
    {
        private readonly HotelService _hotelService;

        public HotelController()
        {
            _hotelService = new HotelService();
        }

        /// <summary>
        /// Gets a sorted list of hotels by passing the request to the service layer.
        /// </summary>
        public List<Hotel> GetHotels(string sortBy, string sortDirection)
        {
            return _hotelService.GetHotels(sortBy, sortDirection);
        }
    }
}
