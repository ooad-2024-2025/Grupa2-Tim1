using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PixelVrtic.Data;
using PixelVrtic.Models;

[Authorize]

public class VaspitacController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<Korisnik> _userManager;


    public VaspitacController(ApplicationDbContext context, UserManager<Korisnik> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [Authorize(Roles = "Administrator")]

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
    [Authorize(Roles = "Administrator")]


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]

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

    [Authorize(Roles = "Administrator,Vaspitac")]
    public IActionResult Dashboard()
    {
        var korisnik = _userManager.GetUserAsync(User);

        if (korisnik == null)
            return Unauthorized();

        var imePrezimeFormatted = $"Zdravo, {korisnik.Result.ime} {korisnik.Result.prezime}";
        ViewBag.ImePrezime = imePrezimeFormatted;

        return View();
    }


}
