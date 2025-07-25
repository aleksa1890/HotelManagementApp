using HotelManagementApp.Model;
using HotelManagementApp.Service;
using System;
using System.Windows;

namespace HotelManagementApp.View
{
    public partial class RegisterOwnerView : Window
    {
        private readonly UserService _userService;

        public RegisterOwnerView()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Validacija - Provera da li su sva polja popunjena
            if (string.IsNullOrWhiteSpace(JmbgTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
            {
                MessageBox.Show("Sva polja su obavezna.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kreiranje novog User objekta
            var newOwner = new User
            {
                Jmbg = JmbgTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                Password = PasswordBox.Password,
                FirstName = FirstNameTextBox.Text.Trim(),
                LastName = LastNameTextBox.Text.Trim(),
                PhoneNumber = PhoneNumberTextBox.Text.Trim()
            };

            try
            {
                // Poziv servisa za registraciju
                _userService.RegisterOwner(newOwner);
                MessageBox.Show("Novi vlasnik je uspešno registrovan.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Zatvori prozor nakon uspešne registracije
            }
            catch (ArgumentException ex)
            {
                // Hvatanje greške ako JMBG ili email već postoje
                MessageBox.Show(ex.Message, "Greška pri registraciji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Hvatanje ostalih, nepredviđenih grešaka
                MessageBox.Show($"Došlo je do nepredviđene greške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
