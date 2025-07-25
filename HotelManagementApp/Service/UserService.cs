using HotelManagementApp.Model;
using HotelManagementApp.Repository;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementApp.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        public User Login(string email, string password)
        {
            var allUsers = _userRepository.GetAll();
            var user = allUsers.FirstOrDefault(u => u.Email.Equals(email, System.StringComparison.OrdinalIgnoreCase));

            if (user == null || user.Password != password || user.IsBlocked)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Registers a new Owner user after validating their JMBG and Email for uniqueness.
        /// </summary>
        public User RegisterOwner(User newOwner)
        {
            newOwner.UserType = UserType.Owner;
            newOwner.IsBlocked = false;

            var allUsers = _userRepository.GetAll();

            if (allUsers.Any(u => u.Jmbg == newOwner.Jmbg))
            {
                throw new System.ArgumentException("User with this JMBG already exists.");
            }

            if (allUsers.Any(u => u.Email.Equals(newOwner.Email, System.StringComparison.OrdinalIgnoreCase)))
            {
                throw new System.ArgumentException("User with this email already exists.");
            }

            allUsers.Add(newOwner);
            _userRepository.Save(allUsers);

            return newOwner;
        }

        /// <summary>
        /// Gets all users from the repository.
        /// </summary>
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        /// <summary>
        /// Gets a filtered and sorted list of users.
        /// </summary>
        public List<User> GetUsers(UserType? filterByType, string sortBy, string sortDirection)
        {
            List<User> users = _userRepository.GetAll();

            // Apply filter if selected
            if (filterByType.HasValue)
            {
                users = users.Where(u => u.UserType == filterByType.Value).ToList();
            }

            // Apply sorting
            if (sortBy == "First Name")
            {
                users = sortDirection == "Ascending"
                    ? users.OrderBy(u => u.FirstName).ToList()
                    : users.OrderByDescending(u => u.FirstName).ToList();
            }
            else if (sortBy == "Last Name")
            {
                users = sortDirection == "Ascending"
                    ? users.OrderBy(u => u.LastName).ToList()
                    : users.OrderByDescending(u => u.LastName).ToList();
            }

            return users;
        }

        /// <summary>
        /// Toggles the IsBlocked status of a user identified by their JMBG.
        /// </summary>
        /// <param name="jmbg">The JMBG of the user to block/unblock.</param>
        public void ToggleUserBlockStatus(string jmbg)
        {
            List<User> allUsers = _userRepository.GetAll();

            User userToToggle = allUsers.FirstOrDefault(u => u.Jmbg == jmbg);

            if (userToToggle != null)
            {
                // Flip the boolean status
                userToToggle.IsBlocked = !userToToggle.IsBlocked;

                // Save the entire list with the updated status
                _userRepository.Save(allUsers);
            }
        }
    }
}