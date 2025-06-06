using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PixelVrtic.Data;
using PixelVrtic.Models;

namespace PixelVrtic.Controllers
{
    [Authorize]
    public class PrisustvoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrisustvoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prisustvo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Prisustvo.Include(p => p.Dijete);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Prisustvo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prisustvo = await _context.Prisustvo
                .Include(p => p.Dijete)
                .FirstOrDefaultAsync(m => m.id == id);
            if (prisustvo == null)
            {
                return NotFound();
            }

            return View(prisustvo);
        }

        // GET: Prisustvo/Create
        public IActionResult Create()
        {
            ViewData["dijeteId"] = new SelectList(_context.Dijete, "id", "id");
            return View();
        }

        // POST: Prisustvo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,datum,dijeteId,prisutan,razlogOdsutnosti")] Prisustvo prisustvo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prisustvo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["dijeteId"] = new SelectList(_context.Dijete, "id", "id", prisustvo.dijeteId);
            return View(prisustvo);
        }

        // GET: Prisustvo/Edit/5
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prisustvo = await _context.Prisustvo.FindAsync(id);
            if (prisustvo == null)
            {
                return NotFound();
            }
            ViewData["dijeteId"] = new SelectList(_context.Dijete, "id", "id", prisustvo.dijeteId);
            return View(prisustvo);
        }

        // POST: Prisustvo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> Edit(int id, [Bind("id,datum,dijeteId,prisutan,razlogOdsutnosti")] Prisustvo prisustvo)
        {
            if (id != prisustvo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prisustvo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrisustvoExists(prisustvo.id))
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
            ViewData["dijeteId"] = new SelectList(_context.Dijete, "id", "id", prisustvo.dijeteId);
            return View(prisustvo);
        }

        // GET: Prisustvo/Delete/5
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prisustvo = await _context.Prisustvo
                .Include(p => p.Dijete)
                .FirstOrDefaultAsync(m => m.id == id);
            if (prisustvo == null)
            {
                return NotFound();
            }

            return View(prisustvo);
        }

        // POST: Prisustvo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prisustvo = await _context.Prisustvo.FindAsync(id);
            if (prisustvo != null)
            {
                _context.Prisustvo.Remove(prisustvo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrisustvoExists(int id)
        {
            return _context.Prisustvo.Any(e => e.id == id);
        }

        [Authorize(Roles = "Administrator, Vaspitac")]
        public IActionResult QR()
        {
            return View();
        }
    }
}
