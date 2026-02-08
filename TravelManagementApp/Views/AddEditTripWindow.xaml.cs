using System.Windows;
using System.Windows.Controls;
using TravelManagementApp.Services;
using TravelDataAccess.Models;

namespace TravelManagementApp.Views
{
    public partial class AddEditTripWindow : Window
    {
        private readonly TripService _tripService;
        private Trip? _trip;
        private bool _isEditMode;

        public AddEditTripWindow(Trip? trip = null)
        {
            InitializeComponent();
            _tripService = new TripService();
            _trip = trip;
            _isEditMode = trip != null;

            if (_isEditMode && _trip != null)
            {
                Title = "Edit Trip";
                txtCode.Text = _trip.Code;
                txtDestination.Text = _trip.Destination;
                txtPrice.Text = _trip.Price.ToString();
                cboStatus.Text = _trip.Status;
            }
            else
            {
                Title = "Add Trip";
                cboStatus.SelectedIndex = 0;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = string.Empty;

            // Validation
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                lblError.Content = "Code is required";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                lblError.Content = "Destination is required";
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                lblError.Content = "Price must be a valid number >= 0";
                return;
            }

            if (cboStatus.SelectedItem == null)
            {
                lblError.Content = "Please select a status";
                return;
            }

            string code = txtCode.Text.Trim();
            string destination = txtDestination.Text.Trim();
            string status = ((ComboBoxItem)cboStatus.SelectedItem).Content.ToString()!;

            // Check unique code
            if (_isEditMode)
            {
                if (_tripService.CodeExists(code, _trip!.TripID))
                {
                    lblError.Content = "Code already exists";
                    return;
                }
            }
            else
            {
                if (_tripService.CodeExists(code))
                {
                    lblError.Content = "Code already exists";
                    return;
                }
            }

            bool success;
            if (_isEditMode)
            {
                _trip!.Code = code;
                _trip.Destination = destination;
                _trip.Price = price;
                _trip.Status = status;
                success = _tripService.UpdateTrip(_trip);
            }
            else
            {
                Trip newTrip = new Trip
                {
                    Code = code,
                    Destination = destination,
                    Price = price,
                    Status = status
                };
                success = _tripService.AddTrip(newTrip);
            }

            if (success)
            {
                MessageBox.Show("Trip saved successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to save trip.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}