using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelVrtic.Data;
using PixelVrtic.Models;

namespace PixelVrtic.Controllers
{
    [Authorize]
    public class RoditeljController : Controller
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ApplicationDbContext _context;

        public RoditeljController(ApplicationDbContext context, UserManager<Korisnik> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Administrator, Vaspitac")]
        public IActionResult Index()
        {
            var roditelji = _userManager.Users
                .Where(u => u.uloga == Uloga.roditelj)
                .ToList();

            var djeca = _context.Dijete
                .Where(d => roditelji.Select(r => r.Id).Contains(d.roditeljId))
                .ToList();

            ViewBag.Djeca = djeca;

            return View(roditelji);
        }

        [Authorize(Roles = "Administrator")]

        public IActionResult Create()
        {
            return View(new Korisnik());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Create(Korisnik korisnik, string password)
        {
            if (!ModelState.IsValid)
                return View(korisnik);

            korisnik.UserName = korisnik.Email;
            korisnik.uloga = Uloga.roditelj;

            var result = await _userManager.CreateAsync(korisnik, password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Roditelj"))
                    await _roleManager.CreateAsync(new IdentityRole("Roditelj"));

                await _userManager.AddToRoleAsync(korisnik, "Roditelj");

                return RedirectToAction(nameof(Index));
            }

            foreach (var err in result.Errors)
                ModelState.AddModelError("", err.Description);

            return View(korisnik);
        }

        // GET: Roditelji/Edit/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(string id)
        {
            var roditelj = await _userManager.FindByIdAsync(id);
            if (roditelj == null || roditelj.uloga != Uloga.roditelj)
            {
                return NotFound();
            }
            return View(roditelj);
        }


        // POST: Roditelji/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(string id, Korisnik korisnik)
        {
            if (id != korisnik.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(korisnik);

            var postojeci = await _userManager.FindByIdAsync(id);
            if (postojeci == null || postojeci.uloga != Uloga.roditelj)
                return NotFound();

            // Ažuriranje dozvoljenih polja
            postojeci.ime = korisnik.ime;
            postojeci.prezime = korisnik.prezime;
            postojeci.datumRodjenja = korisnik.datumRodjenja;
            postojeci.grad = korisnik.grad;
            postojeci.brojTelefona = korisnik.brojTelefona;
            postojeci.Email = korisnik.Email;
            postojeci.UserName = korisnik.Email; // Sync userName ako koristiš email kao username

            try
            {
                await _userManager.UpdateAsync(postojeci);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Dogodila se greška pri ažuriranju. Molimo pokušajte ponovo.");
                return View(korisnik);
            }
        }


        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null || korisnik.uloga != Uloga.roditelj)
                return NotFound();

            return View(korisnik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null || korisnik.uloga != Uloga.roditelj)
                return NotFound();

            var result = await _userManager.DeleteAsync(korisnik);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error deleting user");
                return View(korisnik);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
