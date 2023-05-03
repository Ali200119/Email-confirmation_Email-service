using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Elearn.Models;
using Elearn.Data;
using Microsoft.EntityFrameworkCore;
using Elearn.ViewModels.Home;

namespace Elearn.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Course> courses = await _context.Course.Include(c => c.Author).Include(c => c.CourseImages).ToListAsync();

        HomeVM homeVM = new HomeVM
        {
            Courses = courses
        };

        return View(homeVM);
    }
}