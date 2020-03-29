using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labs_Cinemas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Default1Controller : ControllerBase
    {
        private readonly DBPostilniak_LABSContext _context;
        public Default1Controller(DBPostilniak_LABSContext context) {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData() {
            var cinemas = _context.Genres.Include(b => b.Films).ToList();
            List<object> cinHalls = new List<object>();
            cinHalls.Add(new[] { "Жанр", "Кількість фільмів" });
            foreach (var c in cinemas) {
                cinHalls.Add(new object[] { c.Names, c.Films.Count() });
            }
            return new JsonResult(cinHalls);
        }
    }
}