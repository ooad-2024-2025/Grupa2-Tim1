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
        public AdminController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
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
    }
}
