using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Model
{
    public class User
    {
        public string Jmbg { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public bool IsBlocked { get; set; }

        public User() { }
    }
}