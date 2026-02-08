using System.Windows;
using System.Windows.Controls;
using TravelManagementApp.Services;
using TravelDataAccess.Models;

namespace TravelManagementApp.Views
{
    public partial class MyBookingsWindow : Window
    {
        private readonly BookingService _bookingService;
        private readonly Customer _currentCustomer;
        private bool _isUpdating = false;

        public MyBookingsWindow(Customer customer)
        {
            InitializeComponent();
            _bookingService = new BookingService();
            _currentCustomer = customer;
            LoadAllBookings();
        }

        private void LoadAllBookings()
        {
            var bookings = _bookingService.GetBookingsByCustomer(_currentCustomer.CustomerID);
            dgBookings.ItemsSource = null; // Clear first
            dgBookings.ItemsSource = bookings;
        }

        private void BtnFilterPending_Click(object sender, RoutedEventArgs e)
        {
            var pendingBookings = _bookingService.GetPendingBookingsByCustomer(_currentCustomer.CustomerID);
            dgBookings.ItemsSource = null;
            dgBookings.ItemsSource = pendingBookings;
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            LoadAllBookings();
        }

        private void CboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdating) return; // Prevent recursive calls
            
            if (sender is ComboBox comboBox && comboBox.DataContext is Booking booking)
            {
                if (comboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    string newStatus = selectedItem.Content.ToString()!;
                    
                    // Check if status actually changed
                    if (newStatus == booking.Status)
                        return;

                    _isUpdating = true;
                    
                    if (_bookingService.UpdateBookingStatus(booking.BookingID, newStatus))
                    {
                        MessageBox.Show($"Booking #{booking.BookingID} status updated to {newStatus}!", 
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // Reload data to reflect changes
                        LoadAllBookings();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update booking status.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        
                        // Reload to revert changes
                        LoadAllBookings();
                    }
                    
                    _isUpdating = false;
                }
            }
        }

        private void BtnCreateBooking_Click(object sender, RoutedEventArgs e)
        {
            CreateBookingWindow createWindow = new CreateBookingWindow(_currentCustomer);
            if (createWindow.ShowDialog() == true)
            {
                LoadAllBookings();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}