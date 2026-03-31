using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Data;
using TravelManagementApp.Infrastructure;
using TravelManagementApp.ViewModels;

namespace TravelManagementApp.Controllers;

public class AccountController(TravelDbContext context) : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32(SessionKeys.CustomerId) is not null)
        {
            return RedirectToAction("Main", "Home");
        }

        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var customer = await context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Code == model.Code && c.Password == model.Password);

        if (customer is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid code or password.");
            return View(model);
        }

        HttpContext.Session.SetInt32(SessionKeys.CustomerId, customer.ID);
        HttpContext.Session.SetString(SessionKeys.CustomerCode, customer.Code);
        HttpContext.Session.SetString(SessionKeys.CustomerName, customer.FullName);

        return RedirectToAction("Main", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}