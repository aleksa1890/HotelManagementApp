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

            SortByComboBox.SelectedIndex = 0;
            SortDirectionComboBox.SelectedIndex = 0;
            SearchByComboBox.SelectedIndex = 0;

            LoadHotels();
        }

        private void LoadHotels()
        {
            ApplySort_Click(null, null);
        }

        private void ApplySort_Click(object sender, RoutedEventArgs e)
        {
            // ISPRAVLJEN NAČIN ČITANJA VREDNOSTI
            string sortBy = SortByComboBox.Text;
            string sortDirection = SortDirectionComboBox.Text;

            var hotels = _hotelController.GetHotels(sortBy, sortDirection);
            HotelsDataGrid.ItemsSource = hotels;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // ISPRAVLJEN NAČIN ČITANJA VREDNOSTI
            string searchBy = SearchByComboBox.Text;
            string searchValue = SearchValueTextBox.Text;

            var hotels = _hotelController.SearchHotels(searchBy, searchValue, null, null, null);
            HotelsDataGrid.ItemsSource = hotels;
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchValueTextBox.Clear();
            RoomsTextBox.Clear();
            GuestsTextBox.Clear();
            OperatorTextBox.Text = "&";
            LoadHotels();
        }

        private void SearchByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchByComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string searchBy = selectedItem.Content.ToString();
                if (searchBy == "Apartments")
                {
                    StandardSearchPanel.Visibility = Visibility.Collapsed;
                    ApartmentSearchPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    StandardSearchPanel.Visibility = Visibility.Visible;
                    ApartmentSearchPanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ApartmentSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string rooms = RoomsTextBox.Text;
            string guests = GuestsTextBox.Text;
            string op = OperatorTextBox.Text;

            var hotels = _hotelController.SearchHotels("Apartments", null, rooms, guests, op);
            HotelsDataGrid.ItemsSource = hotels;
        }
    }
}