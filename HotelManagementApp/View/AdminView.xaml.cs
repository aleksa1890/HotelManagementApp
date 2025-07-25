using HotelManagementApp.Controller;
using HotelManagementApp.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementApp.View
{
    public partial class AdminView : Window
    {
        private readonly UserController _userController;

        public AdminView()
        {
            InitializeComponent();
            _userController = new UserController();

            // Set default selections for ComboBoxes
            UserTypeFilterComboBox.SelectedIndex = 0; // All
            SortByComboBox.SelectedIndex = 0; // First Name
            SortDirectionComboBox.SelectedIndex = 0; // Ascending

            LoadUsers();
        }

        private void LoadUsers()
        {
            ApplyFilterSort_Click(null, null);
        }

        private void ApplyFilterSort_Click(object sender, RoutedEventArgs e)
        {
            string userTypeStr = (UserTypeFilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string sortBy = (SortByComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string sortDirection = (SortDirectionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            UserType? filterType = null;
            if (userTypeStr == "Guest")
            {
                filterType = UserType.Guest;
            }
            else if (userTypeStr == "Owner")
            {
                filterType = UserType.Owner;
            }

            var users = _userController.GetUsers(filterType, sortBy, sortDirection);

            UsersDataGrid.ItemsSource = users;
        }

        private void RegisterOwner_Click(object sender, RoutedEventArgs e)
        {
            var registerOwnerView = new RegisterOwnerView();
            registerOwnerView.ShowDialog();

            LoadUsers();
        }


        private void UsersDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // Proveri da li je neki korisnik selektovan
            User selectedUser = UsersDataGrid.SelectedItem as User;

            if (selectedUser != null)
            {
                BlockUnblockMenuItem.IsEnabled = true;
                // Dinamički postavi tekst menija u zavisnosti od statusa korisnika
                BlockUnblockMenuItem.Header = selectedUser.IsBlocked ? "Unblock" : "Block";
            }
            else
            {
                // Ako nijedan red nije selektovan, onemogući opciju
                BlockUnblockMenuItem.IsEnabled = false;
            }
        }

        private void BlockUnblock_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = UsersDataGrid.SelectedItem as User;

            if (selectedUser != null)
            {
                // Pozovi kontroler da promeni status
                _userController.ToggleUserBlockStatus(selectedUser.Jmbg);

                // Osveži prikaz u tabeli da se vidi promena
                LoadUsers();
            }
        }
    }
}