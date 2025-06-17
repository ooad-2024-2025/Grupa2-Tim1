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
    public class AktivnostController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<Korisnik> _userManager;


        public AktivnostController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private void PostaviVaspitaceViewData(object selectedId = null)
        {
            // Dohvati korisnike koji imaju ulogu s id=2
            var roditelji = _context.Korisnik
                .Where(k => k.uloga != null && (int)k.uloga == 1)
                .ToList();


            ViewData["idKorisnika"] = new SelectList(roditelji, "Id", "ime", selectedId);
        }
        // GET: Aktivnost

        public IActionResult Index(int? year, int? month)
        {
            var selectedYear = year ?? DateTime.Now.Year;
            var selectedMonth = month ?? DateTime.Now.Month;

            var aktivnosti = _context.Aktivnost
            .Include(a => a.Grupa)
            .Include(a => a.Korisnik)
            .Where(a => a.datumPocetka.Year == selectedYear && a.datumPocetka.Month == selectedMonth)
            .ToList();

            var user = HttpContext.User;
            ViewBag.IsAuthorizedUser = user.IsInRole("Administrator") || user.IsInRole("Vaspitac") || user.IsInRole("Roditelj");
            ViewBag.Year = selectedYear;
            ViewBag.Month = selectedMonth;

            return View(aktivnosti);
        }


        // GET: Aktivnost/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktivnost = await _context.Aktivnost
                .Include(a => a.Grupa)
                .Include(a => a.Korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (aktivnost == null)
            {
                return NotFound();
            }

            return View(aktivnost);
        }

        // GET: Aktivnost/Create
        [Authorize(Roles = "Administrator, Vaspitac")]

        public IActionResult Create(string datumPocetka)
        {
            var users = _userManager.Users.ToList(); // Materialize to prevent issues
            ViewData["idGrupe"] = new SelectList(_context.Grupa.ToList(), "id", "naziv");
            PostaviVaspitaceViewData();
            ViewBag.TipAktivnosti = new SelectList(Enum.GetValues(typeof(TipAktivnosti)));

            var model = new Aktivnost();

            if (!string.IsNullOrEmpty(datumPocetka))
            {
                if (DateTime.TryParse(datumPocetka, out var parsedDate))
                {
                    model.datumPocetka = parsedDate;
                    model.datumZavrsetka = parsedDate;
                }
                else
                {
                    model.datumPocetka = DateTime.Today;
                    model.datumZavrsetka = DateTime.Today;
                }
            }

            return View(model);
        }





        // POST: Aktivnost/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> Create([Bind("id,nazivAktivnosti,tipAktivnosti,datumPocetka,datumZavrsetka,opis,idGrupe,idKorisnika,fotografija")] Aktivnost aktivnost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aktivnost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idGrupe"] = new SelectList(_context.Grupa.ToList(), "id", "naziv", aktivnost.idGrupe);
            ViewBag.TipAktivnosti = new SelectList(Enum.GetValues(typeof(TipAktivnosti)));
            PostaviVaspitaceViewData(aktivnost.idKorisnika);
            return View(aktivnost);
        }

        // GET: Aktivnost/Edit/5
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> Edit(int? id)
        {
            var users = _userManager.Users.ToList();
            if (id == null)
            {
                return NotFound();
            }

            var aktivnost = await _context.Aktivnost.FindAsync(id);
            if (aktivnost == null)
            {
                return NotFound();
            }
            ViewData["idGrupe"] = new SelectList(_context.Grupa.ToList(), "id", "naziv");
            PostaviVaspitaceViewData(); 
            ViewBag.TipAktivnosti = new SelectList(Enum.GetValues(typeof(TipAktivnosti)));
            return View(aktivnost);
        }

        // POST: Aktivnost/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> Edit(int id, [Bind("id,nazivAktivnosti,tipAktivnosti,datumPocetka,datumZavrsetka,opis,idGrupe,idKorisnika,fotografija")] Aktivnost aktivnost)
        {
            if (id != aktivnost.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aktivnost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AktivnostExists(aktivnost.id))
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
            ViewData["idGrupe"] = new SelectList(_context.Grupa.ToList(), "id", "naziv", aktivnost.idGrupe);
            ViewBag.TipAktivnosti = new SelectList(Enum.GetValues(typeof(TipAktivnosti)));
            PostaviVaspitaceViewData(aktivnost.idKorisnika);
            return View(aktivnost);
        }

        // GET: Aktivnost/Delete/5
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> Delete(int? id)
        {
            var users = _userManager.Users.ToList();
            if (id == null)
            {
                return NotFound();
            }

            var aktivnost = await _context.Aktivnost
                .Include(a => a.Grupa)
                .Include(a => a.Korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (aktivnost == null)
            {
                return NotFound();
            }

            ViewData["idGrupe"] = new SelectList(_context.Grupa.ToList(), "id", "naziv");
            PostaviVaspitaceViewData();
            ViewBag.TipAktivnosti = new SelectList(Enum.GetValues(typeof(TipAktivnosti)));

            return View(aktivnost);
        }

        // POST: Aktivnost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Vaspitac")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aktivnost = await _context.Aktivnost.FindAsync(id);
            if (aktivnost != null)
            {
                _context.Aktivnost.Remove(aktivnost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AktivnostExists(int id)
        {
            return _context.Aktivnost.Any(e => e.id == id);
        }
    }
}
