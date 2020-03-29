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
    public class BuffetsController : Controller
    {
        private readonly DBPostilniak_LABSContext _context;

        public BuffetsController(DBPostilniak_LABSContext context)
        {
            _context = context;
        }

        // GET: Buffets
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Cinemas", "Index");
            ViewBag.CinemasId = id;
            ViewBag.CinemasName = name;
            var buffetsByCinema = _context.Buffets.Where(b => b.CinemaId == id).Include(b => b.Cinema).Include(b => b.Staff);
           // var dBPostilniak_LABSContext = _context.Buffets.Include(b => b.Cinema).Include(b => b.Staff);
            return View(await buffetsByCinema.ToListAsync());
        }

        // GET: Buffets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buffets = await _context.Buffets
                .Include(b => b.Cinema)
                .Include(b => b.Staff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buffets == null)
            {
                return NotFound();
            }

            return View(buffets);
        }

        // GET: Buffets/Create
        public IActionResult Create(int cinemasId)
        {
            ViewBag.CinemasId = cinemasId;
            ViewBag.CinemasName = _context.Cinemas.Where(c => c.Id == cinemasId).FirstOrDefault().Name;
            //ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Contact");
            ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Name");
            return View();
        }

        // POST: Buffets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int cinemasId, [Bind("Id,Product,StaffId,Schedule,CinemaId")] Buffets buffets)
        {
            //if (ModelState.IsValid)
            //{
            //  _context.Add(buffets);
            // await _context.SaveChangesAsync();
            // return RedirectToAction(nameof(Index));
            //}
            //ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Contact", buffets.CinemaId);
            //ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Name", buffets.StaffId);
            buffets.CinemaId = cinemasId;
            if (ModelState.IsValid) {
                _context.Add(buffets);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Buffets", new { id = cinemasId, name = _context.Cinemas.Where(c => c.Id == cinemasId).FirstOrDefault().Name });
            }
            // return View(buffets);
            return RedirectToAction("Index", "Buffets", new { id = cinemasId, name = _context.Cinemas.Where(c => c.Id == cinemasId).FirstOrDefault().Name });
        }

        // GET: Buffets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buffets = await _context.Buffets.FindAsync(id);
            if (buffets == null)
            {
                return NotFound();
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Contact", buffets.CinemaId);
            ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Name", buffets.StaffId);
            return View(buffets);
        }

        // POST: Buffets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int cinemasId, [Bind("Id,Product,StaffId,Schedule,CinemaId")] Buffets buffets)
        {
            if (cinemasId != buffets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buffets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuffetsExists(buffets.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Buffets", new { id = cinemasId, name = _context.Cinemas.Where(c => c.Id == cinemasId).FirstOrDefault().Name });
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Contact", buffets.CinemaId);
            ViewData["StaffId"] = new SelectList(_context.Staffs, "Id", "Name", buffets.StaffId);
            return View(buffets);
        }

        // GET: Buffets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buffets = await _context.Buffets
                .Include(b => b.Cinema)
                .Include(b => b.Staff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buffets == null)
            {
                return NotFound();
            }

            return View(buffets);
        }

        // POST: Buffets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int cinemasId)
        {
            var buffets = await _context.Buffets.FindAsync(cinemasId);
            _context.Buffets.Remove(buffets);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Buffets", new { id = cinemasId, name = _context.Cinemas.Where(c => c.Id == cinemasId).FirstOrDefault().Name });
        }

        private bool BuffetsExists(int id)
        {
            return _context.Buffets.Any(e => e.Id == id);
        }
    }
}
