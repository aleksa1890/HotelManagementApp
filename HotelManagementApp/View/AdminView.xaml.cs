using HotelManagementApp.Controller;
using HotelManagementApp.Model;
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
            // Postavljanje podrazumevanih vrednosti
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
            string userTypeStr = UserTypeFilterComboBox.Text;
            string sortBy = SortByComboBox.Text;
            string sortDirection = SortDirectionComboBox.Text;

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
            // Postavljanje podrazumevanih vrednosti
            HotelSortByComboBox.SelectedIndex = 0;
            HotelSortDirectionComboBox.SelectedIndex = 0;
            HotelSearchByComboBox.SelectedIndex = 0;
        }

        private void LoadHotels()
        {
            ApplyHotelSort_Click(null, null);
        }

        private void ApplyHotelSort_Click(object sender, RoutedEventArgs e)
        {
            string sortBy = HotelSortByComboBox.Text;
            string sortDirection = HotelSortDirectionComboBox.Text;

            HotelsDataGrid.ItemsSource = _hotelController.GetHotels(sortBy, sortDirection);
        }

        private void HotelSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchBy = HotelSearchByComboBox.Text;
            string searchValue = HotelSearchValueTextBox.Text;
            HotelsDataGrid.ItemsSource = _hotelController.SearchHotels(searchBy, searchValue, null, null, null);
        }

        private void HotelClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            HotelSearchValueTextBox.Clear();
            HotelRoomsTextBox.Clear();
            HotelGuestsTextBox.Clear();
            HotelOperatorTextBox.Text = "&";
            LoadHotels();
        }

        private void HotelSearchByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HotelSearchByComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string searchBy = selectedItem.Content.ToString();
                if (searchBy == "Apartments")
                {
                    HotelStandardSearchPanel.Visibility = Visibility.Collapsed;
                    HotelApartmentSearchPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    HotelStandardSearchPanel.Visibility = Visibility.Visible;
                    HotelApartmentSearchPanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void HotelApartmentSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string rooms = HotelRoomsTextBox.Text;
            string guests = HotelGuestsTextBox.Text;
            string op = HotelOperatorTextBox.Text;
            HotelsDataGrid.ItemsSource = _hotelController.SearchHotels("Apartments", null, rooms, guests, op);
        }

        private void AddHotel_Click(object sender, RoutedEventArgs e)
        {
            var addHotelView = new AddHotelView();
            addHotelView.ShowDialog();
            // Ne moramo da zovemo LoadHotels() jer novi hotel svakako nije odobren i neće se videti
        }

        #endregion
    }
}