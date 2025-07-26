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

        public List<Hotel> SearchHotels(string searchBy, string searchValue, string rooms = null, string guests = null, string op = null)
        {
            return _hotelService.SearchHotels(searchBy, searchValue, rooms, guests, op);
        }

        public Hotel CreateHotel(Hotel newHotel)
        {
            return _hotelService.CreateHotel(newHotel);
        }


        public List<Hotel> GetHotelsForOwner(string ownerJmbg)
        {
            return _hotelService.GetHotelsForOwner(ownerJmbg);
        }

        public void ApproveHotel(string hotelCode)
        {
            _hotelService.ApproveHotel(hotelCode);
        }

        public void DenyHotel(string hotelCode)
        {
            _hotelService.DenyHotel(hotelCode);
        }
    }
}
