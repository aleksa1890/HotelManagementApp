using HotelManagementApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HotelManagementApp.Repository
{
    public class UserRepository
    {
        private readonly string _filePath;

        public UserRepository()
        {
            // The data file is expected to be in a 'Data' folder in the project's output directory
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "users.csv");
        }

        /// <summary>
        /// Reads all users from the CSV file.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public List<User> GetAll()
        {
            List<User> users = new List<User>();

            if (!File.Exists(_filePath))
            {
                // If the file doesn't exist, return an empty list.
                return users;
            }

            try
            {
                var lines = File.ReadAllLines(_filePath).Skip(1); // Skip header row if you add one

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] values = line.Split(',');
                    if (values.Length != 8) continue; // Basic validation

                    User user = new User
                    {
                        Jmbg = values[0],
                        Email = values[1],
                        Password = values[2],
                        FirstName = values[3],
                        LastName = values[4],
                        PhoneNumber = values[5],
                        UserType = Enum.Parse<UserType>(values[6]),
                        IsBlocked = bool.Parse(values[7])
                    };

                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                // In a real application, you would log this exception.
                // For now, we can write to the console.
                Console.WriteLine($"Error reading user data: {ex.Message}");
            }

            return users;
        }

        // We will add methods for saving data (e.g., SaveAll, Add, Update) later.

        /// <summary>
        /// Saves a list of users to the CSV file, overwriting the existing content.
        /// </summary>
        /// <param name="users">The list of users to save.</param>
        public void Save(List<User> users)
        {
            try
            {
                // Define the header for the CSV file.
                string header = "Jmbg,Email,Password,FirstName,LastName,PhoneNumber,UserType,IsBlocked";

                // Use a StringBuilder for efficient string concatenation.
                var csvBuilder = new System.Text.StringBuilder();
                csvBuilder.AppendLine(header);

                foreach (var user in users)
                {
                    // Create a line for each user in the specified CSV format.
                    string line = string.Join(",",
                        user.Jmbg,
                        user.Email,
                        user.Password,
                        user.FirstName,
                        user.LastName,
                        user.PhoneNumber,
                        user.UserType.ToString(),
                        user.IsBlocked.ToString());
                    csvBuilder.AppendLine(line);
                }

                // Write the generated content to the file.
                File.WriteAllText(_filePath, csvBuilder.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user data: {ex.Message}");
            }
        }
    }
}