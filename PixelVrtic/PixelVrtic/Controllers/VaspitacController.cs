using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        // Get current user's ID
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        // If not admin, enforce access to self only
        if (!User.IsInRole("Administrator") && id != currentUserId)
            return Forbid();

        var korisnik = await _userManager.FindByIdAsync(id);
        if (korisnik == null)
            return NotFound();

        return View(korisnik);
    }



    // GET: Roditelji/Edit/5
    [Authorize] // Allow all authenticated users
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId != id && !User.IsInRole("Administrator"))
            return Forbid();

        var korisnik = await _userManager.FindByIdAsync(id);
        if (korisnik == null)
            return NotFound();

        return View(korisnik);
    }

    // POST: Roditelji/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize] // Allow all authenticated users
    public async Task<IActionResult> Edit(string id, Korisnik korisnik)
    {
        if (id != korisnik.Id)
            return NotFound();

        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId != id && !User.IsInRole("Administrator"))
            return Forbid();

        if (!ModelState.IsValid)
            return View(korisnik);

        var postojeci = await _userManager.FindByIdAsync(id);
        if (postojeci == null)
            return NotFound();

        // Update allowed fields
        postojeci.ime = korisnik.ime;
        postojeci.prezime = korisnik.prezime;
        postojeci.datumRodjenja = korisnik.datumRodjenja;
        postojeci.grad = korisnik.grad;
        postojeci.brojTelefona = korisnik.brojTelefona;
        postojeci.Email = korisnik.Email;
        postojeci.UserName = korisnik.Email;

        try
        {
            await _userManager.UpdateAsync(postojeci);
            return RedirectToAction(nameof(Details), "Vaspitac", new { id = korisnik.Id });
        }
        catch (DbUpdateConcurrencyException)
        {
            ModelState.AddModelError("", "Dogodila se greška pri ažuriranju. Molimo pokušajte ponovo.");
            return View(korisnik);
        }
    }




}
