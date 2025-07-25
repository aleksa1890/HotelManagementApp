using HotelManagementApp.Controller; // Promena: sada koristimo Controller
using HotelManagementApp.Model;
using System;
using System.Windows;

namespace HotelManagementApp.View
{
    public partial class RegisterOwnerView : Window
    {
        private readonly UserController _userController; // Promena: sa UserService na UserController

        public RegisterOwnerView()
        {
            InitializeComponent();
            _userController = new UserController(); // Promena: instanciramo UserController
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(JmbgTextBox.Text) ||
                /* ... ostale provere ... */
                string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
            {
                MessageBox.Show("Sva polja su obavezna.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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
                // Pozivamo metodu iz kontrolera
                _userController.RegisterOwner(newOwner);
                MessageBox.Show("Novi vlasnik je uspešno registrovan.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Greška pri registraciji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do nepredviđene greške: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
