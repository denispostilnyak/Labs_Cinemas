using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Labs_Cinemas.Models
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options) {
            Database.EnsureCreated();
        }
        public DbSet<Cinemas> Cinemas { get; set; }
        public DbSet<Buffets> Buffets { get; set; }

        public DbSet<Countries> Countries { get; set; }
        public DbSet<Films> Films { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Halls> Halls { get; set; }
        public DbSet<Staffs> Staffs { get; set; }
        public new DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cinemas>().ToTable("Cinemas");
            modelBuilder.Entity<Buffets>().ToTable("Buffets");
            modelBuilder.Entity<Countries>().ToTable("Countries");
            modelBuilder.Entity<Films>().ToTable("Films");
            modelBuilder.Entity<Genres>().ToTable("Genres");
            modelBuilder.Entity<Halls>().ToTable("Halls");
            modelBuilder.Entity<Staffs>().ToTable("Staffs");
            //modelBuilder.Entity<User>().ToTable("User");
        }
        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
    }

}
