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
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dijete = await _context.Dijete
                .Include(d => d.Korisnik)
                .Include(d => d.grupa)
                .FirstOrDefaultAsync(m => m.id == id);
            if (dijete == null)
            {
                return NotFound();
            }

            return View(dijete);
        }

        // GET: Dijete/Create
        public IActionResult Create()
        {

            ViewData["roditeljId"] = new SelectList(_userManager.Users, "Id", "ime");
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "naziv");
            return View();
        }

        // POST: Dijete/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ime,prezime,datumRodjenja,mjestoRodenja,JMBG,grupaId,zdravstveneNapomene,fotografija,roditeljId")] Dijete dijete)
        {
            foreach (var modelState in ModelState)
            {
                foreach (var error in modelState.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"Key: {modelState.Key}, Error: {error.ErrorMessage}");
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(dijete);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["roditeljId"] = new SelectList(_userManager.Users, "Id", "ime", dijete.roditeljId);
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "naziv", dijete.grupaId);

            return View(dijete);
        }

        // GET: Dijete/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dijete = await _context.Dijete.FindAsync(id);
            if (dijete == null)
            {
                return NotFound();
            }
            ViewData["roditeljId"] = new SelectList(_userManager.Users, "Id", "ime", dijete.roditeljId);
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "naziv", dijete.grupaId);
            return View(dijete);
        }

        // POST: Dijete/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ime,prezime,datumRodjenja,mjestoRodenja,JMBG,grupaId,zdravstveneNapomene,fotografija,roditeljId")] Dijete dijete)
        {
            if (id != dijete.id)
            {
                return NotFound();
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
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["roditeljId"] = new SelectList(_userManager.Users, "Id", "ime", dijete?.roditeljId);
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "id", dijete?.grupaId);
            return View(dijete);
        }

        // GET: Dijete/Delete/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dijete = await _context.Dijete
                .Include(d => d.Korisnik)
                .Include(d => d.grupa)
                .FirstOrDefaultAsync(m => m.id == id);
            if (dijete == null)
            {
                return NotFound();
            }

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
            {
                _context.Dijete.Remove(dijete);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DijeteExists(int id)
        {
            return _context.Dijete.Any(e => e.id == id);
        }
    }
}
