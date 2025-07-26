using HotelManagementApp.Model;
using HotelManagementApp.Service;
using System.Windows;

namespace HotelManagementApp.View
{
    public partial class LoginView : Window
    {
        private readonly UserService _userService;
        private int _loginAttempts = 3;

        public LoginView()
        {
            InitializeComponent();
            _userService = new UserService();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            User loggedInUser = _userService.Login(email, password);

            if (loggedInUser != null)
            {
                MessageBox.Show($"Welcome, {loggedInUser.FirstName}!", "Success");

                switch (loggedInUser.UserType)
                {
                    case UserType.Administrator:
                        var adminView = new AdminView();
                        adminView.Show();
                        break;

                    case UserType.Owner:
                        // OVA LINIJA JE ISPRAVLJENA
                        var ownerView = new OwnerView(loggedInUser);
                        ownerView.Show();
                        break;

                    case UserType.Guest:
                        var guestView = new GuestView();
                        guestView.Show();
                        break;
                }

                this.Close();
            }
            else
            {
                _loginAttempts--;
                if (_loginAttempts > 0)
                {
                    ErrorMessageTextBlock.Text = $"Invalid email or password. You have {_loginAttempts} attempts left.";
                }
                else
                {
                    ErrorMessageTextBlock.Text = "You have exceeded the maximum number of login attempts.";
                    MessageBox.Show("The application will now shut down.", "Login Failed");
                    Application.Current.Shutdown();
                }
            }
        }
    }
}