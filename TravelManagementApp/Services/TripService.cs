using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Data;
using TravelDataAccess.Models;

namespace TravelManagementApp.Services
{
    public class TripService
    {
        private readonly TravelDbContext _context;

        public TripService()
        {
            _context = new TravelDbContext();
        }

        public List<Trip> GetAllTrips()
        {
            return _context.Trips.ToList();
        }

        public List<Trip> GetAvailableTrips()
        {
            return _context.Trips.Where(t => t.Status == "Available").ToList();
        }

        public bool AddTrip(Trip trip)
        {
            try
            {
                _context.Trips.Add(trip);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTrip(Trip trip)
        {
            try
            {
                _context.Entry(trip).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CodeExists(string code, int? excludeTripId = null)
        {
            if (excludeTripId.HasValue)
            {
                return _context.Trips.Any(t => t.Code == code && t.TripID != excludeTripId.Value);
            }
            return _context.Trips.Any(t => t.Code == code);
        }
    }
}