using HotelManagementApp.Controller;
using HotelManagementApp.Model;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementApp.View
{
    public partial class OwnerView : Window
    {
        private readonly HotelController _hotelController;
        private readonly User _loggedInOwner;

        public OwnerView(User loggedInOwner)
        {
            InitializeComponent();
            _hotelController = new HotelController();
            _loggedInOwner = loggedInOwner;
            Title = $"Welcome, {loggedInOwner.FirstName}";

            // Inicijalizacija oba taba
            InitializeAllHotelsTab();
            LoadAllHotels();
            LoadMyHotels();
        }

        #region All Hotels Tab Logic

        private void InitializeAllHotelsTab()
        {
            SortByComboBox.SelectedIndex = 0;
            SortDirectionComboBox.SelectedIndex = 0;
            SearchByComboBox.SelectedIndex = 0;
        }

        private void LoadAllHotels()
        {
            ApplySort_Click(null, null);
        }

        private void ApplySort_Click(object sender, RoutedEventArgs e)
        {
            string sortBy = SortByComboBox.Text;
            string sortDirection = SortDirectionComboBox.Text;
            AllHotelsDataGrid.ItemsSource = _hotelController.GetHotels(sortBy, sortDirection);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchBy = SearchByComboBox.Text;
            string searchValue = SearchValueTextBox.Text;
            AllHotelsDataGrid.ItemsSource = _hotelController.SearchHotels(searchBy, searchValue);
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchValueTextBox.Clear();
            RoomsTextBox.Clear();
            GuestsTextBox.Clear();
            OperatorTextBox.Text = "&";
            LoadAllHotels();
        }

        private void SearchByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchByComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content.ToString() == "Apartments")
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
            AllHotelsDataGrid.ItemsSource = _hotelController.SearchHotels("Apartments", null, rooms, guests, op);
        }

        #endregion

        #region My Hotels Tab Logic

        private void LoadMyHotels()
        {
            MyHotelsDataGrid.ItemsSource = _hotelController.GetHotelsForOwner(_loggedInOwner.Jmbg);
        }

        private void MyHotelsDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Hotel selectedHotel = MyHotelsDataGrid.SelectedItem as Hotel;
            bool isPending = selectedHotel != null && !selectedHotel.IsApproved;
            ApproveMenuItem.IsEnabled = isPending;
            DenyMenuItem.IsEnabled = isPending;
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            Hotel selectedHotel = MyHotelsDataGrid.SelectedItem as Hotel;
            if (selectedHotel != null)
            {
                _hotelController.ApproveHotel(selectedHotel.Code);
                // Osveži oba prikaza, jer odobren hotel postaje vidljiv svima
                LoadMyHotels();
                LoadAllHotels();
            }
        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {
            Hotel selectedHotel = MyHotelsDataGrid.SelectedItem as Hotel;
            if (selectedHotel != null)
            {
                _hotelController.DenyHotel(selectedHotel.Code);
                // Osveži prikaz samo "Mojih hotela" jer je hotel obrisan
                LoadMyHotels();
            }
        }

        #endregion
    }
}