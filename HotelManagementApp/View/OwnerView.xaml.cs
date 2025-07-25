using HotelManagementApp.Controller;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementApp.View
{
    public partial class OwnerView : Window
    {
        private readonly HotelController _hotelController;

        public OwnerView()
        {
            InitializeComponent();
            _hotelController = new HotelController();

            SortByComboBox.ItemsSource = new string[] { "Name", "Stars" };
            SortDirectionComboBox.ItemsSource = new string[] { "Ascending", "Descending" };

            SortByComboBox.SelectedIndex = 0;
            SortDirectionComboBox.SelectedIndex = 0;

            LoadHotels();
        }

        private void LoadHotels()
        {
            ApplySort_Click(null, null);
        }

        private void ApplySort_Click(object sender, RoutedEventArgs e)
        {
            string sortBy = SortByComboBox.SelectedItem as string;
            string sortDirection = SortDirectionComboBox.SelectedItem as string;

            var hotels = _hotelController.GetHotels(sortBy, sortDirection);

            HotelsDataGrid.ItemsSource = hotels;
        }
    }
}
