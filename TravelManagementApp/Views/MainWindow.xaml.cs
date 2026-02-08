using System.Windows;
using TravelDataAccess.Models;

namespace TravelManagementApp.Views
{
    public partial class MainWindow : Window
    {
        private Customer _currentCustomer;

        public MainWindow(Customer customer)
        {
            InitializeComponent();
            _currentCustomer = customer;
            txtWelcome.Text = $"Welcome, {_currentCustomer.FullName}!";
        }

        private void BtnTripList_Click(object sender, RoutedEventArgs e)
        {
            TripListWindow tripListWindow = new TripListWindow();
            tripListWindow.ShowDialog();
        }

        private void BtnMyBookings_Click(object sender, RoutedEventArgs e)
        {
            MyBookingsWindow myBookingsWindow = new MyBookingsWindow(_currentCustomer);
            myBookingsWindow.ShowDialog();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}