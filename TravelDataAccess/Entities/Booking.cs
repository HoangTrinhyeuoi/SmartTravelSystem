namespace TravelDataAccess.Entities;

public class Booking
{
    public int ID { get; set; }
    public int TripID { get; set; }
    public int CustomerID { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = string.Empty;

    public Trip? Trip { get; set; }
    public Customer? Customer { get; set; }
}