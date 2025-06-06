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

        [HttpPost]
        public async Task<IActionResult> CreateFromQr()
        {
            using var reader = new StreamReader(Request.Body);
            var idRoditelja = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(idRoditelja))
            {
                return Content("Greška: Prazan QR kod.", "text/plain");
            }

            var dijete = await _context.Dijete
                .FirstOrDefaultAsync(d => d.roditeljId == idRoditelja);

            if (dijete == null)
            {
                return Content("Greška: Nema djeteta s tim roditelj ID-om.", "text/plain");
            }

            var danas = DateTime.Today;
            var sutra = danas.AddDays(1);

            // Provjera postoji li već prisustvo za ovo dijete danas
            bool postojiPrisustvo = await _context.Prisustvo
                .AnyAsync(p => p.dijeteId == dijete.id && p.datum >= danas && p.datum < sutra);

            if (postojiPrisustvo)
            {
                return Content($"Već je zabilježeno prisustvo za {dijete.ime} {dijete.prezime} .", "text/plain");
            }

            var prisustvo = new Prisustvo
            {
                dijeteId = dijete.id,
                datum = DateTime.Now,
                prisutan = true,
                razlogOdsutnosti = null
            };

            _context.Prisustvo.Add(prisustvo);
            await _context.SaveChangesAsync();

            return Content($"Prisustvo zabilježeno za {dijete.ime} {dijete.prezime} u {DateTime.Now:HH:mm:ss}", "text/plain");
        }


        [HttpGet]
        public async Task<IActionResult> GetDanasnjaPrisustva()
        {
            var danas = DateTime.Today;
            var sutra = danas.AddDays(1);

            var prisustva = await (
                from p in _context.Prisustvo
                join d in _context.Dijete on p.dijeteId equals d.id
                where p.datum >= danas && p.datum < sutra
                orderby p.datum descending
                select new
                {
                    Ime = d.ime,
                    Prezime = d.prezime,
                    Vrijeme = p.datum.ToString("HH:mm:ss")
                }
            ).ToListAsync();

            return new JsonResult(prisustva);
        }



    }
}
