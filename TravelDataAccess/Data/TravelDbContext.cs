using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Entities;

namespace TravelDataAccess.Data;

public class TravelDbContext(DbContextOptions<TravelDbContext> options) : DbContext(options)
{
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.ToTable("Trip", table =>
            {
                table.HasCheckConstraint("CK_Trip_Price", "Price >= 0");
                table.HasCheckConstraint("CK_Trip_Status", "Status IN ('Available','Booked')");
            });

            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).HasColumnName("TripID");
            entity.Property(e => e.Code).HasMaxLength(30).IsUnicode(false).IsRequired();
            entity.Property(e => e.Destination).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(12,2)").IsRequired();
            entity.Property(e => e.Status).HasMaxLength(20).IsUnicode(false).IsRequired();
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer", table =>
            {
                table.HasCheckConstraint("CK_Customer_Age", "Age >= 0");
            });

            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).HasColumnName("CustomerID");
            entity.Property(e => e.Code).HasMaxLength(30).IsUnicode(false).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(200).IsUnicode(false);
            entity.Property(e => e.Age).IsRequired();
            entity.Property(e => e.Password).HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Booking", table =>
            {
                table.HasCheckConstraint("CK_Booking_Status", "Status IN ('Pending','Confirmed','Cancelled')");
            });

            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate).HasColumnType("date").HasDefaultValueSql("(GETDATE())").IsRequired();
            entity.Property(e => e.Status).HasMaxLength(20).IsUnicode(false).IsRequired();

            entity.HasOne(e => e.Trip)
                .WithMany(t => t.Bookings)
                .HasForeignKey(e => e.TripID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(e => e.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}