using System.ComponentModel.DataAnnotations;

namespace TravelManagementApp.ViewModels;

public class UpdateBookingStatusViewModel
{
    public int BookingID { get; set; }
    public string TripCode { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;
}