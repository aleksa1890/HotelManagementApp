using HotelManagementApp.Model;
using HotelManagementApp.Repository;
using System.Linq;

namespace HotelManagementApp.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            // The service creates an instance of the repository to work with user data.
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The User object if login is successful, otherwise null.</returns>
        public User Login(string email, string password)
        {
            // 1. Get all users from the repository
            var allUsers = _userRepository.GetAll();

            // 2. Find a user with the matching email
            var user = allUsers.FirstOrDefault(u => u.Email.Equals(email, System.StringComparison.OrdinalIgnoreCase));

            // 3. Perform checks
            if (user == null)
            {
                // User with the given email does not exist
                return null;
            }

            if (user.Password != password)
            {
                // Password does not match
                return null;
            }

            if (user.IsBlocked)
            {
                // User is blocked and cannot log in
                return null;
            }

            // 4. If all checks pass, return the found user object
            return user;
        }

        // Other user-related business logic methods will be added here later.

        /// <summary>
        /// Registers a new Owner user after validating their JMBG and Email for uniqueness.
        /// </summary>
        /// <param name="newOwner">The User object for the new owner.</param>
        /// <returns>Returns the newly created user if registration is successful.</returns>
        /// <exception cref="System.ArgumentException">Thrown if JMBG or Email already exist.</exception>
        public User RegisterOwner(User newOwner)
        {
            // Ensure the new user is of type Owner.
            newOwner.UserType = UserType.Owner;
            newOwner.IsBlocked = false; // Owners are not blocked by default.

            var allUsers = _userRepository.GetAll();

            // Check for uniqueness of JMBG.
            if (allUsers.Any(u => u.Jmbg == newOwner.Jmbg))
            {
                throw new System.ArgumentException("User with this JMBG already exists.");
            }

            // Check for uniqueness of Email.
            if (allUsers.Any(u => u.Email.Equals(newOwner.Email, System.StringComparison.OrdinalIgnoreCase)))
            {
                throw new System.ArgumentException("User with this email already exists.");
            }

            // Add the new owner to the list.
            allUsers.Add(newOwner);

            // Save the updated list back to the file.
            _userRepository.Save(allUsers);

            return newOwner;
        }
    }
}
