using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PixelVrtic.Data;
using PixelVrtic.Models;

namespace PixelVrtic.Controllers
{
    public class DijeteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DijeteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dijete
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dijete.Include(d => d.Korisnik).Include(d => d.grupa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dijete/Details/5
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
            ViewData["roditeljId"] = new SelectList(_context.Korisnik, "id", "id");
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "id");
            return View();
        }

        // POST: Dijete/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ime,prezime,datumRodjenja,mjestoRodenja,JMBG,grupaId,zdravstveneNapomene,fotografija,roditeljId")] Dijete dijete)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dijete);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["roditeljId"] = new SelectList(_context.Korisnik, "id", "id", dijete.roditeljId);
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "id", dijete.grupaId);
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
            ViewData["roditeljId"] = new SelectList(_context.Korisnik, "id", "id", dijete.roditeljId);
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "id", dijete.grupaId);
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
            ViewData["roditeljId"] = new SelectList(_context.Korisnik, "id", "id", dijete.roditeljId);
            ViewData["grupaId"] = new SelectList(_context.Grupa, "id", "id", dijete.grupaId);
            return View(dijete);
        }

        // GET: Dijete/Delete/5
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
