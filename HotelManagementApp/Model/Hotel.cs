using System.Collections.Generic;

namespace HotelManagementApp.Model
{
    public class Hotel
    {
        // Šifra (string) - jedinstveno
        public string Code { get; set; }

        // ime (string)
        public string Name { get; set; }

        // godina izgradnje (int)
        public int YearBuilt { get; set; }

        // broj zvezdica (int)
        public int Stars { get; set; }

        // vlasnik - JMBG vlasnika (string)
        public string OwnerJmbg { get; set; }

        // apartmani (rečnik)
        // Ključ rečnika će biti jedinstveno ime apartmana.
        public Dictionary<string, Apartment> Apartments { get; set; }

        public Hotel()
        {
            Apartments = new Dictionary<string, Apartment>();
        }
    }
}
