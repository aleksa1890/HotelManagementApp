using HotelManagementApp.Controller;
using HotelManagementApp.Model;
using System;
using System.Linq;
using System.Windows;

namespace HotelManagementApp.View
{
    public partial class AddHotelView : Window
    {
        private readonly HotelController _hotelController;
        private readonly UserController _userController;

        public AddHotelView()
        {
            InitializeComponent();
            _hotelController = new HotelController();
            _userController = new UserController();
            LoadOwners();
        }

        private void LoadOwners()
        {
            // Dobavljamo samo korisnike koji su tipa 'Owner'
            var owners = _userController.GetUsers(UserType.Owner, null, null);
            OwnerComboBox.ItemsSource = owners;
        }

        private void AddHotel_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CodeTextBox.Text) ||
                string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(YearBuiltTextBox.Text) ||
                string.IsNullOrWhiteSpace(StarsTextBox.Text) ||
                OwnerComboBox.SelectedItem == null)
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newHotel = new Hotel
            {
                Code = CodeTextBox.Text.Trim(),
                Name = NameTextBox.Text.Trim(),
                YearBuilt = int.Parse(YearBuiltTextBox.Text.Trim()),
                Stars = int.Parse(StarsTextBox.Text.Trim()),
                OwnerJmbg = OwnerComboBox.SelectedValue.ToString()
            };

            try
            {
                _hotelController.CreateHotel(newHotel);
                MessageBox.Show("Hotel successfully created. It is now pending owner approval.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (ArgumentException ex)
            {
                // Hvatamo grešku ako hotel sa tom šifrom već postoji
                MessageBox.Show(ex.Message, "Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}