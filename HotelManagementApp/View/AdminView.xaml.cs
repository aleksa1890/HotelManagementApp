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
        private readonly HotelController _hotelController;

        public AdminView()
        {
            InitializeComponent();
            _userController = new UserController();
            _hotelController = new HotelController();

            InitializeUserTab();
            InitializeHotelTab();

            LoadUsers();
            LoadHotels();
        }

        #region User Management
        private void InitializeUserTab()
        {
            UserTypeFilterComboBox.ItemsSource = new string[] { "All", "Guest", "Owner" };
            SortByComboBox.ItemsSource = new string[] { "First Name", "Last Name" };
            SortDirectionComboBox.ItemsSource = new string[] { "Ascending", "Descending" };

            UserTypeFilterComboBox.SelectedIndex = 0;
            SortByComboBox.SelectedIndex = 0;
            SortDirectionComboBox.SelectedIndex = 0;
        }

        private void LoadUsers()
        {
            ApplyUserFilterSort_Click(null, null);
        }

        private void ApplyUserFilterSort_Click(object sender, RoutedEventArgs e)
        {
            string userTypeStr = UserTypeFilterComboBox.SelectedItem as string;
            string sortBy = SortByComboBox.SelectedItem as string;
            string sortDirection = SortDirectionComboBox.SelectedItem as string;

            UserType? filterType = null;
            if (userTypeStr == "Guest") filterType = UserType.Guest;
            else if (userTypeStr == "Owner") filterType = UserType.Owner;

            UsersDataGrid.ItemsSource = _userController.GetUsers(filterType, sortBy, sortDirection);
        }

        private void RegisterOwner_Click(object sender, RoutedEventArgs e)
        {
            var registerOwnerView = new RegisterOwnerView();
            registerOwnerView.ShowDialog();
            LoadUsers();
        }

        private void UsersDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            User selectedUser = UsersDataGrid.SelectedItem as User;
            BlockUnblockMenuItem.IsEnabled = selectedUser != null;
            if (selectedUser != null)
            {
                BlockUnblockMenuItem.Header = selectedUser.IsBlocked ? "Unblock" : "Block";
            }
        }

        private void BlockUnblock_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = UsersDataGrid.SelectedItem as User;
            if (selectedUser != null)
            {
                _userController.ToggleUserBlockStatus(selectedUser.Jmbg);
                LoadUsers();
            }
        }
        #endregion

        #region Hotel Management
        private void InitializeHotelTab()
        {
            HotelSortByComboBox.ItemsSource = new string[] { "Name", "Stars" };
            HotelSortDirectionComboBox.ItemsSource = new string[] { "Ascending", "Descending" };

            HotelSortByComboBox.SelectedIndex = 0;
            HotelSortDirectionComboBox.SelectedIndex = 0;
        }

        private void LoadHotels()
        {
            ApplyHotelSort_Click(null, null);
        }

        private void ApplyHotelSort_Click(object sender, RoutedEventArgs e)
        {
            string sortBy = HotelSortByComboBox.SelectedItem as string;
            string sortDirection = HotelSortDirectionComboBox.SelectedItem as string;

            HotelsDataGrid.ItemsSource = _hotelController.GetHotels(sortBy, sortDirection);
        }
        #endregion
    }
}