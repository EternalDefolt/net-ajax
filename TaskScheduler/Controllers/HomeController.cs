using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskScheduler.Models;
using TaskScheduler.Services;

namespace TaskScheduler.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITaskRepository _repo;

    public HomeController(ILogger<HomeController> logger, ITaskRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var tasks = await _repo.GetAllAsync(ct);
        return View(tasks);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
