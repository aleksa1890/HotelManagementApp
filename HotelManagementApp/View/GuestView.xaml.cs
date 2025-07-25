using HotelManagementApp.Controller;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementApp.View
{
    public partial class GuestView : Window
    {
        private readonly HotelController _hotelController;

        public GuestView()
        {
            InitializeComponent();
            _hotelController = new HotelController();

            // Postavi podrazumevane vrednosti za sortiranje
            SortByComboBox.SelectedIndex = 0; // Name
            SortDirectionComboBox.SelectedIndex = 0; // Ascending

            // Učitaj hotele sa podrazumevanim sortiranjem
            LoadHotels();
        }

        private void LoadHotels()
        {
            ApplySort_Click(null, null);
        }

        private void ApplySort_Click(object sender, RoutedEventArgs e)
        {
            string sortBy = (SortByComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string sortDirection = (SortDirectionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Pozovi kontroler da dobaviš sortirane hotele
            var hotels = _hotelController.GetHotels(sortBy, sortDirection);

            // Podesi izvor podataka za tabelu
            HotelsDataGrid.ItemsSource = hotels;
        }
    }
}
