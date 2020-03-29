using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Labs_Cinemas
{
    public partial class DBPostilniak_LABSContext : DbContext
    {
        public DBPostilniak_LABSContext()
        {
        }

        public DBPostilniak_LABSContext(DbContextOptions<DBPostilniak_LABSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Buffets> Buffets { get; set; }
        public virtual DbSet<Cinemas> Cinemas { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Films> Films { get; set; }
        public virtual DbSet<Genres> Genres { get; set; }
        public virtual DbSet<Halls> Halls { get; set; }
        public virtual DbSet<Staffs> Staffs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LAPTOP-CV1UVUR2\\SQLEXPRESS; Database=DBPostilniak_LABS; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Buffets>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Product)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Schedule)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Buffets)
                    .HasForeignKey(d => d.CinemaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Buffets_Cinemas");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Buffets)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Buffets_Staffs");
            });

            modelBuilder.Entity<Cinemas>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Adress)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.City)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Contact)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CountOfFilms)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.YearOfIndependency)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Films>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Years)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Films)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Films_Countries");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Films)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Films_Genres");
            });

            modelBuilder.Entity<Genres>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateYear)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Names)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.WhoCreate)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Halls>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Halls)
                    .HasForeignKey(d => d.CinemaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Halls_Cinemas");

                entity.HasOne(d => d.Controler)
                    .WithMany(p => p.Halls)
                    .HasForeignKey(d => d.ControlerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Halls_Staffs");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Halls)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Halls_Films");
            });

            modelBuilder.Entity<Staffs>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Contact)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.JobPlace)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Position)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
