using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelVrtic.Data;
using PixelVrtic.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PixelVrtic.Controllers
{
    [Authorize]
    public class IzvjestajController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IzvjestajController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Dohvati djecu tog roditelja
            var djeca = await _context.Dijete
                .Where(d => d.roditeljId == userId)
                .Select(d => d.id)
                .ToListAsync();

            // Dohvati izvještaje za njih
            var izvjestaji = await _context.Izvjestaj
                .Include(i => i.Dijete)
                .Where(i => djeca.Contains(i.DijeteId))
                .OrderByDescending(i => i.Period)
                .ToListAsync();

            return View(izvjestaji);
        }

        // Pregled izvještaja za vaspitača i unos komentara
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Administrator"))
            {
                // Administrator: get all reports
                var izvjestaji = await _context.Izvjestaj
                    .Include(i => i.Dijete)
                    .OrderByDescending(i => i.Period)
                    .ToListAsync();

                return View(izvjestaji);
            }
            else
            {
                // Vaspitač: get only reports for groups they manage
                var grupe = await _context.Grupa
                    .Where(g => g.idKorisnika == userId)
                    .Select(g => g.id)
                    .ToListAsync();

                var izvjestaji = await _context.Izvjestaj
                    .Include(i => i.Dijete)
                    .Where(i => grupe.Contains(i.Dijete.grupaId))
                    .OrderByDescending(i => i.Period)
                    .ToListAsync();

                return View(izvjestaji);
            }
        }


        // Akcija za spremanje komentara (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajKomentar(int id, string komentar)
        {
            if (string.IsNullOrWhiteSpace(komentar))
            {
                TempData["Greska"] = "Komentar ne može biti prazan.";
                return RedirectToAction(nameof(Index));
            }

            var izvjestaj = await _context.Izvjestaj.FindAsync(id);
            if (izvjestaj == null)
                return NotFound();

            izvjestaj.KomentarVaspitaca = komentar;
            await _context.SaveChangesAsync();

            TempData["Uspjeh"] = "Komentar je uspješno spremljen.";
            return RedirectToAction(nameof(Details));
        }
    }
}
