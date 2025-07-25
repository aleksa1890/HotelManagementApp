using HotelManagementApp.Model;
using HotelManagementApp.Service;
using System;
using System.Collections.Generic;

namespace HotelManagementApp.Controller
{
    public class UserController
    {
        private readonly UserService _userService;

        public UserController()
        {
            _userService = new UserService();
        }

        public List<User> GetAllUsers()
        {
            return _userService.GetAllUsers();
        }

        public List<User> GetUsers(UserType? filterByType, string sortBy, string sortDirection)
        {
            // Kontroler samo prosleđuje poziv i parametre servisu
            return _userService.GetUsers(filterByType, sortBy, sortDirection);
        }

        public User RegisterOwner(User newOwner)
        {
            // Kontroler samo prosleđuje poziv servisu.
            // Try-catch blok ostaje u View-u, jer je View zadužen za prikazivanje grešaka korisniku.
            return _userService.RegisterOwner(newOwner);
        }

        public void ToggleUserBlockStatus(string jmbg)
        {
            _userService.ToggleUserBlockStatus(jmbg);
        }
    }
}