using Microsoft.EntityFrameworkCore;
using ReservationSystem.Models.Entities;

namespace ReservationSystem.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Barber)
                .WithMany(b => b.Reservations)
                .HasForeignKey(r => r.BarberId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
