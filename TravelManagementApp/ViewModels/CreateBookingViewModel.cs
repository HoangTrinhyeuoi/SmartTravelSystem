using System.ComponentModel.DataAnnotations;
using TravelDataAccess.Entities;

namespace TravelManagementApp.ViewModels;

public class CreateBookingViewModel
{
    [Required]
    [Display(Name = "Trip")]
    public int TripID { get; set; }

    public List<Trip> TripOptions { get; set; } = [];
}