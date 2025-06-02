using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PixelVrtic.Data;
using PixelVrtic.Models;

[Authorize]
public class RoditeljController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoditeljController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var roditelji = _context.Korisnik
            .Where(k => (int)k.uloga == 2)
            .ToList();

        return View(roditelji);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Korisnik korisnik)
    {
        if (ModelState.IsValid)
        {
            korisnik.uloga = (Uloga)2;
            _context.Add(korisnik);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(korisnik);
    }
}
