using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TravelDataAccess.Models;

namespace TravelDataAccess.Data
{
    public class TravelDbContext : DbContext
    {
        public TravelDbContext()
        {
        }

        public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("TravelDb");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Trip configuration
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.TripID);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Price).HasPrecision(12, 2);

                entity.HasCheckConstraint("CK_Trip_Price", "[Price] >= 0");
                entity.HasCheckConstraint("CK_Trip_Status", "[Status] IN ('Available','Booked')");
            });

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasCheckConstraint("CK_Customer_Age", "[Age] >= 0");
            });

            // Booking configuration
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingID);

                entity.HasCheckConstraint("CK_Booking_Status", "[Status] IN ('Pending','Confirmed','Cancelled')");

                // Relationships
                entity.HasOne(d => d.Trip)
                      .WithMany(p => p.Bookings)
                      .HasForeignKey(d => d.TripID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Customer)
                      .WithMany(p => p.Bookings)
                      .HasForeignKey(d => d.CustomerID)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}