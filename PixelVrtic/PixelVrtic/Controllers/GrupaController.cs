using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PixelVrtic.Data;
using PixelVrtic.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PixelVrtic.Controllers
{
    [Authorize]

    public class GrupaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public GrupaController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Grupas
        [Authorize(Roles = "Administrator, Vaspitac")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Grupa.Include(g => g.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Grupas/Details/5
        [Authorize(Roles = "Administrator, Vaspitac")]

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
        [Authorize(Roles = "Administrator")]

        public IActionResult Create()
        {
            ViewData["idKorisnika"] = new SelectList(_userManager.Users, "Id", "ime");

            return View();
        }

        // POST: Grupas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Create(Grupa grupa)
        {
            foreach (var modelState in ModelState)
            {
                foreach (var error in modelState.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"Key: {modelState.Key}, Error: {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(grupa);
                await _context.SaveChangesAsync(); // grupa.id is now set by DB

                // Update the user's idGrupe
                if (grupa.idKorisnika != null)
                {
                    var korisnik = await _userManager.FindByIdAsync(grupa.idKorisnika.ToString());
                    if (korisnik != null)
                    {
                        korisnik.idGrupe = grupa.id; // set FK to new group's id

                        // update korisnik using UserManager
                        var result = await _userManager.UpdateAsync(korisnik);
                        if (!result.Succeeded)
                        {
                            // Handle errors here if needed
                            foreach (var error in result.Errors)
                            {
                                System.Diagnostics.Debug.WriteLine($"User update error: {error.Description}");
                            }
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["idKorisnika"] = new SelectList(_userManager.Users, "Id", "ime", grupa.idKorisnika);
            return View(grupa);
        }



        // GET: Grupas/Edit/5
        [Authorize(Roles = "Administrator")]

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
            ViewData["idKorisnika"] = new SelectList(_userManager.Users, "Id", "ime", grupa.idKorisnika);
            return View(grupa);
        }

        // POST: Grupas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int id,Grupa grupa)
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
            ViewData["idKorisnika"] = new SelectList(_userManager.Users, "Id", "ime", grupa.idKorisnika);
            return View(grupa);
        }

        // GET: Grupas/Delete/5
        [Authorize(Roles = "Administrator")]

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
        [Authorize(Roles = "Administrator")]

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
