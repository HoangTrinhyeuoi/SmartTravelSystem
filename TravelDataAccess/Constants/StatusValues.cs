namespace TravelDataAccess.Constants;

public static class StatusValues
{
    public const string Pending = "Pending";
    public const string Confirmed = "Confirmed";
    public const string Cancelled = "Cancelled";

    public const string Available = "Available";
    public const string Booked = "Booked";

    public static readonly string[] BookingStatuses = [Pending, Confirmed, Cancelled];
    public static readonly string[] TripStatuses = [Available, Booked];
}