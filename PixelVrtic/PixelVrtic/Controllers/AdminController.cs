using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelVrtic.Data;
using PixelVrtic.Models;

namespace PixelVrtic.Controllers
{
    [Authorize]

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<Korisnik> _userManager;
        private readonly IzvjestajGeneratorService _izvjestajGeneratorService;

        public AdminController(IzvjestajGeneratorService izvjestajGeneratorService,
            ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
            _izvjestajGeneratorService = izvjestajGeneratorService;
        }

        [HttpPost]
        public async Task<IActionResult> PokreniRucno()
        {
            await _izvjestajGeneratorService.KreirajIzvjestajeZaProsliMjesec();
            TempData["Poruka"] = "Izvještaji su uspješno generisani ručno.";
            return RedirectToAction("Dashboard");
        }
        // GET: /Admin/Dashboard
        [Authorize(Roles = "Administrator")]

        public IActionResult Dashboard()
        {
            ViewBag.VaspitacCount = _context.Korisnik.Where(k => (int)k.uloga == 1).Count();
            ViewBag.RoditeljCount = _context.Korisnik.Where(k => (int)k.uloga == 2).Count();
            ViewBag.DjecaCount = _context.Dijete.Count();
            ViewBag.AktivnostiCount = _context.Aktivnost.Count();

            return View();
        }

        public IActionResult Finansije()
        {
            int mjesec = DateTime.Now.Month - 1;
            int godina = DateTime.Now.Year;
            double cijenaPoDanu = 20.0;

            if (mjesec == 0) 
            {
                mjesec = 12;
                godina -= 1;
            }

            var roditelji = _context.Users
                .Where(k => k.uloga == Uloga.roditelj)
                .ToList();

            foreach (var roditelj in roditelji)
            {
                var dijete = _context.Dijete.FirstOrDefault(d => d.roditeljId == roditelj.Id);
                if (dijete == null) continue;

                var prisustva = _context.Prisustvo
                    .Where(p => p.dijeteId == dijete.id && p.prisutan &&
                                p.datum.Month == mjesec && p.datum.Year == godina)
                    .Count();

                double izracunatiIznos = prisustva * cijenaPoDanu;

                var postojeca = _context.FinansijskaEvidencija
                    .FirstOrDefault(f => f.roditeljId == roditelj.Id && f.mjesec == mjesec && f.godina == godina);

                if (postojeca == null)
                {
                    _context.FinansijskaEvidencija.Add(new FinansijskaEvidencija
                    {
                        roditeljId = roditelj.Id,
                        mjesec = mjesec,
                        godina = godina,
                        iznos = izracunatiIznos,
                        uplaceno = false
                    });
                }
                else
                {
                    postojeca.iznos = izracunatiIznos; 
                }
            }

            _context.SaveChanges();

            var evidencije = _context.FinansijskaEvidencija
                .Include(f => f.Roditelj)
                .Where(f => f.mjesec == mjesec && f.godina == godina)
                .ToList();

            return View(evidencije);
        }

        [HttpPost]
        public IActionResult OznaciKaoPlaceno(int id)
        {
            var zapis = _context.FinansijskaEvidencija.FirstOrDefault(f => f.id == id);
            if (zapis != null)
            {
                zapis.uplaceno = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Finansije");
        }

    }
}
