using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TravelManagementApp.Filters;
using TravelManagementApp.Models;

namespace TravelManagementApp.Controllers;

public class HomeController : Controller
{
    [RequireLogin]
    public IActionResult Main()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
