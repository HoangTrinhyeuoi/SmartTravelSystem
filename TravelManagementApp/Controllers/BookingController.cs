using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Constants;
using TravelDataAccess.Data;
using TravelDataAccess.Entities;
using TravelManagementApp.Filters;
using TravelManagementApp.Infrastructure;
using TravelManagementApp.ViewModels;

namespace TravelManagementApp.Controllers;

[RequireLogin]
public class BookingController(TravelDbContext context) : Controller
{
    public async Task<IActionResult> Index(bool pendingOnly = false)
    {
        var customerId = HttpContext.Session.GetInt32(SessionKeys.CustomerId);
        if (customerId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var query = context.Bookings
            .AsNoTracking()
            .Include(b => b.Trip)
            .Where(b => b.CustomerID == customerId);

        if (pendingOnly)
        {
            query = query.Where(b => b.Status == StatusValues.Pending);
        }

        var bookings = await query
            .OrderBy(b => b.BookingDate)
            .ToListAsync();

        ViewBag.PendingOnly = pendingOnly;
        return View(bookings);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var trips = await context.Trips
            .AsNoTracking()
            .Where(t => t.Status == StatusValues.Available)
            .OrderBy(t => t.Destination)
            .ToListAsync();

        var model = new CreateBookingViewModel
        {
            TripOptions = trips
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel model)
    {
        var customerId = HttpContext.Session.GetInt32(SessionKeys.CustomerId);
        if (customerId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var trip = await context.Trips.FindAsync(model.TripID);
        if (trip is null || trip.Status != StatusValues.Available)
        {
            ModelState.AddModelError(nameof(CreateBookingViewModel.TripID), "Selected trip is not available.");
        }

        if (!ModelState.IsValid)
        {
            model.TripOptions = await context.Trips
                .AsNoTracking()
                .Where(t => t.Status == StatusValues.Available)
                .OrderBy(t => t.Destination)
                .ToListAsync();

            return View(model);
        }

        var booking = new Booking
        {
            TripID = model.TripID,
            CustomerID = customerId.Value,
            BookingDate = DateTime.Today,
            Status = StatusValues.Pending
        };

        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        TempData["Success"] = "Booking created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditStatus(int id)
    {
        var customerId = HttpContext.Session.GetInt32(SessionKeys.CustomerId);
        if (customerId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var booking = await context.Bookings
            .AsNoTracking()
            .Include(b => b.Trip)
            .FirstOrDefaultAsync(b => b.ID == id && b.CustomerID == customerId);

        if (booking is null)
        {
            return NotFound();
        }

        var model = new UpdateBookingStatusViewModel
        {
            BookingID = booking.ID,
            TripCode = booking.Trip?.Code ?? string.Empty,
            Destination = booking.Trip?.Destination ?? string.Empty,
            BookingDate = booking.BookingDate,
            Status = booking.Status
        };

        ViewBag.Statuses = StatusValues.BookingStatuses;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStatus(UpdateBookingStatusViewModel model)
    {
        var customerId = HttpContext.Session.GetInt32(SessionKeys.CustomerId);
        if (customerId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!StatusValues.BookingStatuses.Contains(model.Status))
        {
            ModelState.AddModelError(nameof(UpdateBookingStatusViewModel.Status), "Invalid status.");
        }

        var booking = await context.Bookings
            .Include(b => b.Trip)
            .FirstOrDefaultAsync(b => b.ID == model.BookingID && b.CustomerID == customerId);

        if (booking is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.TripCode = booking.Trip?.Code ?? string.Empty;
            model.Destination = booking.Trip?.Destination ?? string.Empty;
            model.BookingDate = booking.BookingDate;
            ViewBag.Statuses = StatusValues.BookingStatuses;
            return View(model);
        }

        booking.Status = model.Status;
        await context.SaveChangesAsync();

        TempData["Success"] = "Booking status updated successfully.";
        return RedirectToAction(nameof(Index));
    }
}