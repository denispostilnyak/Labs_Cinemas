using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labs_Cinemas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labs_Cinemas.Controllers
{
    public class RequestsController : Controller {
        private readonly IdentityContext _context;

        public RequestsController(IdentityContext context) {
            _context = context;
        }
        // GET: Requests
        public async Task<IActionResult> Index(string searchString) {
            var requests = from m in _context.Requests
                           select m;
            if (!String.IsNullOrEmpty(searchString)) {
                requests = requests.Where(s => s.Name.Contains(searchString));
            }
            return View(await requests.ToListAsync());
        }
        // GET: Requests/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Info")] Requests requests) {
            if (ModelState.IsValid) {
                _context.Add(requests);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requests);
        }
        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var requests = await _context.Requests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requests == null) {
                return NotFound();
            }

            return View(requests);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var requests = await _context.Requests.FindAsync(id);
            _context.Requests.Remove(requests);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Get: Requests/Settings/1
        public IActionResult Settings(int id) {
            switch (id) {
                case 1:
                    return RedirectToAction(nameof(FirstAction));
                case 2:
                    return RedirectToAction(nameof(SecondAction));
                case 3:
                    return RedirectToAction(nameof(ThirdAction));
                case 4:
                    return RedirectToAction(nameof(FourthAction));
                case 5:
                    return RedirectToAction(nameof(FifthAction));
                case 6:
                    return RedirectToAction(nameof(SixthAction));
                case 7:
                    return RedirectToAction(nameof(SeventhAction));
                case 8:
                    return RedirectToAction(nameof(EigthAction));
            }
            return Ok();
        }

        //Get: Requests/FirstAction/1
        public async Task<IActionResult> FirstAction() {
            var films = _context.Films.Where(f => f.Name == "");
            return View( await films.ToListAsync()); ;
        }

        //Get: Requests/SecondAction/1
        public async Task<IActionResult> SecondAction() {
            var films = _context.Films.Where(f => f.Name == "");
            return View(await films.ToListAsync()); ;
        }

        //Get: Requests/ThirdAction/1
        public async Task<IActionResult> ThirdAction() {
            var buffets = _context.Buffets.Where(f => f.Product == "");
            return View(await buffets.ToListAsync()); ;
        }

        //Get: Requests/FourthAction/1
        public async Task<IActionResult> FourthAction() {
            var buffets = _context.Buffets.Where(f => f.Product == "");
            return View(await buffets.ToListAsync()); ;
        }

        //Get: Requests/FifthAction/1
        public async Task<IActionResult> FifthAction() {
            var hall = _context.Halls.Where(f => f.Name == "");
            return View(await hall.ToListAsync()); ;
        }

        //Get: Requests/SixthAction/1
        public async Task<IActionResult> SixthAction() {
            var hall = _context.Halls.Where(f => f.Name == "");
            return View(await hall.ToListAsync()); ;
        }

        //Get: Requests/SeventhAction/1
        public async Task<IActionResult> SeventhAction() {
            var hall = _context.Halls.Where(f => f.Name == "");
            return View(await hall.ToListAsync()); ;
        }

        //Get: Requests/EigthAction/1
        public async Task<IActionResult> EigthAction() {
            var film = _context.Films.Where(f => f.Name == "");
            return View(await film.ToListAsync()); ;
        }

        //Post: Requests/FirstAction/1
        [HttpPost]
        public async Task<IActionResult> FirstAction(string county) {
            if (!String.IsNullOrEmpty(county)) {
                int countryId = _context.Countries.Where(c => c.Name.Contains(county)).Select(c => c.Id).FirstOrDefault();
                var films = _context.Films.Where(f => f.CountryId == countryId);
                return View(await films.ToListAsync());
            }
            return Ok();
        }

        //Post: Requests/SecondAction/1
        [HttpPost]
        public async Task<IActionResult> SecondAction(string genre) {
            if (!String.IsNullOrEmpty(genre)) {
                int genreId = _context.Genres.Where(c => c.Names.Contains(genre)).Select(c => c.Id).FirstOrDefault();
                var films = _context.Films.Where(f => f.GenreId == genreId);
                return View(await films.ToListAsync());
            }
            return Ok();
        }
        //Post: Requests/ThirdAction/1
        [HttpPost]
        public async Task<IActionResult> ThirdAction(string cinema) {
            if (!String.IsNullOrEmpty(cinema)) {
                int cinemaId = _context.Cinemas.Where(c => c.Name.Contains(cinema)).Select(c => c.Id).FirstOrDefault();
                var buffet = _context.Buffets.Where(f => f.CinemaId == cinemaId);
                return View(await buffet.ToListAsync());
            }
            return Ok();
        }

        //Post: Requests/ThirdAction/1
        [HttpPost]
        public async Task<IActionResult> FourthAction(string staff) {
            if (!String.IsNullOrEmpty(staff)) {
                int staffId = _context.Staffs.Where(c => c.Name.Contains(staff)).Select(c => c.Id).FirstOrDefault();
                var buffet = _context.Buffets.Where(f => f.StaffId == staffId);
                return View(await buffet.ToListAsync());
            }
            return Ok();
        }

        //Post: Requests/FifthAction/1
        [HttpPost]
        public async Task<IActionResult> FifthAction(string cinema) {
            if (!String.IsNullOrEmpty(cinema)) {
                int cinemaId = _context.Cinemas.Where(c => c.Name.Contains(cinema)).Select(c => c.Id).FirstOrDefault();
                var hall = _context.Halls.Where(f => f.CinemaId == cinemaId);
                return View(await hall.ToListAsync());
            }
            return Ok();
        }

        //Post: Requests/SixthAction/1
        [HttpPost]
        public async Task<IActionResult> SixthAction(string hall, int price) {
            if (!String.IsNullOrEmpty(hall)) {
                int filmId = _context.Halls.Where(c => c.Name.Contains(hall)).Select(c => c.FilmId).FirstOrDefault();
                var prices = _context.Halls.Where(h => h.FilmId == filmId && h.Name!=hall).Select(h=>h.Price).FirstOrDefault();
                if (Int32.Parse(prices) <= price) {
                    return View( await _context.Halls.Where(h => h.FilmId == filmId && h.Name != hall).ToListAsync());
                } else return View(await _context.Halls.Where(h => h.FilmId == 100).ToListAsync());
            }
            return Ok();
        }

        //Post: Requests/SeventhAction/1
        [HttpPost]
        public async Task<IActionResult> SeventhAction(string staff) {
            if (!String.IsNullOrEmpty(staff)) {
                var staffId = _context.Staffs.Where(s => s.Name.Contains(staff)).Select(s=>s.Id).FirstOrDefault();
                var halls = _context.Halls.Where(h => h.ControlerId == staffId);
                List<Halls> listHalls =  await halls.ToListAsync(); ;
                foreach (var currHall in _context.Halls) {
                    foreach (var hall in halls) {
                        if(hall.Name == currHall.Name && currHall.ControlerId != staffId) {
                            listHalls.Remove(hall);
                        }
                    }
                }
                return View(listHalls);
            }
            return Ok();
        }
        //Post: Requests/SeventhAction/1
        [HttpPost]
        public async Task<IActionResult> EigthAction(int min, int max) {
            List<Films> resultList = new List<Films>();
            var result = (from film in _context.Films.ToList()
                       join genre in _context.Genres.ToList() on film.GenreId equals genre.Id
                       group _context.Films.ToList() by film.Name into g
                       where g.Count() > min && g.Count() < max
                       select g.Key );
            foreach(var res in result.ToList()) {
                var a = _context.Films.Where(c => c.Name == res).ToList().FirstOrDefault();
                resultList.Add(a);
            }
            return View(resultList);
        }
    }
}
