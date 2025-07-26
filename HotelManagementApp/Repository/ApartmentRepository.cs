using HotelManagementApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HotelManagementApp.Repository
{
    public class ApartmentRepository
    {
        private readonly string _filePath;

        public ApartmentRepository()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "apartments.csv");
        }

        /// <summary>
        /// Reads all apartments from the CSV and groups them by HotelCode.
        /// </summary>
        /// <returns>A dictionary where the key is the HotelCode and the value is a list of apartments.</returns>
        public Dictionary<string, List<Apartment>> GetAllGroupedByHotel()
        {
            var apartmentsByHotel = new Dictionary<string, List<Apartment>>();

            if (!File.Exists(_filePath))
            {
                // Ensure you've set 'Copy to Output Directory' for apartments.csv
                return apartmentsByHotel;
            }

            var lines = File.ReadAllLines(_filePath).Skip(1); // Skip header

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] values = line.Split(',');
                if (values.Length != 5) continue;

                Apartment apartment = new Apartment
                {
                    Name = values[0].Trim(),
                    Description = values[1].Trim(),
                    RoomCount = int.Parse(values[2].Trim()),
                    MaxGuestCount = int.Parse(values[3].Trim())
                };

                string hotelCode = values[4].Trim();

                // If the dictionary doesn't have this hotel key yet, create a new list for it
                if (!apartmentsByHotel.ContainsKey(hotelCode))
                {
                    apartmentsByHotel[hotelCode] = new List<Apartment>();
                }

                // Add the apartment to the correct hotel's list
                apartmentsByHotel[hotelCode].Add(apartment);
            }

            return apartmentsByHotel;
        }
    }
}
