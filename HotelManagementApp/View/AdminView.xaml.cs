using System.Windows;

namespace HotelManagementApp.View
{
    public partial class AdminView : Window
    {
        public AdminView()
        {
            InitializeComponent();
        }

        private void RegisterOwner_Click(object sender, RoutedEventArgs e)
        {
            var registerOwnerView = new RegisterOwnerView();
            registerOwnerView.ShowDialog(); // ShowDialog osigurava da je prozor modalan
        }
    }
}