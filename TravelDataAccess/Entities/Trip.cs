namespace TravelDataAccess.Entities;

public class Trip
{
    public int ID { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}