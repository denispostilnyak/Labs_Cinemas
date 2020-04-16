using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Labs_Cinemas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Labs_Cinemas.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<User>
    {
       
        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
    }
}
