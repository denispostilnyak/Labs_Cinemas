using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Labs_Cinemas.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace Labs_Cinemas.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class GenresController : Controller
    {
        private readonly IdentityContext _context;

        public GenresController(IdentityContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Genres.ToListAsync());
        }
        public async Task<IActionResult> Import(IFormFile fileExcel) {
            if (ModelState.IsValid) {
                if (fileExcel != null) {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create)) {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled)) {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets) {
                                Genres newcat;
                                var c = (from cin in _context.Genres
                                         where cin.Names.Contains(worksheet.Name)
                                         select cin).ToList();
                                if (c.Count > 0) {
                                    newcat = c[0];
                                } else {
                                    newcat = new Genres();
                                    Random random = new Random();
                                    int id = random.Next(10000);
                                    newcat.Names = worksheet.Name;
                                    newcat.Id = id;
                                    newcat.WhoCreate = "Default";
                                    newcat.CreateYear = "Default";
                                   
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1)) {
                                    try {
                                        Films film1 = new Films();
                                        Random random = new Random();
                                        int id = random.Next(10000);
                                        film1.Id = id;
                                        film1.Name = row.Cell(2).Value.ToString();
                                        int dateForFilm;
                                        if (int.TryParse(row.Cell(1).Value.ToString(), out dateForFilm) && dateForFilm <= DateTime.Now.Year) {
                                            film1.Years = dateForFilm.ToString();
                                        } else {
                                                throw new Exception("InCorrect date");
                                        }
                                        string templateForPrice = @"^[0-9]*$";
                                        var durationFromExcel = row.Cell(3).Value.ToString();
                                        if (Regex.IsMatch(durationFromExcel, templateForPrice, RegexOptions.IgnoreCase)) {
                                            film1.Duration = Convert.ToInt32(durationFromExcel);
                                        } else {
                                            throw new Exception("InCorrect duration");
                                        }
                                        var ageFromExcel = row.Cell(4).Value.ToString();
                                        if (Regex.IsMatch(ageFromExcel, templateForPrice, RegexOptions.IgnoreCase) && Convert.ToInt32(ageFromExcel) > 0 && Convert.ToInt32(ageFromExcel) < 100)  {
                                            film1.Age = Convert.ToInt32(ageFromExcel);
                                        } else {
                                            throw new Exception("InCorrect age");
                                        }
                                        if (c.Count() == 0) {
                                            _context.Genres.Add(newcat);
                                        }
                                        film1.Genre = newcat;
                                        Countries country = new Countries();
                                        var a = (from aut in _context.Countries
                                                 where aut.Name.Contains(row.Cell(5).Value.ToString())
                                                 select aut).ToList();
                                        if (a.Count > 0) {
                                            film1.Country = a[0];
                                        } else {
                                            country.Name = row.Cell(5).Value.ToString();
                                            country.CountOfFilms = "Default";
                                            country.YearOfIndependency= "Default";
                                            country.Id = random.Next(10000);
                                            //додати в контекст
                                            _context.Countries.Add(country);
                                            film1.Country = country;
                                        }
                                        _context.Films.Add(film1);
                                    } catch (Exception e) {
                                        ViewBag.ErrorMessage = e.Message;
                                        return View(nameof(Index), await _context.Genres.ToListAsync());
                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export() {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled)) {
                var genres = _context.Genres.Include("Films").ToList();
                foreach (var c in genres) {
                    var workSheet = workbook.Worksheets.Add(c.Names);
                    workSheet.Cell("A1").Value = "Рік випуску";
                    workSheet.Cell("B1").Value = "Назва";
                    workSheet.Cell("C1").Value = "Тривалість";
                    workSheet.Cell("D1").Value = "Країна";
                    workSheet.Row(1).Style.Font.Bold = true;
                    var films = c.Films.ToList();
                    for (int i = 0; i < films.Count; ++i) {
                        var countryName = _context.Countries.Where(c => c.Id == films[i].CountryId).Select(c => c.Name);
                        workSheet.Cell(i + 2, 1).Value = films[i].Years;
                        workSheet.Cell(i + 2, 2).Value = films[i].Name;
                        workSheet.Cell(i + 2, 3).Value = films[i].Duration;
                        workSheet.Cell(i + 2, 4).Value = countryName;
                    }
                }
                using (var stream = new MemoryStream()) {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                        FileDownloadName = $"genre_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genres = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genres == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Films", new { id = genres.Id, name = genres.Names });
            //return View(genres);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Names,CreateYear,WhoCreate")] Genres genres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genres);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genres);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genres = await _context.Genres.FindAsync(id);
            if (genres == null)
            {
                return NotFound();
            }
            return View(genres);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Names,CreateYear,WhoCreate")] Genres genres)
        {
            if (id != genres.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genres);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenresExists(genres.Id))
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
            return View(genres);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genres = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genres == null)
            {
                return NotFound();
            }

            return View(genres);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genres = await _context.Genres.FindAsync(id);
            DeleteFilms(id);
            _context.Genres.Remove(genres);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async void DeleteFilms(int id) {
            var genre = await _context.Genres.FindAsync(id);
            foreach (var i in _context.Films) {
                if (genre.Id == i.GenreId) _context.Films.Remove(i);
            }

        }

        private bool GenresExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}
