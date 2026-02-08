using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Data;
using TravelDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelManagementApp.Services
{
    public class BookingService
    {
        private TravelDbContext _context;

        public BookingService()
        {
            _context = new TravelDbContext();
        }

        public List<Booking> GetBookingsByCustomer(int customerId)
        {
            // Recreate context to avoid caching issues
            _context = new TravelDbContext();
            
            return _context.Bookings
                .AsNoTracking() // Don't track entities
                .Include(b => b.Trip)
                .Include(b => b.Customer)
                .Where(b => b.CustomerID == customerId)
                .ToList();
        }

        public List<Booking> GetPendingBookingsByCustomer(int customerId)
        {
            // Recreate context to avoid caching issues
            _context = new TravelDbContext();
            
            return _context.Bookings
                .AsNoTracking()
                .Include(b => b.Trip)
                .Include(b => b.Customer)
                .Where(b => b.CustomerID == customerId && b.Status == "Pending")
                .OrderBy(b => b.BookingDate)
                .ToList();
        }

        public bool UpdateBookingStatus(int bookingId, string status)
        {
            try
            {
                // Use a fresh context for update
                using (var updateContext = new TravelDbContext())
                {
                    // Validate status
                    if (status != "Pending" && status != "Confirmed" && status != "Cancelled")
                    {
                        throw new ArgumentException($"Invalid status: {status}");
                    }

                    var booking = updateContext.Bookings.Find(bookingId);
                    if (booking == null) 
                    {
                        throw new InvalidOperationException($"Booking {bookingId} not found");
                    }

                    booking.Status = status;
                    
                    var changes = updateContext.SaveChanges();
                    return changes > 0;
                }
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error updating booking status: {ex.Message}");
                System.Windows.MessageBox.Show($"Error: {ex.Message}", "Update Error", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        public bool CreateBooking(Booking booking)
        {
            try
            {
                using (var createContext = new TravelDbContext())
                {
                    createContext.Bookings.Add(booking);
                    createContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating booking: {ex.Message}");
                return false;
            }
        }
    }
}