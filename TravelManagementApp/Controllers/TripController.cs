using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Constants;
using TravelDataAccess.Data;
using TravelDataAccess.Entities;
using TravelManagementApp.Filters;

namespace TravelManagementApp.Controllers;

[RequireLogin]
public class TripController(TravelDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var trips = await context.Trips
            .AsNoTracking()
            .OrderBy(t => t.ID)
            .ToListAsync();

        return View(trips);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Statuses = StatusValues.TripStatuses;
        return View(new Trip { Status = StatusValues.Available });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Trip trip)
    {
        if (!StatusValues.TripStatuses.Contains(trip.Status))
        {
            ModelState.AddModelError(nameof(Trip.Status), "Invalid status.");
        }

        if (await context.Trips.AnyAsync(t => t.Code == trip.Code))
        {
            ModelState.AddModelError(nameof(Trip.Code), "Code already exists.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Statuses = StatusValues.TripStatuses;
            return View(trip);
        }

        context.Trips.Add(trip);
        await context.SaveChangesAsync();
        TempData["Success"] = "Trip created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var trip = await context.Trips.FindAsync(id);
        if (trip is null)
        {
            return NotFound();
        }

        ViewBag.Statuses = StatusValues.TripStatuses;
        return View(trip);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Trip model)
    {
        if (id != model.ID)
        {
            return BadRequest();
        }

        var trip = await context.Trips.FindAsync(id);
        if (trip is null)
        {
            return NotFound();
        }

        if (!StatusValues.TripStatuses.Contains(model.Status))
        {
            ModelState.AddModelError(nameof(Trip.Status), "Invalid status.");
        }

        if (await context.Trips.AnyAsync(t => t.Code == model.Code && t.ID != model.ID))
        {
            ModelState.AddModelError(nameof(Trip.Code), "Code already exists.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Statuses = StatusValues.TripStatuses;
            return View(model);
        }

        trip.Code = model.Code;
        trip.Destination = model.Destination;
        trip.Price = model.Price;
        trip.Status = model.Status;

        await context.SaveChangesAsync();
        TempData["Success"] = "Trip updated successfully.";
        return RedirectToAction(nameof(Index));
    }
}