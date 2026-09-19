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

    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task is null)
            return NotFound();

        return View(task);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ScheduledTask task, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(task);

        var created = await _repo.AddAsync(task, ct);
        return RedirectToAction(nameof(Details), new { id = created.Id });
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
