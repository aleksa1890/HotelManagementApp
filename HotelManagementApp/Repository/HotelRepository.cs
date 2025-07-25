using HotelManagementApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HotelManagementApp.Repository
{
    public class HotelRepository
    {
        private readonly string _filePath;

        public HotelRepository()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "hotels.csv");
        }

        public List<Hotel> GetAll()
        {
            List<Hotel> hotels = new List<Hotel>();

            if (!File.Exists(_filePath))
            {
                return hotels;
            }

            var lines = File.ReadAllLines(_filePath).Skip(1);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] values = line.Split(',');
                if (values.Length != 5) continue;

                Hotel hotel = new Hotel
                {
                    Code = values[0].Trim(),
                    Name = values[1].Trim(),
                    YearBuilt = int.Parse(values[2].Trim()),
                    Stars = int.Parse(values[3].Trim()),
                    OwnerJmbg = values[4].Trim(),
                    Apartments = new Dictionary<string, Apartment>() // Za sada prazno
                };

                hotels.Add(hotel);
            }

            return hotels;
        }

        // Metode za čuvanje (Save) ćemo dodati kasnije po potrebi.
    }
}
