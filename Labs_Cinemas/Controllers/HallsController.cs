using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Labs_Cinemas;

namespace Labs_Cinemas.Controllers
{
    public class HallsController : Controller
    {
        private readonly DBPostilniak_LABSContext _context;

        public HallsController(DBPostilniak_LABSContext context)
        {
            _context = context;
        }

        // GET: Halls
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Cinemas", "Index");
            ViewBag.CinemasId = id;
            ViewBag.CinemasName = name;
            var hallsByCinemas = _context.Halls.Where(b => b.CinemaId == id).Include(b => b.Cinema).Include(b => b.Controler).Include(b => b.Film);
            //var dBPostilniak_LABSContext = _context.Halls.Include(h => h.Cinema).Include(h => h.Controler).Include(h => h.Film);
            return View(await hallsByCinemas.ToListAsync());
        }

        // GET: Halls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var halls = await _context.Halls
                .Include(h => h.Cinema)
                .Include(h => h.Controler)
                .Include(h => h.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (halls == null)
            {
                return NotFound();
            }

            return View(halls);
        }

        // GET: Halls/Create
        public IActionResult Create(int cinemasId)
        {
            // ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Contact");
            ViewData["ControlerId"] = new SelectList(_context.Staffs, "Id", "Name");
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name");
            ViewBag.CinemasId = cinemasId;
            ViewBag.CinemasName = _context.Cinemas.Where(c => c.Id == cinemasId).FirstOrDefault().Name;
            return View();
        }

        // POST: Halls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int cinemasId, [Bind("Id,Name,PeopleAmount,ControlerId,CinemaId,DateTime,Price,FilmId")] Halls halls)
        {
            halls.CinemaId = cinemasId;
            if (ModelState.IsValid)
            {
                _context.Add(halls);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Halls", new { id = cinemasId, name = _context.Cinemas.Where(c => c.Id == cinemasId).FirstOrDefault().Name });
            }
            // ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Contact", halls.CinemaId);
            //ViewData["ControlerId"] = new SelectList(_context.Staffs, "Id", "Name", halls.ControlerId);
            //ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", halls.FilmId);
            // return View(halls);
            return RedirectToAction("Index", "Halls", new { id = cinemasId, name = _context.Cinemas.Where(c => c.Id == cinemasId).FirstOrDefault().Name });
        }

        // GET: Halls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var halls = await _context.Halls.FindAsync(id);
            if (halls == null)
            {
                return NotFound();
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Contact", halls.CinemaId);
            ViewData["ControlerId"] = new SelectList(_context.Staffs, "Id", "Name", halls.ControlerId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", halls.FilmId);
            return View(halls);
        }

        // POST: Halls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int cinemaId, [Bind("Id,Name,PeopleAmount,ControlerId,CinemaId,DateTime,Price,FilmId")] Halls halls)
        {
            if (cinemaId != halls.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(halls);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallsExists(halls.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new {id=cinemaId, name=_context.Cinemas.Where(c=>c.Id==cinemaId).FirstOrDefault().Name });
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Contact", halls.CinemaId);
            ViewData["ControlerId"] = new SelectList(_context.Staffs, "Id", "Name", halls.ControlerId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", halls.FilmId);
            return View(halls);
        }

        // GET: Halls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var halls = await _context.Halls
                .Include(h => h.Cinema)
                .Include(h => h.Controler)
                .Include(h => h.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (halls == null)
            {
                return NotFound();
            }

            return View(halls);
        }

        // POST: Halls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int cinemaId)
        {
            var halls = await _context.Halls.FindAsync(cinemaId);
            _context.Halls.Remove(halls);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = cinemaId, name = _context.Cinemas.Where(c => c.Id == cinemaId).FirstOrDefault().Name });
        }

        private bool HallsExists(int id)
        {
            return _context.Halls.Any(e => e.Id == id);
        }
    }
}
