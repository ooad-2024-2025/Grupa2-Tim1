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
    public class GrupaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GrupaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Grupas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Grupa.Include(g => g.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Grupas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupa = await _context.Grupa
                .Include(g => g.Korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (grupa == null)
            {
                return NotFound();
            }

            return View(grupa);
        }

        // GET: Grupas/Create
        public IActionResult Create()
        {
            ViewData["idKorisnika"] = new SelectList(_context.Korisnik, "id", "id");
            return View();
        }

        // POST: Grupas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,naziv,idKorisnika")] Grupa grupa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grupa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idKorisnika"] = new SelectList(_context.Korisnik, "id", "id", grupa.idKorisnika);
            return View(grupa);
        }

        // GET: Grupas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupa = await _context.Grupa.FindAsync(id);
            if (grupa == null)
            {
                return NotFound();
            }
            ViewData["idKorisnika"] = new SelectList(_context.Korisnik, "id", "id", grupa.idKorisnika);
            return View(grupa);
        }

        // POST: Grupas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,naziv,idKorisnika")] Grupa grupa)
        {
            if (id != grupa.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grupa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupaExists(grupa.id))
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
            ViewData["idKorisnika"] = new SelectList(_context.Korisnik, "id", "id", grupa.idKorisnika);
            return View(grupa);
        }

        // GET: Grupas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupa = await _context.Grupa
                .Include(g => g.Korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (grupa == null)
            {
                return NotFound();
            }

            return View(grupa);
        }

        // POST: Grupas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupa = await _context.Grupa.FindAsync(id);
            if (grupa != null)
            {
                _context.Grupa.Remove(grupa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GrupaExists(int id)
        {
            return _context.Grupa.Any(e => e.id == id);
        }
    }
}
