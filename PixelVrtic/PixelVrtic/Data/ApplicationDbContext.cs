using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PixelVrtic.Models;

namespace PixelVrtic.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Grupa> Grupa { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Obavijest> Obavijest { get; set; }
        public DbSet<Prisustvo> Prisustvo { get; set; }
        public DbSet<Dijete> Dijete { get; set; }
        public DbSet<Aktivnost> Aktivnost { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Korisnik>().ToTable("Korisnik");
            builder.Entity<Grupa>().ToTable("Grupa");
            builder.Entity<Obavijest>().ToTable("Obavijest");
            builder.Entity<Prisustvo>().ToTable("Prisustvo");
            builder.Entity<Dijete>().ToTable("Dijete");
            builder.Entity<Aktivnost>().ToTable("Aktivnost");
            base.OnModelCreating(builder);
            builder.Entity<Aktivnost>()
                .HasOne(a => a.Korisnik)
                .WithMany() 
                .HasForeignKey(a => a.idKorisnika)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Aktivnost>()
                .HasOne(a => a.Grupa)
                .WithMany()
                .HasForeignKey(a => a.idGrupe)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<Dijete>()
                .HasOne(d => d.Korisnik)
                .WithMany() 
                .HasForeignKey(d => d.roditeljId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
