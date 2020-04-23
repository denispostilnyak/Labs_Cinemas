using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Labs_Cinemas.Models;
using Microsoft.EntityFrameworkCore;

namespace Labs_Cinemas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly IdentityContext _context;
        public DefaultController(IdentityContext context) {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData() {
            var cinemas = _context.Cinemas.Include(b => b.Halls).ToList();
            List<object> cinHalls = new List<object>();
            cinHalls.Add(new[] { "Кінотеатр", "Кількість зал" });
            foreach(var c in cinemas) {
                cinHalls.Add(new object[] { c.Name, c.Halls.Count() });
            }
            return new JsonResult(cinHalls);
        }
    }
}