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

        public IActionResult Index()
        {
            var roditelji = _userManager.Users.Where(u => u.uloga == Uloga.roditelj).ToList();
            return View(roditelji);
        }

        public IActionResult Create()
        {
            return View(new Korisnik());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public IActionResult Edit(string id, Korisnik korisnik)
        {
            if (id != korisnik.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(korisnik);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(korisnik);
        }


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
