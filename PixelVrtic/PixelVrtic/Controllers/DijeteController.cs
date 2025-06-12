using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PixelVrtic.Data;
using PixelVrtic.Models;

namespace PixelVrtic.Controllers
{
    [Authorize]
    public class DijeteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public DijeteController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Dijete
        [Authorize(Roles = "Administrator, Vaspitac")]
        public async Task<IActionResult> Index(int? idGrupe, string search)
        {
            var dijeteQuery = _context.Dijete
                .Include(d => d.Korisnik)
                .Include(d => d.grupa)
                .AsQueryable();

            if (idGrupe.HasValue)
            {
                dijeteQuery = dijeteQuery.Where(d => d.grupaId == idGrupe.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                dijeteQuery = dijeteQuery.Where(d =>
                    d.ime.ToLower().Contains(search.ToLower()) ||
                    d.prezime.ToLower().Contains(search.ToLower()) ||
                    d.Korisnik.ime.ToLower().Contains(search.ToLower()) ||
                    d.Korisnik.prezime.ToLower().Contains(search.ToLower()));
            }

            var dijeca = await dijeteQuery.ToListAsync();
            return View(dijeca);
        }

        // GET: Dijete/Details/5
        [Authorize(Roles = "Administrator, Vaspitac, Roditelj")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dijete = await _context.Dijete
                .Include(d => d.Korisnik)
                .Include(d => d.grupa)
                .FirstOrDefaultAsync(m => m.id == id);

            if (dijete == null)
                return NotFound();

            return View(dijete);
        }

        // GET: Dijete/Create
        [Authorize(Roles = "Administrator, Vaspitac")]
        public IActionResult Create()
        {
            UcitajRoditeljeUViewData();
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "naziv");
            return View();
        }

        // POST: Dijete/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> Create([Bind("id,ime,prezime,datumRodjenja,mjestoRodenja,JMBG,grupaId,zdravstveneNapomene,fotografija,roditeljId")] Dijete dijete)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dijete);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            UcitajRoditeljeUViewData();
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "naziv", dijete.grupaId);
            return View(dijete);
        }

        // GET: Dijete/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dijete = await _context.Dijete.FindAsync(id);
            if (dijete == null)
                return NotFound();

            // Provjera dozvole
            if (!User.IsInRole("Administrator") && !User.IsInRole("Vaspitac"))
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (dijete.roditeljId != userId)
                    return Forbid(); // Ili RedirectToAction("AccessDenied", "Account");
            }

            UcitajRoditeljeUViewData(dijete.roditeljId);
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "naziv", dijete.grupaId);
            return View(dijete);
        }


        // POST: Dijete/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ime,prezime,datumRodjenja,mjestoRodenja,JMBG,grupaId,zdravstveneNapomene,fotografija,roditeljId")] Dijete dijete)
        {
            if (id != dijete.id)
                return NotFound();

            // Učitaj originalno dijete iz baze radi provjere vlasništva
            var originalDijete = await _context.Dijete.AsNoTracking().FirstOrDefaultAsync(d => d.id == id);
            if (originalDijete == null)
                return NotFound();

            if (!User.IsInRole("Administrator") && !User.IsInRole("Vaspitac"))
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (originalDijete.roditeljId != userId)
                    return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dijete);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DijeteExists(dijete.id))
                        return NotFound();
                    else
                        throw;
                }
                if (User.IsInRole("Roditelj"))
                {
                    return RedirectToAction("Details", new { id = dijete.id });
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }

            UcitajRoditeljeUViewData(dijete.roditeljId);
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "naziv", dijete.grupaId);
            return View(dijete);
        }


        // GET: Dijete/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dijete = await _context.Dijete
                .Include(d => d.Korisnik)
                .Include(d => d.grupa)
                .FirstOrDefaultAsync(m => m.id == id);

            if (dijete == null)
                return NotFound();

            return View(dijete);
        }

        // POST: Dijete/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dijete = await _context.Dijete.FindAsync(id);
            if (dijete != null)
                _context.Dijete.Remove(dijete);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Administrator, Vaspitac")]
        private bool DijeteExists(int id)
        {
            return _context.Dijete.Any(e => e.id == id);
        }

        public async Task<IActionResult> MojeDijete()
        {
            var userId = _userManager.GetUserId(User);

            var dijete = await _context.Dijete
                .Include(d => d.Korisnik)
                .Include(d => d.grupa)
                .FirstOrDefaultAsync(d => d.roditeljId == userId);

            if (dijete == null)
                return NotFound();

            return View("Details", dijete);
        }
        [Authorize(Roles = "Administrator, Vaspitac")]
        private void UcitajRoditeljeUViewData(string selectedRoditeljId = null)
        {
            var roditelji = _context.Korisnik
                .Where(k => k.uloga != null && (int)k.uloga == 2)
                .Select(k => new
                {
                    k.Id,
                    punoIme = (k.ime ?? "") + " " + (k.prezime ?? "")
                })
                .ToList()
                .Select(k => new SelectListItem
                {
                    Value = k.Id,
                    Text = k.punoIme,
                    Selected = (k.Id == selectedRoditeljId)
                })
                .ToList();

            if (roditelji.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("Nema roditelja s ulogom == 2");
            }

            ViewData["roditeljId"] = roditelji;
        }

    }
}
