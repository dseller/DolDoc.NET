using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DolDoc.Examples.WebServer.Models;
using Microsoft.Extensions.Logging;

namespace DolDoc.Examples.WebServer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("Login")]
    public IActionResult Login()
    {
        var username = Request.Form["Username"].ToString();
        var password = Request.Form["Password"].ToString();
        
        return Content($"Welcome, $BK,1${username}$BK,0$", "text/doldoc");
    }
}
