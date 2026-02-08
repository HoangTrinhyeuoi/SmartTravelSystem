using System.Windows;
using System.Windows.Input;
using TravelManagementApp.Services;
using TravelDataAccess.Models;

namespace TravelManagementApp.Views
{
    public partial class TripListWindow : Window
    {
        private readonly TripService _tripService;

        public TripListWindow()
        {
            InitializeComponent();
            _tripService = new TripService();
            LoadTrips();
        }

        private void LoadTrips()
        {
            dgTrips.ItemsSource = _tripService.GetAllTrips();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditTripWindow addWindow = new AddEditTripWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadTrips();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgTrips.SelectedItem is Trip selectedTrip)
            {
                AddEditTripWindow editWindow = new AddEditTripWindow(selectedTrip);
                if (editWindow.ShowDialog() == true)
                {
                    LoadTrips();
                }
            }
            else
            {
                MessageBox.Show("Please select a trip to edit.", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DgTrips_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BtnEdit_Click(sender, e);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}