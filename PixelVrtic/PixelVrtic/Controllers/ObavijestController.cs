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

    public class ObavijestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public ObavijestController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Obavijest
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Obavijest.Include(o => o.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Obavijest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obavijest = await _context.Obavijest
                .Include(o => o.Korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (obavijest == null)
            {
                return NotFound();
            }

            return View(obavijest);
        }

        // GET: Obavijest/Create
        public IActionResult Create()
        {
            ViewData["idAutora"] = new SelectList(_userManager.Users, "Id", "ime");
            return View();
        }

        // POST: Obavijest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Obavijest obavijest)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return BadRequest("Nije moguće utvrditi autora.");

            obavijest.idAutora = currentUser.Id;
            obavijest.datum = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(obavijest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obavijest);
        }




        // GET: Obavijest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obavijest = await _context.Obavijest.FindAsync(id);
            if (obavijest == null)
            {
                return NotFound();
            }
            ViewData["idAutora"] = new SelectList(_userManager.Users, "Id", "ime", obavijest.idAutora);
            return View(obavijest);
        }

        // POST: Obavijest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,naslov,tekst,datum,idAutora")] Obavijest obavijest)
        {
            if (id != obavijest.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obavijest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObavijestExists(obavijest.id))
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
            ViewData["idAutora"] = new SelectList(_userManager.Users, "Id", "ime", obavijest.idAutora);
            return View(obavijest);
        }

        // GET: Obavijest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obavijest = await _context.Obavijest
                .Include(o => o.Korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (obavijest == null)
            {
                return NotFound();
            }

            return View(obavijest);
        }

        // POST: Obavijest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obavijest = await _context.Obavijest.FindAsync(id);
            if (obavijest != null)
            {
                _context.Obavijest.Remove(obavijest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObavijestExists(int id)
        {
            return _context.Obavijest.Any(e => e.id == id);
        }

        public IActionResult Choice()
        {
            return View();
        }
    }
}
