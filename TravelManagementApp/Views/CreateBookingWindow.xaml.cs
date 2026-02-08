using System;
using System.Windows;
using TravelManagementApp.Services;
using TravelDataAccess.Models;

namespace TravelManagementApp.Views
{
    public partial class CreateBookingWindow : Window
    {
        private readonly BookingService _bookingService;
        private readonly TripService _tripService;
        private readonly Customer _currentCustomer;

        public CreateBookingWindow(Customer customer)
        {
            InitializeComponent();
            _bookingService = new BookingService();
            _tripService = new TripService();
            _currentCustomer = customer;

            dpBookingDate.SelectedDate = DateTime.Now;
            LoadAvailableTrips();
        }

        private void LoadAvailableTrips()
        {
            var availableTrips = _tripService.GetAvailableTrips();
            dgAvailableTrips.ItemsSource = availableTrips;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = string.Empty;

            if (dgAvailableTrips.SelectedItem is not Trip selectedTrip)
            {
                lblError.Content = "Please select a trip";
                return;
            }

            Booking newBooking = new Booking
            {
                TripID = selectedTrip.TripID,
                CustomerID = _currentCustomer.CustomerID,
                BookingDate = dpBookingDate.SelectedDate ?? DateTime.Now,
                Status = "Pending"
            };

            if (_bookingService.CreateBooking(newBooking))
            {
                MessageBox.Show("Booking created successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to create booking.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}