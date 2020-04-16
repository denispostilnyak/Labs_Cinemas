using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using Labs_Cinemas.Models;
using Microsoft.AspNetCore.Authorization;


namespace Labs_Cinemas.Controllers
{
    [Authorize(Roles="admin, user")]
    public class CinemasController : Controller
    {
        private readonly IdentityContext _context;

        public CinemasController(IdentityContext context) {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel) {
            if (ModelState.IsValid) {
                if (fileExcel != null) {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create)) {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled)) {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets) {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Cinemas newcat;
                                var c = (from cin in _context.Cinemas
                                         where cin.Name.Contains(worksheet.Name)
                                         select cin).ToList();
                                if (c.Count > 0) {
                                    newcat = c[0];
                                } else {
                                    newcat = new Cinemas();
                                    Random random = new Random();
                                    int id = random.Next(10000);
                                    newcat.Name = worksheet.Name;
                                    newcat.Id = id;
                                    newcat.Contact = "Default";
                                    newcat.Adress = "Default";
                                    newcat.City = "Kyiv";
                                    //додати в контекст
                                    _context.Cinemas.Add(newcat);

                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1)) {
                                    try {
                                        Halls hall = new Halls();
                                        Random random = new Random();
                                        int id = random.Next(10000);
                                        hall.Id = id;
                                        hall.Name = row.Cell(1).Value.ToString();
                                        hall.Cinema = newcat;
                                        DateTime date;
                                        if (DateTime.TryParse(row.Cell(2).Value.ToString(), out date) && date >= DateTime.Now) {
                                            hall.DateTime = date;
                                        } else {
                                            _context.Cinemas.Remove(newcat);
                                            throw new Exception("InCorrect date");
                                        }
                                        string templateForPrice = @"^[0-9]*$";
                                        var dateFromExcel = row.Cell(3).Value.ToString();
                                        if (Regex.IsMatch(dateFromExcel, templateForPrice, RegexOptions.IgnoreCase)) {
                                            hall.Price = dateFromExcel;
                                        } else {
                                            _context.Cinemas.Remove(newcat);
                                            throw new Exception("InCorrect price");
                                        }
                                        var capacityFromExcel = row.Cell(4).Value.ToString();
                                        if (Regex.IsMatch(dateFromExcel, templateForPrice, RegexOptions.IgnoreCase)) {
                                            hall.PeopleAmount = Convert.ToInt32(capacityFromExcel);
                                        } else {
                                            _context.Cinemas.Remove(newcat);
                                            throw new Exception("InCorrect capacity");
                                        }
                                        Staffs controller = new Staffs();
                                        var a = (from aut in _context.Staffs
                                                 where aut.Name.Contains(row.Cell(5).Value.ToString())
                                                 select aut).ToList();
                                        if (a.Count > 0) {
                                            hall.Controler = a[0];
                                        } else {
                                            controller.Name = row.Cell(5).Value.ToString();
                                            controller.Age = 18;
                                            controller.Contact = "Default";
                                            controller.Id = random.Next(10000);
                                            controller.JobPlace = "Cashier";
                                            controller.Position = "Junior";
                                            //додати в контекст
                                            _context.Staffs.Add(controller);
                                            hall.Controler = controller;
                                        }
                                        Films film = new Films();
                                        var b = (from aut in _context.Films
                                                 where aut.Name.Contains(row.Cell(6).Value.ToString())
                                                 select aut).ToList();
                                        if (b.Count > 0) {
                                            hall.Film= b[0];
                                        } else {
                                            film.Id = random.Next(10000);
                                            film.Name = row.Cell(6).Value.ToString();
                                            film.Years = "Default";
                                            film.Genre = _context.Genres.First();
                                            film.Duration = 180;
                                            film.Age = 18;
                                            film.Country = _context.Countries.First();
                                            //додати в контекст
                                            _context.Films.Add(film);
                                            hall.Film = film;
                                        }
                                        _context.Halls.Add(hall);
                                    } catch (Exception e) {
                                        ViewBag.ErrorMessage = e.Message;
                                        return View(nameof(Index),await _context.Cinemas.ToListAsync());
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
                var cinemas = _context.Cinemas.Include("Halls").Include("Buffets").ToList();
                foreach (var c in cinemas) {
                    var workSheet = workbook.Worksheets.Add(c.Name);
                    workSheet.Cell("A1").Value = "Зали";
                    workSheet.Cell("B1").Value = "Назва";
                    workSheet.Cell("C1").Value = "Касир";
                    workSheet.Cell("D1").Value = "ФІльм";
                    workSheet.Row(1).Style.Font.Bold = true;
                    var halls = c.Halls.ToList();
                    for (int i = 0; i < halls.Count; ++i) {
                        var controllerName = _context.Staffs.Where(c => c.Id == halls[i].ControlerId).Select(c => c.Name);
                        var filmName = _context.Films.Where(c => c.Id == halls[i].FilmId).Select(c => c.Name);
                        workSheet.Cell(i + 2, 2).Value = halls[i].Name;
                        workSheet.Cell(i + 2, 3).Value = controllerName;
                        workSheet.Cell(i + 2, 4).Value = filmName;
                    }
                    workSheet.Cell("F1").Value = "Буфети";
                    workSheet.Cell("G1").Value = "Товар";
                    workSheet.Cell("H1").Value = "Графік";
                    workSheet.Cell("I1").Value = "Касир";
                    var buffets = c.Buffets.ToList();
                    for (int i = 0; i < buffets.Count; ++i) {
                        var controllerName = _context.Staffs.Where(c => c.Id == buffets[i].StaffId).Select(c => c.Name);
                        workSheet.Cell(i + 2, 7).Value = buffets[i].Product;
                        workSheet.Cell(i + 2, 8).Value = buffets[i].Schedule;
                        workSheet.Cell(i + 2, 9).Value = controllerName;
                    }
                }
                using (var stream = new MemoryStream()) {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                        FileDownloadName = $"cinema_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
        // GET: Cinemas
        public async Task<IActionResult> Index(string searchString) {
            var movies = from m in _context.Cinemas
                         select m;
            if (!String.IsNullOrEmpty(searchString)) {
                movies = movies.Where(s => s.Name.Contains(searchString));
            }
            return View(await movies.ToListAsync());
        }

        // GET: Cinemas/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var cinemas = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinemas == null) {
                return NotFound();
            }
            return RedirectToAction("Index", "Halls", new { id = cinemas.Id, name = cinemas.Name });
            // return View(cinemas);
        }

        public async Task<IActionResult> Buffets(int? id) {
            if (id == null) {
                return NotFound();
            }

            var cinemas = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinemas == null) {
                return NotFound();
            }
            return RedirectToAction("Index", "Buffets", new { id = cinemas.Id, name = cinemas.Name });
            // return View(cinemas);
        }

        // GET: Cinemas/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Cinemas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,City,Adress,Contact")] Cinemas cinemas) {
            if (ModelState.IsValid) {
                _context.Add(cinemas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinemas);
        }

        // GET: Cinemas/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var cinemas = await _context.Cinemas.FindAsync(id);
            if (cinemas == null) {
                return NotFound();
            }
            return View(cinemas);
        }

        // POST: Cinemas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City,Adress,Contact")] Cinemas cinemas) {
            if (id != cinemas.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(cinemas);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!CinemasExists(cinemas.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cinemas);
        }

        // GET: Cinemas/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var cinemas = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinemas == null) {
                return NotFound();
            }

            return View(cinemas);
        }

        // POST: Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var cinemas = await _context.Cinemas.FindAsync(id);
            DeleteHalls(id);
            _context.Cinemas.Remove(cinemas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async void DeleteHalls(int id) {
            var cinemas = await _context.Cinemas.FindAsync(id);
            foreach (var i in _context.Halls) {
                if (cinemas.Id == i.CinemaId) _context.Halls.Remove(i);
            }

        }

        private bool CinemasExists(int id) {
            return _context.Cinemas.Any(e => e.Id == id);
        }
    }
}
