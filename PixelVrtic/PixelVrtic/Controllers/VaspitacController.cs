using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelVrtic.Data;
using PixelVrtic.Models;

[Authorize]

public class VaspitacController : Controller
{
    private readonly ApplicationDbContext _context;

    public VaspitacController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var vaspitaci = _context.Korisnik
            .Where(k => (int)k.uloga == 1)
            .ToList();

        var grupe = _context.Grupa
            .ToDictionary(g => g.id, g => g.naziv);

        ViewBag.Grupe = grupe;

        return View(vaspitaci);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Korisnik korisnik)
    {
        if (ModelState.IsValid)
        {
            korisnik.uloga = (Uloga)1;
            _context.Add(korisnik);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(korisnik);
    }
}
