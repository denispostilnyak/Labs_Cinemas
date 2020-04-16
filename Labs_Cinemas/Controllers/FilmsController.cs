using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Labs_Cinemas.Models;

namespace Labs_Cinemas.Controllers
{
    public class FilmsController : Controller
    {
        private readonly IdentityContext _context;

        public FilmsController(IdentityContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index(int? id, string? name)
        {

            if (id == null) return RedirectToAction("Genres", "Index");
            ViewBag.GenresId = id;
            ViewBag.GenresName = name;
            var filmsByGenres = _context.Films.Where(b => b.GenreId == id).Include(b => b.Genre);
            //var dBPostilniak_LABSContext = _context.Films.Include(f => f.Country).Include(f => f.Genre);
            return View(await filmsByGenres.ToListAsync());
        }
       

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var films = await _context.Films
                .Include(f => f.Country)
                .Include(f => f.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (films == null)
            {
                return NotFound();
            }

            return View(films);
        }

        // GET: Films/Create
        public IActionResult Create(int genresId)
        {
            ViewBag.GenresId = genresId;
            ViewBag.GenresName = _context.Genres.Where(c => c.Id == genresId).FirstOrDefault().Names;
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
           // ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Names");
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int genresId, [Bind("Id,CountryId,Years,GenreId,Name,Duration,Age")] Films films)
        {
            // if (ModelState.IsValid)
            //{
            //  _context.Add(films);
            // await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            // }
             ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", films.CountryId);
            //ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Names", films.GenreId);
            films.GenreId = genresId;
            if (ModelState.IsValid) {
                _context.Add(films);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Films", new { id = genresId, name = _context.Genres.Where(c => c.Id == genresId).FirstOrDefault().Names });
            }
            // return View(films);
            return RedirectToAction("Index", "Films", new { id = genresId, name = _context.Genres.Where(c => c.Id == genresId).FirstOrDefault().Names });
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var films = await _context.Films.FindAsync(id);
            if (films == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", films.CountryId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Names", films.GenreId);
            return View(films);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int genresId, [Bind("Id,CountryId,Years,GenreId,Name,Duration,Age")] Films films)
        {
            if (genresId != films.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(films);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmsExists(films.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Films", new { id = genresId, name = _context.Genres.Where(c => c.Id == genresId).FirstOrDefault().Names });
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", films.CountryId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Names", films.GenreId);
            return View(films);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var films = await _context.Films
                .Include(f => f.Country)
                .Include(f => f.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (films == null)
            {
                return NotFound();
            }

            return View(films);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int genresId)
        {
            var films = await _context.Films.FindAsync(genresId);
            _context.Films.Remove(films);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Films", new { id = genresId, name = _context.Genres.Where(c => c.Id == genresId).FirstOrDefault().Names });
        }
        private bool FilmsExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }
    }
}
